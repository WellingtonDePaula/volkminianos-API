using System.ComponentModel.DataAnnotations;

namespace VolkminianosAPI.DTOs.Ponto;

public class CriarPontoDto {
    [Required(ErrorMessage = "O nome do ponto é obrigatório.")]
    [MaxLength(150, ErrorMessage = "O nome deve ter no máximo 150 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [MaxLength(250, ErrorMessage = "O endereço deve ter no máximo 250 caracteres.")]
    public string? Endereco { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Informe um bairro válido.")]
    public int BairroId { get; set; }

    [Range(-90.0, 90.0, ErrorMessage = "A latitude deve estar entre -90 e 90.")]
    public decimal Latitude { get; set; }

    [Range(-180.0, 180.0, ErrorMessage = "A longitude deve estar entre -180 e 180.")]
    public decimal Longitude { get; set; }

    public bool PontoTuristico { get; set; }

    [MaxLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres.")]
    public string? Descricao { get; set; }

    public bool Ativo { get; set; } = true;
}
