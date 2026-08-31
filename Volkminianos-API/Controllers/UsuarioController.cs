using Microsoft.AspNetCore.Mvc;
using VolkminianosAPI.DTOs.Usuario;
using VolkminianosAPI.Services;

namespace VolkminianosAPI.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase {
        private readonly IUsuarioService service;

        public UsuarioController(IUsuarioService service) {
            this.service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> ObterTodosAsync() {
            var usuarios = await service.ObterTodosAsync();
            if (usuarios == null || !usuarios.Any()) {
                return NotFound("Nenhum usuário encontrado.");
            }
            return Ok(usuarios);
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<UsuarioDto>> ObterPorIdAsync(int id) {
            var usuario = await service.ObterPorIdAsync(id);
            if (usuario == null) {
                return NotFound("Usuário não encontrado.");
            }
            return Ok(usuario);
        }

        [HttpGet("por-email")]
        public async Task<ActionResult<UsuarioDto>> ObterPorEmailAsync([FromQuery] string email) {
            if (string.IsNullOrEmpty(email)) {
                return BadRequest("O e-mail deve ser fornecido.");
            }

            var usuario = await service.ObterPorEmailAsync(email);
            if (usuario == null) {
                return NotFound("Usuário não encontrado.");
            }
            return Ok(usuario);
        }

        [HttpPost]
        public async Task<ActionResult<UsuarioDto>> CriarAsync([FromBody] CriarUsuarioDto dto) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            try {
                var usuario = await service.CriarAsync(dto);
                return CreatedAtAction(nameof(ObterPorIdAsync), new { id = usuario.Id }, usuario);
            } catch (InvalidOperationException ex) {
                return Conflict(ex.Message);
            }
        }

        [HttpPut("{id:int:min(1)}")]
        public async Task<IActionResult> AtualizarAsync(int id, [FromBody] AtualizarUsuarioDto dto) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var atualizado = await service.AtualizarAsync(id, dto);
            if (!atualizado) {
                return NotFound("Usuário não encontrado.");
            }
            return NoContent();
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<IActionResult> DeletarAsync(int id) {
            var deletado = await service.DeletarAsync(id);
            if (!deletado) {
                return NotFound("Usuário não encontrado.");
            }
            return NoContent();
        }
    }
}