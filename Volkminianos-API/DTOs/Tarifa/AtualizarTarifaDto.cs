using System.ComponentModel.DataAnnotations;

namespace VolkminianosAPI.DTOs.Tarifa;

// Só o valor pode mudar. Para trocar o par de bairros, apague a tarifa e crie outra.
public class AtualizarTarifaDto {
    [Range(0.01, 9999.99, ErrorMessage = "O valor deve estar entre 0,01 e 9999,99.")]
    public decimal Valor { get; set; }
}
