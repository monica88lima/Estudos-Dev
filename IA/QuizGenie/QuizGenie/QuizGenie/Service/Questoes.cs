using System.Text.Json.Serialization;

namespace QuizGenie.Service;

public class Questoes
{
    [JsonPropertyName("pergunta")]
    public string Pergunta { get; set; }
    [JsonPropertyName("justificativa")]
    public string Justificativa { get; set; }
    [JsonPropertyName("alternativas")]
    public List<Alternativas> Alternativas { get; set; }
}

public class Alternativas
{
    [JsonPropertyName("texto")]
    public string Texto { get; set; }
    [JsonPropertyName("correta")]
    public bool Correta { get; set; }
}