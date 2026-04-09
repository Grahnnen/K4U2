using ContentApi.Clients;
using ContentAPI.Data;
using ContentAPI.Filters;
using ContentAPI.Options;
using ContentAPI.Repositories;
using ContentAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;

namespace WebApplication1
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

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ExecutionTimeFilter>();
            });

            builder.Services.AddProblemDetails(options =>
            {
                options.CustomizeProblemDetails = context =>
                {
                    context.ProblemDetails.Instance = context.HttpContext.Request.Path;
                };
            });

            builder.Services.AddScoped<IAiContentRepository, AiContentRepository>();
            builder.Services.AddScoped<IAiContentService, AiContentService>();

            builder.Services.AddAuthorization();

            builder.Services.Configure<ServiceBOptions>(
                builder.Configuration.GetSection("ServiceB"));

            builder.Services.AddTransient<ServiceBApiKeyHandler>();

            builder.Services.AddHttpClient<LlmProxyClient>((sp, client) =>
            {
                var options = sp.GetRequiredService<IOptions<ServiceBOptions>>().Value;
                client.BaseAddress = new Uri(options.BaseUrl);
            })
            .AddHttpMessageHandler<ServiceBApiKeyHandler>();

            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite("Data Source=aicontent.db"));

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
            app.UseAuthorization();


            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();
            }

            app.Run();
        }
    }
}
