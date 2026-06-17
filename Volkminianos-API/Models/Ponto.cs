using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VolkminianosAPI.Models;

public class Ponto {
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(150)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    public int BairroId { get; set; }
    [ForeignKey(nameof(BairroId))]
    public Bairro? Bairro { get; set; }

    [MaxLength(250)]
    public string? Endereco { get; set; }

    [Column(TypeName = "decimal(10, 7)")]
    public decimal Latitude { get; set; }

    [Column(TypeName = "decimal(10, 7)")]
    public decimal Longitude { get; set; }

    public bool PontoTuristico { get; set; } = false;

    [MaxLength(500)]
    public string? Descricao { get; set; }

    public bool Ativo { get; set; } = true;

    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    public DateTime? AtualizadoEm { get; set; }
}