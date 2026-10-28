using VolkminianosAPI.Models;

namespace VolkminianosAPI.Domain.Interfaces;

public interface ITarifaRepository {
    Task<IEnumerable<Tarifa>> ObterTodosAsync();
    Task<Tarifa?> ObterPorIdAsync(int id);
    // Espera o par já ordenado (bairroAId <= bairroBId). Quem normaliza é o service.
    Task<Tarifa?> ObterPorBairrosAsync(int bairroAId, int bairroBId);
    Task AdicionarAsync(Tarifa tarifa);
    void Atualizar(Tarifa tarifa);
    void Deletar(Tarifa tarifa);
    Task<bool> SalvarMudancasAsync();
}
