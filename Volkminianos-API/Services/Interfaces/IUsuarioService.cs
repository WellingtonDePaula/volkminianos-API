using VolkminianosAPI.DTOs.Usuario;

namespace VolkminianosAPI.Services.Interfaces;

public interface IUsuarioService {
    Task<IEnumerable<UsuarioDto>> ObterTodosAsync();
    Task<UsuarioDto?> ObterPorIdAsync(int id);
    Task<UsuarioDto?> ObterPorEmailAsync(string email);
    Task<UsuarioDto> CriarAsync(CriarUsuarioDto dto);
    Task<bool> AtualizarAsync(int id, AtualizarUsuarioDto dto);
    Task<bool> DeletarAsync(int id);
}