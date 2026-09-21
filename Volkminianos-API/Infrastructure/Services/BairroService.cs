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

        return new BairroDto {
            Id = novoBairro.Id,
            Nome = novoBairro.Nome,
            Ativo = novoBairro.Ativo
        };
    }

    public async Task<bool> DeletarAsync(int id) {
        Bairro? bairro = await _repository.ObterPorIdAsync(id);
        if(bairro is null) {
            return false;
        }
        _repository.Deletar(bairro);
        return await _repository.SalvarMudancasAsync();
    }

    public async Task<BairroDto?> ObterPorIdAsync(int id) {
        Bairro? bairro = await _repository.ObterPorIdAsync(id);
        if(bairro is null) {
            return null;
        }

        return new BairroDto {
            Id = bairro.Id,
            Nome = bairro.Nome,
            Ativo = bairro.Ativo
        };
    }

    public async Task<IEnumerable<BairroDto>> ObterTodosAsync() {
        IEnumerable<Bairro> bairros = await _repository.ObterTodosAsync();
        return bairros.Select(b => new BairroDto {
            Id = b.Id,
            Nome = b.Nome,
            Ativo = b.Ativo
        });
    }
}
