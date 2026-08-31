using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VolkminianosAPI.DTOs.Usuario;
using VolkminianosAPI.Services;

namespace VolkminianosAPI.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase {
        private readonly IUsuarioService _service;

        public UsuarioController(IUsuarioService service) {
            this._service = service;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUsuarioDto dto) {
            try {
                var token = await _service.LoginAsync(dto);

                return Ok(new { token });
            } catch (UnauthorizedAccessException) {
                return Unauthorized(new {
                    mensagem = "Email ou senha inválidos."
                });
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> ObterTodosAsync() {
            var usuarios = await _service.ObterTodosAsync();
            if (usuarios == null || !usuarios.Any()) {
                return NotFound("Nenhum usuário encontrado.");
            }
            return Ok(usuarios);
        }

        [Authorize]
        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<UsuarioDto>> ObterPorIdAsync(int id) {
            var usuario = await _service.ObterPorIdAsync(id);
            if (usuario == null) {
                return NotFound("Usuário não encontrado.");
            }
            return Ok(usuario);
        }

        [Authorize]
        [HttpGet("por-email")]
        public async Task<ActionResult<UsuarioDto>> ObterPorEmailAsync([FromQuery] string email) {
            if (string.IsNullOrEmpty(email)) {
                return BadRequest("O e-mail deve ser fornecido.");
            }

            var usuario = await _service.ObterPorEmailAsync(email);
            if (usuario == null) {
                return NotFound("Usuário não encontrado.");
            }
            return Ok(usuario);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<UsuarioDto>> CriarAsync([FromBody] CriarUsuarioDto dto) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            try {
                var usuario = await _service.CriarAsync(dto);
                return CreatedAtAction(nameof(ObterPorIdAsync), new { id = usuario.Id }, usuario);
            } catch (InvalidOperationException ex) {
                return Conflict(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int:min(1)}")]
        public async Task<IActionResult> AtualizarAsync(int id, [FromBody] AtualizarUsuarioDto dto) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var atualizado = await _service.AtualizarAsync(id, dto);
            if (!atualizado) {
                return NotFound("Usuário não encontrado.");
            }
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int:min(1)}")]
        public async Task<IActionResult> DeletarAsync(int id) {
            var deletado = await _service.DeletarAsync(id);
            if (!deletado) {
                return NotFound("Usuário não encontrado.");
            }
            return NoContent();
        }
    }
}