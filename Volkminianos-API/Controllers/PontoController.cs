using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolkminianosAPI.Domain.Services;
using VolkminianosAPI.DTOs.Ponto;

namespace VolkminianosAPI.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class PontoController : ControllerBase {
        private readonly IPontoService _service;

        public PontoController(IPontoService service) {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PontoDto>>> ObterTodosAsync() {
            var pontos = await _service.ObterTodosAsync();
            return Ok(pontos);
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<PontoDto>> ObterPorIdAsync(int id) {
            var ponto = await _service.ObterPorIdAsync(id);
            if (ponto == null) {
                return NotFound("Ponto não encontrado.");
            }
            return Ok(ponto);
        }

        [HttpGet("bairro/{bairroId:int:min(1)}")]
        public async Task<ActionResult<IEnumerable<PontoDto>>> ObterPorBairroIdAsync(int bairroId) {
            var pontos = await _service.ObterPorBairroIdAsync(bairroId);
            return Ok(pontos);
        }

        [HttpGet("nome/{nome}")]
        public async Task<ActionResult<PontoDto>> ObterPorNomeAsync(string nome) {
            var ponto = await _service.ObterPorNomeAsync(nome);
            if (ponto == null) {
                return NotFound("Ponto não encontrado.");
            }
            return Ok(ponto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CriarAsync(CriarPontoDto dto) {
            try {
                var ponto = await _service.CriarAsync(dto);
                return CreatedAtAction(nameof(ObterPorIdAsync), new { id = ponto.Id }, ponto);
            } catch (KeyNotFoundException ex) {
                return BadRequest(new { mensagem = ex.Message });
            } catch (InvalidOperationException ex) {
                return Conflict(new { mensagem = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int:min(1)}")]
        public async Task<IActionResult> AtualizarAsync(int id, [FromBody] AtualizarPontoDto dto) {
            try {
                bool atualizado = await _service.AtualizarAsync(id, dto);
                if (!atualizado) {
                    return NotFound("Ponto não encontrado.");
                }
                return NoContent();
            } catch (KeyNotFoundException ex) {
                return BadRequest(new { mensagem = ex.Message });
            } catch (InvalidOperationException ex) {
                return Conflict(new { mensagem = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int:min(1)}")]
        public async Task<IActionResult> DeletarAsync(int id) {
            bool deletado = await _service.DeletarAsync(id);
            if (!deletado) {
                return NotFound("Ponto não encontrado.");
            }
            return NoContent();
        }
    }
}
