using VolkminianosAPI.Domain.Interfaces;
using VolkminianosAPI.Domain.Services;
using VolkminianosAPI.DTOs.Ponto;
using VolkminianosAPI.Models;

namespace VolkminianosAPI.Infrastructure.Services;

public class PontoService : IPontoService {
    private readonly IPontoRepository _repository;

    public PontoService(IPontoRepository repository) {
        _repository = repository;
    }

    public async Task<bool> AtualizarAsync(int id, AtualizarPontoDto dto) {
        Ponto? ponto = await _repository.ObterPorIdAsync(id);

        if (ponto == null) {
            return false;
        }

        ponto.Nome = dto.Nome;
        ponto.Endereco = dto.Endereco;
        ponto.BairroId = dto.BairroId;
        ponto.Latitude = dto.Latitude;
        ponto.Longitude = dto.Longitude;
        ponto.PontoTuristico = dto.PontoTuristico;
        ponto.Descricao = dto.Descricao;
        ponto.Ativo = dto.Ativo;

        _repository.Atualizar(ponto);
        return await _repository.SalvarMudancasAsync();
    }

    public async Task<PontoDto> CriarAsync(CriarPontoDto dto) {
        Ponto? pontoExistente = await _repository.ObterPorNomeAsync(dto.Nome);
        if (pontoExistente is not null) {
            throw new InvalidOperationException("Já existe um bairro cadastrado com este nome.");
        }

        Ponto ponto = new Ponto {
            Nome = dto.Nome,
            Endereco = dto.Endereco,
            BairroId = dto.BairroId,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            PontoTuristico = dto.PontoTuristico,
            Descricao = dto.Descricao,
            Ativo = dto.Ativo
        };
        await _repository.AdicionarAsync(ponto);
        await _repository.SalvarMudancasAsync();

        PontoDto pontoDto = new PontoDto {
            Nome = ponto.Nome,
            Endereco = ponto.Endereco,
            Latitude = ponto.Latitude,
            Longitude = ponto.Longitude,
            PontoTuristico = ponto.PontoTuristico,
            Descricao = ponto.Descricao,
            Ativo = ponto.Ativo,
            CriadoEm = ponto.CriadoEm,
            AtualizadoEm = ponto.AtualizadoEm
        };

        return pontoDto;
    }

    public async Task<bool> DeletarAsync(int id) {
        Ponto? ponto = await _repository.ObterPorIdAsync(id);
        if(ponto is null) {
            return false;
        }
        _repository.Deletar(ponto);
        return true;
    }

    public async Task<IEnumerable<PontoDto>> ObterPorBairroIdAsync(int bairroId) {
        var pontos = await _repository.ObterPorBairroIdAsync(bairroId);

        if(pontos is null) {
            return Enumerable.Empty<PontoDto>();
        }

        var pontoDtos = pontos.Select(ponto => new PontoDto {
            Nome = ponto.Nome,
            Endereco = ponto.Endereco,
            Latitude = ponto.Latitude,
            Longitude = ponto.Longitude,
            PontoTuristico = ponto.PontoTuristico,
            Descricao = ponto.Descricao,
            Ativo = ponto.Ativo,
            CriadoEm = ponto.CriadoEm,
            AtualizadoEm = ponto.AtualizadoEm
        });

        return pontoDtos;
    }

    public async Task<PontoDto?> ObterPorIdAsync(int id) {
        Ponto? ponto = await _repository.ObterPorIdAsync(id);
        if (ponto is null) {
            return null;
        }

        return new PontoDto {
            Nome = ponto.Nome,
            Endereco = ponto.Endereco,
            Latitude = ponto.Latitude,
            Longitude = ponto.Longitude,
            PontoTuristico = ponto.PontoTuristico,
            Descricao = ponto.Descricao,
            Ativo = ponto.Ativo,
            CriadoEm = ponto.CriadoEm,
            AtualizadoEm = ponto.AtualizadoEm
        };
    }

    public async Task<PontoDto> ObterPorNomeAsync(string nome) {
        Ponto? ponto = await _repository.ObterPorNomeAsync(nome);
        if(ponto is null) {
            throw new InvalidOperationException("Não existe um ponto cadastrado com este nome.");
        }

        return new PontoDto {
            Nome = ponto.Nome,
            Endereco = ponto.Endereco,
            Latitude = ponto.Latitude,
            Longitude = ponto.Longitude,
            PontoTuristico = ponto.PontoTuristico,
            Descricao = ponto.Descricao,
            Ativo = ponto.Ativo,
            CriadoEm = ponto.CriadoEm,
            AtualizadoEm = ponto.AtualizadoEm
        };
    }

    public async Task<IEnumerable<PontoDto>> ObterTodosAsync() {
        var pontos = await _repository.ObterTodosAsync();
        
        return pontos.Select(ponto => new PontoDto {
            Nome = ponto.Nome,
            Endereco = ponto.Endereco,
            Latitude = ponto.Latitude,
            Longitude = ponto.Longitude,
            PontoTuristico = ponto.PontoTuristico,
            Descricao = ponto.Descricao,
            Ativo = ponto.Ativo,
            CriadoEm = ponto.CriadoEm,
            AtualizadoEm = ponto.AtualizadoEm
        });
    }
}
