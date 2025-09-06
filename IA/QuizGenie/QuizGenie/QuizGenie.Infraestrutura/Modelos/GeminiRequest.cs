using System.Text.Json.Serialization;

namespace QuizGenie.Infraestrutura.Modelos;


public static class GeminiSttingsConfigurations
{
    public static GeminiSettings geminiSettings { get; set; }
}
public class GeminiSettings
{
    public string ApiKey { get; set; }
    public string Model { get; set; }
}

/// <summary>
/// Representa tanto a configuração lida do appsettings.json quanto o corpo da requisição para a API Gemini.
/// </summary>
public class GeminiRequest
{
    // --- Propriedades para o CORPO DA REQUISIÇÃO (enviadas para a API) ---
    // O atributo [JsonPropertyName] garante que o nome no JSON gerado seja exatamente "contents".
    [JsonPropertyName("contents")]
    public List<Content> contents { get; set; } = [];
}

/// <summary>
/// Parte do corpo da requisição, representa um bloco de conteúdo.
/// </summary>
public class Content
{
    [JsonPropertyName("parts")]
    public List<Part> parts { get; set; } = [];
}

/// <summary>
/// A menor unidade de conteúdo, contém o texto do prompt.
/// </summary>
public class Part
{
    [JsonPropertyName("text")]
    public string Text {
        get;
        set;
    }
}
