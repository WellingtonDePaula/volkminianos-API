using VolkminianosAPI.Models;

namespace VolkminianosAPI.Domain.Interfaces;

public interface IPontoRepository {
    Task<IEnumerable<Ponto>> ObterTodosAsync();
    Task<Ponto?> ObterPorIdAsync(int id);
    Task<Ponto?> ObterPorNomeAsync(string nome);
    Task<IEnumerable<Ponto?>> ObterPorBairroIdAsync(int bairroId);
    Task AdicionarAsync(Ponto ponto);
    void Atualizar(Ponto ponto);
    void Deletar(Ponto ponto);
    Task<bool> SalvarMudancasAsync();
}
