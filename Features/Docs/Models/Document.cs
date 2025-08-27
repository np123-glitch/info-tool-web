using System.Text.Json.Serialization;

namespace ZmaReference.Features.Docs.Models;

public readonly record struct Document(string Name, string Url);
