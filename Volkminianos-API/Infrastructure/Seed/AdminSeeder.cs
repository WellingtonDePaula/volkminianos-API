using Microsoft.EntityFrameworkCore;
using VolkminianosAPI.Context;
using VolkminianosAPI.Models;

namespace VolkminianosAPI.Infrastructure.Seed;

// Resolve o "ovo e a galinha": criar usuário exige ser Admin, mas ainda não existe nenhum.
// Só age se Admin:Email e Admin:Senha estiverem configurados E ainda não houver admin no banco.
public static class AdminSeeder {
    public static async Task SeedAsync(IServiceProvider services) {
        using var scope = services.CreateScope();
        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("AdminSeeder");

        string? email = config["Admin:Email"];
        string? senha = config["Admin:Senha"];
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha)) {
            return;
        }

        try {
            if (await context.Usuarios!.AnyAsync(u => u.Email == email && u.IsAdmin)) {
                return;
            }

            context.Usuarios!.Add(new Usuario {
                Nome = config["Admin:Nome"] ?? "Administrador",
                Email = email,
                Senha = BCrypt.Net.BCrypt.HashPassword(senha),
                IsAdmin = true
            });
            await context.SaveChangesAsync();

            logger.LogInformation("Usuário administrador inicial criado: {Email}", email);
        } catch (Exception ex) {
            logger.LogWarning(ex, "Não foi possível criar o administrador inicial.");
        }
    }
}
