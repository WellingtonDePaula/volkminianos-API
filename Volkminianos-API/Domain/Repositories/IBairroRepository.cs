using VolkminianosAPI.Models;

namespace VolkminianosAPI.Domain.Interfaces;

public interface IBairroRepository {
    Task<IEnumerable<Bairro>> ObterTodosAsync();
    Task<Bairro?> ObterPorIdAsync(int id);
    //Task<Ponto?> ObterPontosAsync();
    Task AdicionarAsync(Bairro bairro);
    void Atualizar(Bairro bairro);
    void Deletar(Bairro bairro);
    Task<bool> SalvarMudancasAsync();
}
