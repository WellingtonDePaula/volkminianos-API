using VolkminianosAPI.Domain.Interfaces;
using VolkminianosAPI.DTOs.Usuario;
using VolkminianosAPI.Services.Interfaces;
using Usuario = VolkminianosAPI.Models.Usuario;

namespace VolkminianosAPI.Services;

public class UsuarioService : IUsuarioService {
    private readonly IUsuarioRepository _repository;

    public UsuarioService(IUsuarioRepository repository) {
        _repository = repository;
    }

    public async Task<IEnumerable<UsuarioDto>> ObterTodosAsync() {
        var usuarios = await _repository.ObterTodosAsync();
        return usuarios.Select(MapearParaDto);
    }

    public async Task<UsuarioDto?> ObterPorIdAsync(int id) {
        var usuario = await _repository.ObterPorIdAsync(id);
        return usuario is null ? null : MapearParaDto(usuario);
    }

    public async Task<UsuarioDto?> ObterPorEmailAsync(string email) {
        var usuario = await _repository.ObterPorEmailAsync(email);
        return usuario is null ? null : MapearParaDto(usuario);
    }

    public async Task<UsuarioDto> CriarAsync(CriarUsuarioDto dto) {
        var usuarioExistente = await _repository.ObterPorEmailAsync(dto.Email);
        if (usuarioExistente is not null) {
            throw new InvalidOperationException("Já existe um usuário cadastrado com este e-mail.");
        }

        var usuario = new Usuario {
            Nome = dto.Nome,
            Email = dto.Email
        };

        await _repository.AdicionarAsync(usuario);
        await _repository.SalvarMudancasAsync();

        return MapearParaDto(usuario);
    }

    public async Task<bool> AtualizarAsync(int id, AtualizarUsuarioDto dto) {
        var usuario = await _repository.ObterPorIdAsync(id);
        if (usuario is null) {
            return false;
        }

        usuario.Nome = dto.Nome;
        usuario.Email = dto.Email;
        usuario.Ativo = dto.Ativo;
        usuario.AtualizadoEm = DateTime.UtcNow;

        _repository.Atualizar(usuario);
        return await _repository.SalvarMudancasAsync();
    }

    public async Task<bool> DeletarAsync(int id) {
        var usuario = await _repository.ObterPorIdAsync(id);
        if (usuario is null) {
            return false;
        }

        _repository.Deletar(usuario);
        return await _repository.SalvarMudancasAsync();
    }

    private static UsuarioDto MapearParaDto(Usuario usuario) {
        return new UsuarioDto {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
            IsAdmin = usuario.IsAdmin,
            Ativo = usuario.Ativo,
            CriadoEm = usuario.CriadoEm,
            AtualizadoEm = usuario.AtualizadoEm
        };
    }
}