using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
using VolkminianosAPI.Context;
using VolkminianosAPI.Domain.Interfaces;
using VolkminianosAPI.Domain.Services;
using VolkminianosAPI.Infrastructure.Repositories;
using VolkminianosAPI.Infrastructure.Seed;
using VolkminianosAPI.Infrastructure.Services;

namespace VolkminianosAPI {
    public class Program {
        public static async Task Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers(options => {
                options.SuppressAsyncSuffixInActionNames = false;
            });
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

            // CORS: o front (outro endereço/porta) precisa de permissão para chamar a API.
            // Em produção, liste os endereços em "Cors:Origens" no appsettings.
            string[] origensPermitidas = builder.Configuration.GetSection("Cors:Origens").Get<string[]>() ?? [];
            builder.Services.AddCors(options => {
                options.AddPolicy("Front", policy => {
                    if (origensPermitidas.Length > 0) {
                        policy.WithOrigins(origensPermitidas).AllowAnyHeader().AllowAnyMethod();
                    } else {
                        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                    }
                });
            });

            // Repositories
            builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            builder.Services.AddScoped<IBairroRepository, BairroRepository>();
            builder.Services.AddScoped<IPontoRepository, PontoRepository>();
            builder.Services.AddScoped<ITarifaRepository, TarifaRepository>();

            // Services
            builder.Services.AddScoped<IUsuarioService, UsuarioService>();
            builder.Services.AddScoped<IBairroService, BairroService>();
            builder.Services.AddScoped<IPontoService, PontoService>();
            builder.Services.AddScoped<ITarifaService, TarifaService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) {
                app.MapOpenApi();

                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();

            app.UseCors("Front");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            await AdminSeeder.SeedAsync(app.Services);

            app.Run();
        }
    }
}
