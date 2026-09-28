namespace VolkminianosAPI.DTOs.Ponto;

public class CriarPontoDto {
    public string Nome { get; set; } = string.Empty;
    public string Endereco { get; set; } = string.Empty;
    public int BairroId { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public bool PontoTuristico { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public bool Ativo { get; set; }
    public DateTime CriadoEm { get; set; }
    public DateTime? AtualizadoEm { get; set; }
}
