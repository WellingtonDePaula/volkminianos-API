using System.ComponentModel.DataAnnotations;

namespace VolkminianosAPI.DTOs.Tarifa;

public class CriarTarifaDto {
    [Range(1, int.MaxValue, ErrorMessage = "Informe um bairro de origem válido.")]
    public int BairroAId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Informe um bairro de destino válido.")]
    public int BairroBId { get; set; }

    // decimal(6, 2) no banco => máximo 9999.99
    [Range(0.01, 9999.99, ErrorMessage = "O valor deve estar entre 0,01 e 9999,99.")]
    public decimal Valor { get; set; }
}
