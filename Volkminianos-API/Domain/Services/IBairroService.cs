using VolkminianosAPI.DTOs.Bairro;

namespace VolkminianosAPI.Domain.Services;

public interface IBairroService {
    Task<IEnumerable<BairroDto>> ObterTodosAsync();
    Task<BairroDto?> ObterPorIdAsync(int id);
    Task<BairroDto> CriarAsync(CriarBairroDto dto);
    Task<bool> AtualizarAsync(int id, AtualizarUsuarioDto dto);
    Task<bool> DeletarAsync(int id);
}