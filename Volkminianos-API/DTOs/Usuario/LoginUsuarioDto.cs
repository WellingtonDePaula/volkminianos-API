using System.ComponentModel.DataAnnotations;

namespace VolkminianosAPI.DTOs.Usuario;
public class LoginUsuarioDto {
    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [MaxLength(150, ErrorMessage = "O e-mail deve ter no máximo 150 caracteres.")]
    [EmailAddress(ErrorMessage = "O e-mail informado não é válido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [MinLength(8, ErrorMessage = "A senha deve ter no mínimo 8 caracteres.")]
    public string Senha { get; set; } = string.Empty;
}
