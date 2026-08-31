using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
using VolkminianosAPI.Context;
using VolkminianosAPI.Domain.Interfaces;
using VolkminianosAPI.Repositories;
using VolkminianosAPI.Services;

namespace VolkminianosAPI {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi(options => {
                options.AddDocumentTransformer((document, context, cancellationToken) => {
                    document.Components ??= new();

                    document.Components.SecuritySchemes.Add("Bearer", new() {
                        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT"
                    });

                    document.SecurityRequirements = new[] {
                        new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                        {
                            {
                                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                                {
                                    Reference = new Microsoft.OpenApi.Models.OpenApiReference
                                    {
                                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    }
                                },
                                Array.Empty<string>()
                            }
                        }
                    };

                    return Task.CompletedTask;
                });
            });


            string mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AppDbContext>(options => {
                options.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection));
            });

            // JWT
            var jwtChave = builder.Configuration["Jwt:Chave"]!;

            builder.Services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["Jwt:Emissor"],
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtChave))
                };
            });

            // Repositories
            builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

            // Services
            builder.Services.AddScoped<IUsuarioService, UsuarioService>();

            builder.Services.AddControllers(options => {
                options.SuppressAsyncSuffixInActionNames = false;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) {
                app.MapOpenApi();

                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}