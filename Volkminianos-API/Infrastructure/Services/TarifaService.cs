using VolkminianosAPI.Domain.Interfaces;
using VolkminianosAPI.Domain.Services;
using VolkminianosAPI.DTOs.Tarifa;
using VolkminianosAPI.Models;

namespace VolkminianosAPI.Infrastructure.Services;

public class TarifaService : ITarifaService {
    private readonly ITarifaRepository _repository;
    private readonly IBairroRepository _bairroRepository;
    private readonly IPontoRepository _pontoRepository;

    public TarifaService(
        ITarifaRepository repository,
        IBairroRepository bairroRepository,
        IPontoRepository pontoRepository) {
        _repository = repository;
        _bairroRepository = bairroRepository;
        _pontoRepository = pontoRepository;
    }

    public async Task<IEnumerable<TarifaDto>> ObterTodosAsync() {
        var tarifas = await _repository.ObterTodosAsync();
        return tarifas.Select(MapearParaDto);
    }

    public async Task<TarifaDto?> ObterPorIdAsync(int id) {
        Tarifa? tarifa = await _repository.ObterPorIdAsync(id);
        return tarifa is null ? null : MapearParaDto(tarifa);
    }

    // "Quanto custa ir do ponto X ao ponto Y?" -> descobre o bairro de cada ponto e busca a tarifa do par.
    public async Task<ConsultaTarifaDto> ConsultarAsync(int origemPontoId, int destinoPontoId) {
        Ponto origem = await _pontoRepository.ObterPorIdAsync(origemPontoId)
            ?? throw new KeyNotFoundException("Ponto de origem não encontrado.");
        Ponto destino = await _pontoRepository.ObterPorIdAsync(destinoPontoId)
            ?? throw new KeyNotFoundException("Ponto de destino não encontrado.");

        var (bairroAId, bairroBId) = NormalizarPar(origem.BairroId, destino.BairroId);

        Tarifa tarifa = await _repository.ObterPorBairrosAsync(bairroAId, bairroBId)
            ?? throw new KeyNotFoundException("Não há tarifa cadastrada entre os bairros desses pontos.");

        return new ConsultaTarifaDto {
            OrigemPontoId = origem.Id,
            OrigemPontoNome = origem.Nome,
            OrigemBairro = NomeDoBairro(tarifa, origem.BairroId),
            DestinoPontoId = destino.Id,
            DestinoPontoNome = destino.Nome,
            DestinoBairro = NomeDoBairro(tarifa, destino.BairroId),
            Valor = tarifa.Valor
        };
    }

    public async Task<TarifaDto> CriarAsync(CriarTarifaDto dto) {
        var (bairroAId, bairroBId) = NormalizarPar(dto.BairroAId, dto.BairroBId);

        Bairro bairroA = await ObterBairroOuFalharAsync(bairroAId);
        Bairro bairroB = bairroAId == bairroBId ? bairroA : await ObterBairroOuFalharAsync(bairroBId);

        if (await _repository.ObterPorBairrosAsync(bairroAId, bairroBId) is not null) {
            throw new InvalidOperationException("Já existe uma tarifa cadastrada entre esses bairros.");
        }

        Tarifa tarifa = new Tarifa {
            BairroA = bairroA,
            BairroB = bairroB,
            Valor = dto.Valor
        };

        await _repository.AdicionarAsync(tarifa);
        await _repository.SalvarMudancasAsync();

        return MapearParaDto(tarifa);
    }

    public async Task<bool> AtualizarAsync(int id, AtualizarTarifaDto dto) {
        Tarifa? tarifa = await _repository.ObterPorIdAsync(id);
        if (tarifa is null) {
            return false;
        }

        tarifa.Valor = dto.Valor;
        tarifa.AtualizadoEm = DateTime.UtcNow;

        _repository.Atualizar(tarifa);
        return await _repository.SalvarMudancasAsync();
    }

    public async Task<bool> DeletarAsync(int id) {
        Tarifa? tarifa = await _repository.ObterPorIdAsync(id);
        if (tarifa is null) {
            return false;
        }

        _repository.Deletar(tarifa);
        return await _repository.SalvarMudancasAsync();
    }

    // A tarifa vale nos dois sentidos (A->B = B->A). Para isso existir uma única linha por par,
    // guardamos sempre o menor id em BairroA e o maior em BairroB.
    private static (int menor, int maior) NormalizarPar(int x, int y) {
        return x <= y ? (x, y) : (y, x);
    }

    private async Task<Bairro> ObterBairroOuFalharAsync(int bairroId) {
        return await _bairroRepository.ObterPorIdAsync(bairroId)
            ?? throw new KeyNotFoundException($"O bairro {bairroId} não existe.");
    }

    // Como o par foi normalizado, o bairro de origem pode estar em A ou em B.
    private static string NomeDoBairro(Tarifa tarifa, int bairroId) {
        return tarifa.BairroAId == bairroId ? tarifa.BairroA.Nome : tarifa.BairroB.Nome;
    }

    private static TarifaDto MapearParaDto(Tarifa tarifa) {
        return new TarifaDto {
            Id = tarifa.Id,
            BairroAId = tarifa.BairroAId,
            BairroANome = tarifa.BairroA.Nome,
            BairroBId = tarifa.BairroBId,
            BairroBNome = tarifa.BairroB.Nome,
            Valor = tarifa.Valor,
            CriadoEm = tarifa.CriadoEm,
            AtualizadoEm = tarifa.AtualizadoEm
        };
    }
}
