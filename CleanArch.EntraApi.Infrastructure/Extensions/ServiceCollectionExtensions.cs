using CleanArch.EntraApi.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.EntraApi.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add Controllers
        services.AddControllers();

        // Bind AzureAd configuration to Options
        services.Configure<AzureAdOptions>(configuration.GetSection(AzureAdOptions.SectionName));

        // Add Entra ID authentication
        services.AddMicrosoftIdentityWebApiAuthentication(configuration, AzureAdOptions.SectionName);

        // Configure Authorization policies
        services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
            options.AddPolicy("User", policy => policy.RequireRole("User"));
        });

        // Configure Swagger/OpenAPI
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            var azureAdOptions = configuration.GetSection(AzureAdOptions.SectionName).Get<AzureAdOptions>()
                ?? throw new InvalidOperationException("AzureAd configuration is missing.");

            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "CleanArch.EntraApi.WebApi",
                Version = "v1",
                Description = "API for CleanArch.EntraApi.WebApi"
            });

            // Use Implicit flow instead of Authorization Code flow
            c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri($"{azureAdOptions.Instance}{azureAdOptions.TenantId}/oauth2/v2.0/authorize"),
                        Scopes = new Dictionary<string, string>
                    {
                        { $"api://{azureAdOptions.ClientId}/access_as_user", "Access API" }
                    }
                    }
                }
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                },
                new[] { $"api://{azureAdOptions.ClientId}/access_as_user" }
            }
        });

            c.CustomSchemaIds(type => type.FullName);
        });

        return services;
    }
}
