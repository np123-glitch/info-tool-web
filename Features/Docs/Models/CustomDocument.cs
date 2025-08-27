using System.Text.Json.Serialization;

namespace ZmaReference.Features.Docs.Models;

public record CustomDocument
{
    public string Name { get; set; } = "";

    public string Url { get; set; } = "";
}
