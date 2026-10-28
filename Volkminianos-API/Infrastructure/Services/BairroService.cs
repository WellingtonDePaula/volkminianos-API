using Microsoft.EntityFrameworkCore;
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

    public async Task<IEnumerable<BairroDto>> ObterTodosAsync() {
        IEnumerable<Bairro> bairros = await _repository.ObterTodosAsync();
        return bairros.Select(MapearParaDto);
    }

    public async Task<BairroDto?> ObterPorIdAsync(int id) {
        Bairro? bairro = await _repository.ObterPorIdAsync(id);
        return bairro is null ? null : MapearParaDto(bairro);
    }

    public async Task<BairroDto> CriarAsync(CriarBairroDto dto) {
        Bairro? bairroExistente = await _repository.ObterPorNomeAsync(dto.Nome);
        if (bairroExistente is not null) {
            throw new InvalidOperationException("Já existe um bairro cadastrado com este nome.");
        }

        Bairro novoBairro = new Bairro {
            Nome = dto.Nome,
            Ativo = true
        };
        await _repository.AdicionarAsync(novoBairro);
        await _repository.SalvarMudancasAsync();

        return MapearParaDto(novoBairro);
    }

    public async Task<bool> AtualizarAsync(int id, AtualizarBairroDto dto) {
        Bairro? bairro = await _repository.ObterPorIdAsync(id);
        if (bairro is null) {
            return false;
        }

        // Outro bairro (id diferente) já usa esse nome?
        Bairro? bairroComMesmoNome = await _repository.ObterPorNomeAsync(dto.Nome);
        if (bairroComMesmoNome is not null && bairroComMesmoNome.Id != id) {
            throw new InvalidOperationException("Já existe um bairro cadastrado com este nome.");
        }

        bairro.Nome = dto.Nome;
        bairro.Ativo = dto.Ativo;

        _repository.Atualizar(bairro);
        return await _repository.SalvarMudancasAsync();
    }

    public async Task<bool> DeletarAsync(int id) {
        Bairro? bairro = await _repository.ObterPorIdAsync(id);
        if (bairro is null) {
            return false;
        }

        _repository.Deletar(bairro);
        try {
            return await _repository.SalvarMudancasAsync();
        } catch (DbUpdateException) {
            // Pontos e Tarifas usam DeleteBehavior.Restrict: o banco recusa apagar bairro em uso.
            throw new InvalidOperationException(
                "Não é possível excluir o bairro: existem pontos ou tarifas vinculados a ele.");
        }
    }

    private static BairroDto MapearParaDto(Bairro bairro) {
        return new BairroDto {
            Id = bairro.Id,
            Nome = bairro.Nome,
            Ativo = bairro.Ativo
        };
    }
}
