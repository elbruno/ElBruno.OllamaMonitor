using System.Text.Json.Serialization;

namespace ElBruno.OllamaMonitor.Ollama.Contracts;

public sealed record OllamaTagsResponse(
    [property: JsonPropertyName("models")] IReadOnlyList<OllamaTagModelResponse> Models);
