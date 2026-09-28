using Microsoft.EntityFrameworkCore;
using VolkminianosAPI.Context;
using VolkminianosAPI.Domain.Interfaces;
using VolkminianosAPI.Models;

namespace VolkminianosAPI.Infrastructure.Repositories {
    public class PontoRepository : IPontoRepository {
        private readonly AppDbContext _context;

        public PontoRepository(AppDbContext context) {
            _context = context;
        }

        public async Task AdicionarAsync(Ponto ponto) {
            await _context.Pontos!.AddAsync(ponto);
        }

        public void Atualizar(Ponto ponto) {
            _context.Pontos!.Update(ponto);
        }

        public void Deletar(Ponto ponto) {
            _context.Pontos!.Remove(ponto);
        }

        public async Task<IEnumerable<Ponto?>> ObterPorBairroIdAsync(int bairroId) {
            return await _context.Pontos!.AsNoTracking().Where(p => p.BairroId == bairroId).ToListAsync();
        }

        public async Task<Ponto?> ObterPorIdAsync(int id) {
            return await _context.Pontos!.FindAsync(id);
        }

        public async Task<Ponto?> ObterPorNomeAsync(string nome) {
            return await _context.Pontos!.AsNoTracking().FirstOrDefaultAsync(p => p.Nome == nome);
        }

        public async Task<IEnumerable<Ponto>> ObterTodosAsync() {
            return await _context.Pontos!.AsNoTracking().ToListAsync();
        }

        public async Task<bool> SalvarMudancasAsync() {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
