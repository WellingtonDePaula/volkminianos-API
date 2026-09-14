using VolkminianosAPI.Models;

namespace VolkminianosAPI.Domain.Interfaces;

public interface IUsuarioRepository {
    Task<IEnumerable<Usuario>> ObterTodosAsync();
    Task<Usuario?> ObterPorIdAsync(int id);
    Task<Usuario?> ObterPorEmailAsync(string email);
    Task AdicionarAsync(Usuario usuario);
    void Atualizar(Usuario usuario);
    void Deletar(Usuario usuario);
    Task<bool> SalvarMudancasAsync();
}