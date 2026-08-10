using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using VolkminianosAPI.Context;
using VolkminianosAPI.Domain.Interfaces;
using VolkminianosAPI.Infrastructure.Repositories;
using VolkminianosAPI.Services;
using VolkminianosAPI.Services.Interfaces;

namespace VolkminianosAPI {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();


            string mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AppDbContext>(options => {
                options.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection));
            });

            builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
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

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}