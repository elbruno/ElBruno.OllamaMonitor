using System.Text.Json.Serialization;

namespace ElBruno.OllamaMonitor.Ollama.Contracts;

public sealed record OllamaVersionResponse(
    [property: JsonPropertyName("version")] string Version);
