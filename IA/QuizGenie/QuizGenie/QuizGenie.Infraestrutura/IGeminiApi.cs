using QuizGenie.Infraestrutura.Modelos;
using Refit;
using System.Threading.Tasks;

namespace QuizGenie.Infraestrutura;

public interface IGeminiApi
{
    [Post("/v1beta/models/{model}:generateContent")]
    Task<HttpResponseMessage> GerarConteudoAsync(
        [AliasAs("model")] string model,
        [Body] GeminiRequest request,
        [Query] string key
    );

}
