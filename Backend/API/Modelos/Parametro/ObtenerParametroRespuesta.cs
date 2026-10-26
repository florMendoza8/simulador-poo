namespace API.Modelos.Parametro;
using Dominio;

public class ObtenerParametroRespuesta(Parametro parametro)
{
    public Guid Id { get; } = parametro.Id;
    public string? Nombre { get; } = parametro.Nombre;
    public TipoDato? TipoDato { get; } = parametro.TipoDato;
}
