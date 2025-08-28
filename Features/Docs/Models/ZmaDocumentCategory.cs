using System.Text.Json.Serialization;

namespace ZmaReference.Features.Docs.Models;

public class ZmaDocumentCategory
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("files")]
    public List<ZmaDocument> Documents { get; set; } = [];

    public DocumentCategory ToGenericDocumentCategory()
    {
        // Map category names to display names
        var displayName = Name switch
        {
            "eloa" => "Ext. LOAs",
            "iloa" => "Int. LOAs", 
            "mfr" => "Directive Documents",
            "references" => "References",
            "training" => "Training",
            _ => Name
        };

        return new DocumentCategory
        {
            Name = displayName,
            Documents = Documents.Select(d => new Document(d.Name, d.Url)).ToList()
        };
    }
}

public class ZmaDocumentsApiResponse
{
    [JsonPropertyName("ret_det")]
    public ZmaResponseDetails RetDet { get; set; } = new();

    [JsonPropertyName("data")]
    public List<ZmaDocumentCategory> Data { get; set; } = [];
}

public class ZmaResponseDetails
{
    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; } = "";
}
