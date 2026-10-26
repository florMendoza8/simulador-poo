namespace API.Modelos.TipoDato;
using Dominio;

public class ObtenerTipoDatoRespuesta(TipoDato tipoDato)
{
    public Guid Id { get; set; } = tipoDato.Id;
    public string? Nombre { get; set; } = tipoDato.Nombre;
    public TipoDato? TipoPadre { get; set; } = tipoDato.TipoPadre;
}
