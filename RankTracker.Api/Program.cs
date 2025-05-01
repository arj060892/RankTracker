using RankTracker.Persistence.Extensions;
using RankTracker.Service.Extensions;
using FluentValidation.AspNetCore;
using RankTracker.Api.Validators;
using FluentValidation;
using Microsoft.Playwright;
using System.Diagnostics;

namespace RankTracker.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            builder.Services
                .AddPersistence(builder.Configuration)
                .AddServices(builder.Configuration);

            builder.Services.AddControllers();

            builder.Services
                .AddValidatorsFromAssemblyContaining<SearchRequestDtoValidator>();

            builder.Services
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            app.UseCors("AllowAll");

            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapControllers();
            app.Run();
        }
    }
}