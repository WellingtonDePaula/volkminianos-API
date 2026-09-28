using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolkminianosAPI.Domain.Services;
using VolkminianosAPI.DTOs.Bairro;

namespace VolkminianosAPI.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class BairroController : ControllerBase {
        private readonly IBairroService _service;

        public BairroController(IBairroService service) {
            this._service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BairroDto>>> ObterTodosAsync() {
            var bairros = await _service.ObterTodosAsync();
            if (bairros == null || !bairros.Any()) {
                return NotFound("Nenhum bairro encontrado.");
            }
            return Ok(bairros);
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<BairroDto>> ObterPorIdAsync(int id) {
            var bairro = await _service.ObterPorIdAsync(id);
            if (bairro == null) {
                return NotFound("Bairro não encontrado.");
            }
            return Ok(bairro);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CriarAsync(CriarBairroDto dto) {
            try {
                var bairro = await _service.CriarAsync(dto);
                return CreatedAtAction(nameof(ObterPorIdAsync), new { id = bairro.Id }, bairro);
            } catch (InvalidOperationException ex) {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int:min(1)}")]
        public async Task<IActionResult> AtualizarAsync(int id, [FromBody] AtualizarBairroDto dto) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            bool atualizado = await _service.AtualizarAsync(id, dto);
            if (!atualizado) {
                return NotFound("Bairro não encontrado.");
            }
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("{id:int:min(1)}")]
        public async Task<IActionResult> DeletarAsync(int id) {
            bool deletado = await _service.DeletarAsync(id);
            if (!deletado) {
                return NotFound("Bairro não encontrado.");
            }
            return NoContent();
        }

    }
}