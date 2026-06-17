using Microsoft.AspNetCore.Mvc;
using VolkminianosAPI.Domain.Interfaces;
using VolkminianosAPI.Models;

namespace VolkminianosAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase {
        private readonly IUsuarioRepository repository;

        public UsuarioController(IUsuarioRepository repository) {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> ObterTodosAsync() {
            var usuarios = await repository.ObterTodosAsync();
            if (usuarios == null || !usuarios.Any()) {
                return NotFound("Nenhum usuário encontrado.");
            }
            return Ok(usuarios);
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<Usuario>> ObterPorIdAsync(int id) {
            var usuario = await repository.ObterPorIdAsync(id);
            if (usuario == null) {
                return NotFound("Usuário não encontrado.");
            }
            return Ok(usuario);
        }

        [HttpGet("por-email")]
        public async Task<ActionResult<Usuario>> ObterPorEmailAsync([FromQuery] string email) {
            if (string.IsNullOrEmpty(email)) {
                return BadRequest("O e-mail deve ser fornecido.");
            }

            var usuario = await repository.ObterPorEmailAsync(email);
            if (usuario == null) {
                return NotFound("Usuário não encontrado.");
            }
            return Ok(usuario);
        }
    }
}
