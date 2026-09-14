using VolkminianosAPI.Models;

namespace VolkminianosAPI.Domain.Interfaces;

public interface IBairroRepository {
    Task<IEnumerable<Bairro>> ObterTodosAsync();
    Task<Bairro?> ObterPorIdAsync(int id);
    Task AdicionarAsync(Bairro usuario);
    void Atualizar(Bairro usuario);
    void Deletar(Bairro usuario);
    Task<bool> SalvarMudancasAsync();
}
