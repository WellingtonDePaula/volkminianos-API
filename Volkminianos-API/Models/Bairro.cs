using System.ComponentModel.DataAnnotations;

namespace VolkminianosAPI.Models;

public class Bairro {
    [Key]
    public int Id { get; set; }
    [Required, MaxLength(200)]
    public string Nome { get; set; } = string.Empty;
    public bool Ativo { get; set; } = true;

    public ICollection<Ponto> Pontos { get; set; } = [];
}