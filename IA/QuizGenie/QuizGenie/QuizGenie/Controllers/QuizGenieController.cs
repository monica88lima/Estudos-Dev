using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using QuizGenie.Infraestrutura;
using QuizGenie.Infraestrutura.Modelos;
using QuizGenie.Service;
using static QuizGenie.Infraestrutura.Modelos.GeminiSettings;

namespace QuizGenie.Controllers;

[Route("api/[controller]")]
[ApiController]
public class QuizGenieController(
  IGeminiApi _geminiApi,
  IRedisService _redisService) : Controller
{
  
  [HttpPost]
  public async Task<List<Questoes>> GerarQuestaoAsync(string tema,string serie,int idade,string intencionalidade,int tipoPergunta,int qtdQuestoes)
  {
    var prompt = QuizGenie_Service.GerarPergunta(tema, serie, idade, intencionalidade, tipoPergunta, qtdQuestoes );

    var geminiConfigurations = GeminiSttingsConfigurations.geminiSettings;
    
    var request = new GeminiRequest
    {
      contents = new List<Content>
      {
        new Content
        {
          parts = new List<Part>
          {
            new Part { Text = prompt }
          }
        }
      }
    };

    string json = JsonSerializer.Serialize(request);

    var resposta = await _geminiApi.GerarConteudoAsync(
      geminiConfigurations.Model, 
      request, 
      geminiConfigurations.ApiKey);
    
    var chave_id =  $"quiz:{Guid.NewGuid()}";
    var retorno  = await resposta.Content.ReadAsStringAsync();
    
    var geminiResponse = JsonSerializer.Deserialize<GeminiResponse>(retorno);
    var rawText = geminiResponse?.candidates?[0]?.content?.parts?[0]?.text;

// Remove a marcação ```json e quebra de linha
    var cleanedJson = rawText
      .Replace("```json", "")
      .Replace("```", "")
      .Trim();
    var perguntas = JsonSerializer.Deserialize<List<Questoes>>(cleanedJson);
    await  _redisService.SetAsync(chave_id, cleanedJson);
    return perguntas ;
  }

}