using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace KaptchaNET.Swagger
{
    public static class Utility
    {
        public static void AddCustomizedSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            var swaggerDoc =  configuration.GetSection("SwaggerDoc").Get<SwaggerDocOptions>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(swaggerDoc.Version, new OpenApiInfo {
                    Title = swaggerDoc.Title,
                    Description = swaggerDoc.Description,
                    Version = swaggerDoc.Version,
                    Contact = new OpenApiContact {
                        Email = swaggerDoc.Contact.Email,
                        Name = swaggerDoc.Contact.Name,
                        Url = new Uri(swaggerDoc.Contact.Url)
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
                options.CustomSchemaIds(o => o.FullName);
            });
        }


        public static void UseSwaggerAndSwaggerUI(this IApplicationBuilder app, IConfiguration configuration)
        {
            var swaggerDoc =  configuration.GetSection("SwaggerDoc").Get<SwaggerDocOptions>();

            app.UseSwagger();

            app.UseSwaggerUI(options => {
                options.SwaggerEndpoint($"/swagger/{swaggerDoc.Version}/swagger.json", swaggerDoc.Title);
                options.RoutePrefix = "swagger";
                options.DocExpansion(DocExpansion.List);
            });
        }
    }
}
