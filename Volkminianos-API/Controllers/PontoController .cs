using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
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
            if (pontos == null || !pontos.Any()) {
                return NotFound("Nenhum ponto encontrado.");
            }
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
            if (pontos == null || !pontos.Any()) {
                return NotFound("Nenhum ponto encontrado para o bairro informado.");
            }
            return Ok(pontos);
        }

        [HttpGet("nome/{nome}")]
        public async Task<ActionResult<PontoDto>> ObterPorNomeAsync(string nome) {
            try {
                var ponto = await _service.ObterPorNomeAsync(nome);
                return Ok(ponto);
            } catch (InvalidOperationException ex) {
                return NotFound(new { mensagem = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CriarAsync(CriarPontoDto dto) {
            try {
                var ponto = await _service.CriarAsync(dto);
                return CreatedAtAction(nameof(ObterPorNomeAsync), new { nome = ponto.Nome }, ponto);
            } catch (InvalidOperationException ex) {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int:min(1)}")]
        public async Task<IActionResult> AtualizarAsync(int id, [FromBody] AtualizarPontoDto dto) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            bool atualizado = await _service.AtualizarAsync(id, dto);
            if (!atualizado) {
                return NotFound("Ponto não encontrado.");
            }
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("{id:int:min(1)}")]
        public async Task<IActionResult> DeletarAsync(int id) {
            bool deletado = await _service.DeletarAsync(id);
            if (!deletado) {
                return NotFound("Ponto não encontrado.");
            }
            return NoContent();
        }
    }
}