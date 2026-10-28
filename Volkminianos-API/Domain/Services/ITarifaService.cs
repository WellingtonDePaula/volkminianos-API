using VolkminianosAPI.DTOs.Tarifa;

namespace VolkminianosAPI.Domain.Services;

public interface ITarifaService {
    Task<IEnumerable<TarifaDto>> ObterTodosAsync();
    Task<TarifaDto?> ObterPorIdAsync(int id);
    Task<ConsultaTarifaDto> ConsultarAsync(int origemPontoId, int destinoPontoId);
    Task<TarifaDto> CriarAsync(CriarTarifaDto dto);
    Task<bool> AtualizarAsync(int id, AtualizarTarifaDto dto);
    Task<bool> DeletarAsync(int id);
}
