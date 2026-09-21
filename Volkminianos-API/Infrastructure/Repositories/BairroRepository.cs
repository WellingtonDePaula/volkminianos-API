using Microsoft.EntityFrameworkCore;
using VolkminianosAPI.Context;
using VolkminianosAPI.Domain.Interfaces;
using VolkminianosAPI.Models;

namespace VolkminianosAPI.Infrastructure.Repositories {
    public class BairroRepository : IBairroRepository {
        private readonly AppDbContext _context;

        public BairroRepository(AppDbContext context) {
            _context = context;
        }
        public async Task AdicionarAsync(Bairro bairro) {
            await _context.Bairros!.AddAsync(bairro);
        }

        public void Atualizar(Bairro bairro) {
            _context.Bairros!.Update(bairro);
        }

        public void Deletar(Bairro bairro) {
            _context.Bairros!.Remove(bairro);
        }

        public async Task<Bairro?> ObterPorIdAsync(int id) {
            return await _context.Bairros!.FindAsync(id);
        }

        public async Task<Bairro?> ObterPorNomeAsync(string nome) {
            return await _context.Bairros!.AsNoTracking().FirstOrDefaultAsync(u => u.Nome == nome);
        }

        public async Task<IEnumerable<Bairro>> ObterTodosAsync() {
            return await _context.Bairros!.AsNoTracking().ToListAsync();
        }

        public async Task<bool> SalvarMudancasAsync() {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
