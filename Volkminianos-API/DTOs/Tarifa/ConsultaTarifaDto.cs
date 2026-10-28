namespace VolkminianosAPI.DTOs.Tarifa;

// Resposta da consulta "quanto custa ir do ponto X ao ponto Y?"
public class ConsultaTarifaDto {
    public int OrigemPontoId { get; set; }
    public string OrigemPontoNome { get; set; } = string.Empty;
    public string OrigemBairro { get; set; } = string.Empty;

    public int DestinoPontoId { get; set; }
    public string DestinoPontoNome { get; set; } = string.Empty;
    public string DestinoBairro { get; set; } = string.Empty;

    public decimal Valor { get; set; }
}
