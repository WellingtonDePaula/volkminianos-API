using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VolkminianosAPI.Models;

public class Tarifa {
    [Key]
    public int Id { get; set; }

    [ForeignKey(nameof(BairroA))]
    public int BairroAId { get; set; }
    public Bairro BairroA { get; set; } = null!;


    [ForeignKey(nameof(BairroB))]
    public int BairroBId { get; set; }
    public Bairro BairroB { get; set; } = null!;


    [Required, Column(TypeName = "decimal(6, 2)")]
    public decimal Valor { get; set; }

    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    public DateTime? AtualizadoEm { get; set; }
}