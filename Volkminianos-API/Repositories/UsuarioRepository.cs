using Microsoft.EntityFrameworkCore;
using VolkminianosAPI.Context;
using VolkminianosAPI.Domain.Interfaces;
using VolkminianosAPI.Models;

namespace VolkminianosAPI.Repositories;

public class UsuarioRepository : IUsuarioRepository {
    private readonly AppDbContext _context;

    public UsuarioRepository(AppDbContext context) {
        _context = context;
    }

    public async Task<IEnumerable<Usuario>> ObterTodosAsync() {
        return await _context.Usuarios!.AsNoTracking().ToListAsync();
    }

    public async Task<Usuario?> ObterPorIdAsync(int id) {
        return await _context.Usuarios!.FindAsync(id);
    }

    public async Task<Usuario?> ObterPorEmailAsync(string email) {
        return await _context.Usuarios!.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task AdicionarAsync(Usuario usuario) {
        await _context.Usuarios!.AddAsync(usuario);
    }

    public void Atualizar(Usuario usuario) {
        _context.Usuarios!.Update(usuario);
    }

    public void Deletar(Usuario usuario) {
        _context.Usuarios!.Remove(usuario);
    }

    public async Task<bool> SalvarMudancasAsync() {
        return await _context.SaveChangesAsync() > 0;
    }
}