namespace VolkminianosAPI.DTOs.Tarifa;

public class TarifaDto {
    public int Id { get; set; }
    public int BairroAId { get; set; }
    public string BairroANome { get; set; } = string.Empty;
    public int BairroBId { get; set; }
    public string BairroBNome { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public DateTime CriadoEm { get; set; }
    public DateTime? AtualizadoEm { get; set; }
}
