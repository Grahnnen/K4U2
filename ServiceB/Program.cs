using LLMProxy.Authentication;
using LLMProxy.Clients;
using LLMProxy.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;

namespace ServiceB
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseDefaultServiceProvider(options =>
            {
                options.ValidateOnBuild = true;
                options.ValidateScopes = true;
            });
            builder.Services.Configure<InternalApiKeyOptions>(
            builder.Configuration.GetSection("InternalApiKey"));

            builder.Services.Configure<ExternalLlmOptions>(
                builder.Configuration.GetSection("ExternalLlm"));

            builder.Services
                .AddAuthentication("ApiKey")
                .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(
                    "ApiKey", _ => { });

            builder.Services.Configure<OpenAiOptions>(
            builder.Configuration.GetSection("OpenAI"));

            builder.Services.AddTransient<OpenAiAuthHandler>();

            builder.Services.AddHttpClient<OpenAiClient>((sp, client) =>
            {
                var options = sp.GetRequiredService<IOptions<OpenAiOptions>>().Value;
                client.BaseAddress = new Uri(options.BaseUrl);
            })
            .AddHttpMessageHandler<OpenAiAuthHandler>();

            builder.Services.AddAuthorization();

            builder.Services.AddHttpClient();
            builder.Services.AddControllers();

            builder.Services.AddProblemDetails(options =>
            {
                options.CustomizeProblemDetails = context =>
                {
                    context.ProblemDetails.Instance = context.HttpContext.Request.Path;
                };
            });

            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }
            app.UseExceptionHandler();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
