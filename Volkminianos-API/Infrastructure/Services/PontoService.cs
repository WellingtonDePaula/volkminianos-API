using VolkminianosAPI.Domain.Interfaces;
using VolkminianosAPI.Domain.Services;
using VolkminianosAPI.DTOs.Ponto;
using VolkminianosAPI.Models;

namespace VolkminianosAPI.Infrastructure.Services;

public class PontoService : IPontoService {
    private readonly IPontoRepository _repository;
    private readonly IBairroRepository _bairroRepository;

    public PontoService(IPontoRepository repository, IBairroRepository bairroRepository) {
        _repository = repository;
        _bairroRepository = bairroRepository;
    }

    public async Task<IEnumerable<PontoDto>> ObterTodosAsync() {
        var pontos = await _repository.ObterTodosAsync();
        return pontos.Select(MapearParaDto);
    }

    public async Task<PontoDto?> ObterPorIdAsync(int id) {
        Ponto? ponto = await _repository.ObterPorIdAsync(id);
        return ponto is null ? null : MapearParaDto(ponto);
    }

    public async Task<IEnumerable<PontoDto>> ObterPorBairroIdAsync(int bairroId) {
        var pontos = await _repository.ObterPorBairroIdAsync(bairroId);
        return pontos.Select(MapearParaDto);
    }

    public async Task<PontoDto?> ObterPorNomeAsync(string nome) {
        Ponto? ponto = await _repository.ObterPorNomeAsync(nome);
        return ponto is null ? null : MapearParaDto(ponto);
    }

    public async Task<PontoDto> CriarAsync(CriarPontoDto dto) {
        await GarantirQueBairroExisteAsync(dto.BairroId);
        await GarantirQueNomeEstaDisponivelAsync(dto.Nome, idAtual: null);

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

        return MapearParaDto(ponto);
    }

    public async Task<bool> AtualizarAsync(int id, AtualizarPontoDto dto) {
        Ponto? ponto = await _repository.ObterPorIdAsync(id);
        if (ponto is null) {
            return false;
        }

        await GarantirQueBairroExisteAsync(dto.BairroId);
        await GarantirQueNomeEstaDisponivelAsync(dto.Nome, idAtual: id);

        ponto.Nome = dto.Nome;
        ponto.Endereco = dto.Endereco;
        ponto.BairroId = dto.BairroId;
        ponto.Latitude = dto.Latitude;
        ponto.Longitude = dto.Longitude;
        ponto.PontoTuristico = dto.PontoTuristico;
        ponto.Descricao = dto.Descricao;
        ponto.Ativo = dto.Ativo;
        ponto.AtualizadoEm = DateTime.UtcNow;

        _repository.Atualizar(ponto);
        return await _repository.SalvarMudancasAsync();
    }

    public async Task<bool> DeletarAsync(int id) {
        Ponto? ponto = await _repository.ObterPorIdAsync(id);
        if (ponto is null) {
            return false;
        }

        _repository.Deletar(ponto);
        return await _repository.SalvarMudancasAsync();
    }

    private async Task GarantirQueBairroExisteAsync(int bairroId) {
        if (await _bairroRepository.ObterPorIdAsync(bairroId) is null) {
            throw new KeyNotFoundException("O bairro informado não existe.");
        }
    }

    private async Task GarantirQueNomeEstaDisponivelAsync(string nome, int? idAtual) {
        Ponto? existente = await _repository.ObterPorNomeAsync(nome);
        if (existente is not null && existente.Id != idAtual) {
            throw new InvalidOperationException("Já existe um ponto cadastrado com este nome.");
        }
    }

    private static PontoDto MapearParaDto(Ponto ponto) {
        return new PontoDto {
            Id = ponto.Id,
            Nome = ponto.Nome,
            Endereco = ponto.Endereco,
            BairroId = ponto.BairroId,
            Latitude = ponto.Latitude,
            Longitude = ponto.Longitude,
            PontoTuristico = ponto.PontoTuristico,
            Descricao = ponto.Descricao,
            Ativo = ponto.Ativo,
            CriadoEm = ponto.CriadoEm,
            AtualizadoEm = ponto.AtualizadoEm
        };
    }
}
