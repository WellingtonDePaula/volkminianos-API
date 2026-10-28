namespace VolkminianosAPI.DTOs.Ponto;

public class PontoDto {
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Endereco { get; set; }
    public int BairroId { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public bool PontoTuristico { get; set; }
    public string? Descricao { get; set; }
    public bool Ativo { get; set; }
    public DateTime CriadoEm { get; set; }
    public DateTime? AtualizadoEm { get; set; }
}
