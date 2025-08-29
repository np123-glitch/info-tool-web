using Coravel.Invocable;
using Microsoft.Extensions.Options;
using ZmaReference.Features.Docs.Models;
using ZmaReference.Features.Docs.Repositories;

namespace ZmaReference.Features.Docs.ScheduledJobs;

public class FetchAndStoreDocs(ILogger<FetchAndStoreDocs> logger, IHttpClientFactory httpClientFactory, IWebHostEnvironment webHostEnvironment, IOptionsMonitor<AppSettings> appSettings, DocumentRepository documentRepository) : IInvocable
{
    public async Task Invoke()
    {
        var client = httpClientFactory.CreateClient();
        List<DocumentCategory> compiledDocCategories = [];

        try
        {
            logger.LogInformation("Fetching ZMA docs from {url}",
                appSettings.CurrentValue.Urls.ZmaDocumentsApiEndpoint);
            var apiResponse = await client.GetFromJsonAsync<ZmaDocumentsApiResponse>(appSettings.CurrentValue.Urls.ZmaDocumentsApiEndpoint);
            
            if (apiResponse?.Data is not null && apiResponse.RetDet.Code == 200)
            {
                // Filter out unwanted categories and sort by priority
                var filteredAndSortedCategories = apiResponse.Data
                    .Where(c => c.Name is "eloa" or "iloa" or "mfr" or "references" or "training")
                    .Select(c => c.ToGenericDocumentCategory())
                    .OrderBy(c => GetCategorySortOrder(c.Name))
                    .ToList();

                compiledDocCategories.AddRange(filteredAndSortedCategories);
                logger.LogInformation("Successfully fetched {count} document categories from ZMA API: {categories}", 
                    filteredAndSortedCategories.Count, 
                    string.Join(", ", filteredAndSortedCategories.Select(c => c.Name)));
            }
            else
            {
                logger.LogWarning("Fetched ZMA documents null, zero, or API returned error code: {code}", apiResponse?.RetDet.Code ?? -1);
            }

            var customDocCategories = appSettings.CurrentValue.CustomDocuments
                .Where(c => c.Name != "Quick Reference") // Filter out Quick Reference
                .Select(c => c.ToGenericDocumentCategory());
            compiledDocCategories.AddRange(customDocCategories);

            logger.LogInformation("Successfully fetched ZMA and custom docs. Final categories: {categories}", 
                string.Join(", ", compiledDocCategories.Select(c => c.Name)));
        }
        catch (Exception e)
        {
            logger.LogError("Error while fetching ZMA docs: {ex}", e.ToString());
        }

        logger.LogInformation("Using direct PDF URLs - no local storage needed");
        documentRepository.ClearAllDocumentCategories();
        documentRepository.AddDocumentCategories(compiledDocCategories);
    }

    private static int GetCategorySortOrder(string categoryName)
    {
        return categoryName switch
        {
            "Directive Documents" => 1,  // D## documents first
            "Int. LOAs" => 2,           // Internal LOAs second
            "Ext. LOAs" => 3,           // External LOAs third
            "References" => 4,          // References fourth
            "Training" => 5,            // Training documents last
            _ => 999                    // Any other categories at the end
        };
    }
}