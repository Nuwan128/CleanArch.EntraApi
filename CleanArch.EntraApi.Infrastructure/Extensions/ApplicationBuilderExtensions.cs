using CleanArch.EntraApi.Infrastructure.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace CleanArch.EntraApi.Infrastructure.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder ConfigureWebApiPipeline(this IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                var azureAdOptions = configuration.GetSection(AzureAdOptions.SectionName).Get<AzureAdOptions>()
                    ?? throw new InvalidOperationException("AzureAd configuration is missing.");

                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CleanArch.EntraApi.WebApi v1");
                c.OAuthClientId(azureAdOptions.ClientId);
                // Corrected method call to OAuthUsePkce with the correct overload
                c.OAuthUsePkce(); // Removed the argument as the method likely does not take any parameters
            });
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => endpoints.MapControllers());

        return app;
    }   
}
