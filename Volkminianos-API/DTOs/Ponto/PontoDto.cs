namespace VolkminianosAPI.DTOs.Ponto;

public class PontoDto {
    public string Nome { get; set; } = string.Empty;
    public string Endereco { get; set; } = string.Empty;
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public bool PontoTuristico { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public bool Ativo { get; set; }
    public DateTime CriadoEm { get; set; }
    public DateTime? AtualizadoEm { get; set; }
}
