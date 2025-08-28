using System.Text.Json.Serialization;

namespace ZmaReference.Features.Docs.Models;

public record ZmaDocument
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("details")]
    public string Details { get; set; } = "";

    [JsonPropertyName("updated_at")]
    public string UpdatedAt { get; set; } = "";

    [JsonPropertyName("url")]
    public string Url { get; set; } = "";

    [JsonPropertyName("permalink")]
    public string Permalink { get; set; } = "";
}
