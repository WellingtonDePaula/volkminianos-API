using VolkminianosAPI.DTOs.Ponto;

namespace VolkminianosAPI.Domain.Services;

public interface IPontoService {
    Task<IEnumerable<PontoDto>> ObterTodosAsync();
    Task<PontoDto?> ObterPorIdAsync(int id);
    Task<IEnumerable<PontoDto>> ObterPorBairroIdAsync(int bairroId);
    Task<PontoDto> ObterPorNomeAsync(string nome);
    Task<PontoDto> CriarAsync(CriarPontoDto dto);
    Task<bool> AtualizarAsync(int id, AtualizarPontoDto dto);
    Task<bool> DeletarAsync(int id);
}