using System.Text.Json.Serialization;

namespace ElBruno.OllamaMonitor.Ollama.Contracts;

public sealed record OllamaPsResponse(
    [property: JsonPropertyName("models")] IReadOnlyList<OllamaPsModelResponse> Models);
