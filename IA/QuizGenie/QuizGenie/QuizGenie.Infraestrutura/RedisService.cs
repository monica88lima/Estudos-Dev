using Polly;
using StackExchange.Redis;

namespace QuizGenie.Infraestrutura;

public class RedisService: IRedisService
{
    private readonly IDatabase _database;

    public RedisService(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();

    }
    public Task SetAsync(string key, string value)
    {
        return  _database.StringSetAsync(key, value);
    }

    public async Task<string?> GetAsync(string key)
    {
        

        //Retry: tenta até 3 vezes, com espera crescente entre as tentativas

        var retryPolicy = Policy
            .Handle<RedisConnectionException>()
            .Or<RedisTimeoutException>()
            .WaitAndRetryAsync(3, attempt => TimeSpan.FromMilliseconds(200 * attempt));
        //se 2 falhas consecutivas ocorrerem, ele “abre o circuito” e bloqueia novas chamadas por 30 segundos
       
        var circuitBreakerPolicy = Policy
            .Handle<RedisConnectionException>()
            .CircuitBreakerAsync(2, TimeSpan.FromSeconds(30));

        var policyWrap = Policy.WrapAsync(retryPolicy, circuitBreakerPolicy);
        //- Policy Wrap: combina os dois comportamentos em uma única execução

        return await policyWrap.ExecuteAsync(async () =>
        {
            var result = await _database.StringGetAsync(key);
            return result.HasValue? result.ToString() : null;
            var redis = ConnectionMultiplexer.Connect("localhost:6379", Console.Out);
        });
            
       

        
    }


}