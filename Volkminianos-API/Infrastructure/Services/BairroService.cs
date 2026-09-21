using VolkminianosAPI.Domain.Interfaces;
using VolkminianosAPI.Domain.Services;
using VolkminianosAPI.DTOs.Bairro;
using VolkminianosAPI.Models;

namespace VolkminianosAPI.Infrastructure.Services;

public class BairroService : IBairroService {
    private readonly IBairroRepository _repository;

    public BairroService(IBairroRepository repository) {
        _repository = repository;
    }

    public async Task<bool> AtualizarAsync(int id, AtualizarBairroDto dto) {
        Bairro? bairro = await _repository.ObterPorIdAsync(id);
        if(bairro is null) {
            return false;
        }

        bairro.Nome = dto.Nome;
        bairro.Ativo = dto.Ativo;

        _repository.Atualizar(bairro);
        return await _repository.SalvarMudancasAsync();
    }

    public Task<BairroDto> CriarAsync(CriarBairroDto dto) {
        throw new NotImplementedException();
    }

    public Task<bool> DeletarAsync(int id) {
        throw new NotImplementedException();
    }

    public Task<BairroDto?> ObterPorIdAsync(int id) {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<BairroDto>> ObterTodosAsync() {
        throw new NotImplementedException();
    }
}
