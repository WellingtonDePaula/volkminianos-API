using Microsoft.EntityFrameworkCore;
using VolkminianosAPI.Context;
using VolkminianosAPI.Domain.Interfaces;
using VolkminianosAPI.Models;

namespace VolkminianosAPI.Infrastructure.Repositories;

public class TarifaRepository : ITarifaRepository {
    private readonly AppDbContext _context;

    public TarifaRepository(AppDbContext context) {
        _context = context;
    }

    public async Task<IEnumerable<Tarifa>> ObterTodosAsync() {
        return await _context.Tarifas!
            .AsNoTracking()
            .Include(t => t.BairroA)
            .Include(t => t.BairroB)
            .OrderBy(t => t.BairroA.Nome)
            .ThenBy(t => t.BairroB.Nome)
            .ToListAsync();
    }

    // Com rastreamento (sem AsNoTracking) porque o service pode alterar e salvar.
    public async Task<Tarifa?> ObterPorIdAsync(int id) {
        return await _context.Tarifas!
            .Include(t => t.BairroA)
            .Include(t => t.BairroB)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Tarifa?> ObterPorBairrosAsync(int bairroAId, int bairroBId) {
        return await _context.Tarifas!
            .AsNoTracking()
            .Include(t => t.BairroA)
            .Include(t => t.BairroB)
            .FirstOrDefaultAsync(t => t.BairroAId == bairroAId && t.BairroBId == bairroBId);
    }

    public async Task AdicionarAsync(Tarifa tarifa) {
        await _context.Tarifas!.AddAsync(tarifa);
    }

    public void Atualizar(Tarifa tarifa) {
        // Marca só a tarifa como modificada. Update() percorreria também BairroA/BairroB.
        _context.Entry(tarifa).State = EntityState.Modified;
    }

    public void Deletar(Tarifa tarifa) {
        _context.Tarifas!.Remove(tarifa);
    }

    public async Task<bool> SalvarMudancasAsync() {
        return await _context.SaveChangesAsync() > 0;
    }
}
