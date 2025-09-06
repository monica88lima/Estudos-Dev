using QuizGenie.Infraestrutura;
using QuizGenie.Infraestrutura.Modelos;
using Refit;
using StackExchange.Redis;
using static QuizGenie.Infraestrutura.Modelos.GeminiSettings;

namespace QuizGenie;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        string? conexaoRedis = builder.Configuration.GetConnectionString("redis");
        if(string.IsNullOrEmpty(conexaoRedis))
            throw new InvalidOperationException("Redis connection string is missing.");
        
        builder.Services.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(conexaoRedis)
        );

        builder.Services.AddScoped<IRedisService, RedisService>();
        GeminiSttingsConfigurations.geminiSettings = builder.Configuration.GetSection("Gemini").Get<GeminiSettings>() ?? new GeminiSettings();
        
        builder.Services.AddRefitClient<IGeminiApi>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri("https://generativelanguage.googleapis.com");
                
            });


        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}