using System.Text.Json.Serialization;

namespace ZmaReference.Features.Docs.Models;

public record DocumentCategory
{
    public string Name { get; init; } = "";
    public List<Document> Documents { get; init; } = [];
}
