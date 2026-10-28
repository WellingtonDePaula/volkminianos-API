using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolkminianosAPI.Domain.Services;
using VolkminianosAPI.DTOs.Tarifa;

namespace VolkminianosAPI.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class TarifaController : ControllerBase {
        private readonly ITarifaService _service;

        public TarifaController(ITarifaService service) {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TarifaDto>>> ObterTodosAsync() {
            var tarifas = await _service.ObterTodosAsync();
            return Ok(tarifas);
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<TarifaDto>> ObterPorIdAsync(int id) {
            var tarifa = await _service.ObterPorIdAsync(id);
            if (tarifa == null) {
                return NotFound("Tarifa não encontrada.");
            }
            return Ok(tarifa);
        }

        // GET api/tarifa/consulta?origemPontoId=1&destinoPontoId=7
        // É o endpoint principal para o passageiro: não exige login.
        [HttpGet("consulta")]
        public async Task<ActionResult<ConsultaTarifaDto>> ConsultarAsync(
            [FromQuery] int origemPontoId,
            [FromQuery] int destinoPontoId) {
            if (origemPontoId < 1 || destinoPontoId < 1) {
                return BadRequest(new { mensagem = "Informe origemPontoId e destinoPontoId válidos." });
            }

            try {
                var consulta = await _service.ConsultarAsync(origemPontoId, destinoPontoId);
                return Ok(consulta);
            } catch (KeyNotFoundException ex) {
                return NotFound(new { mensagem = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CriarAsync(CriarTarifaDto dto) {
            try {
                var tarifa = await _service.CriarAsync(dto);
                return CreatedAtAction(nameof(ObterPorIdAsync), new { id = tarifa.Id }, tarifa);
            } catch (KeyNotFoundException ex) {
                return BadRequest(new { mensagem = ex.Message });
            } catch (InvalidOperationException ex) {
                return Conflict(new { mensagem = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int:min(1)}")]
        public async Task<IActionResult> AtualizarAsync(int id, [FromBody] AtualizarTarifaDto dto) {
            bool atualizado = await _service.AtualizarAsync(id, dto);
            if (!atualizado) {
                return NotFound("Tarifa não encontrada.");
            }
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int:min(1)}")]
        public async Task<IActionResult> DeletarAsync(int id) {
            bool deletado = await _service.DeletarAsync(id);
            if (!deletado) {
                return NotFound("Tarifa não encontrada.");
            }
            return NoContent();
        }
    }
}
