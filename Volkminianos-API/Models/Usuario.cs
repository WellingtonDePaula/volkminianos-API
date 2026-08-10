using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VolkminianosAPI.Models;

public class Usuario {
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Senha { get; set; } = string.Empty;

    public bool IsAdmin { get; set; } = false;

    public bool Ativo { get; set; } = true;

    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    public DateTime? AtualizadoEm { get; set; }
}