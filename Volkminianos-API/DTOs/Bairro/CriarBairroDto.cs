using System.ComponentModel.DataAnnotations;

namespace VolkminianosAPI.DTOs.Bairro;

public class CriarBairroDto {
    [Required(ErrorMessage = "O bairro deve ter um nome")]
    [MaxLength(200, ErrorMessage = "Tamanho máximo de 200 caracteres")]
    public string Nome { get; set; } = string.Empty;
    public bool Ativo { get; set; } = true;
}