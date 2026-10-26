namespace API.Modelos.VariableLocal;
using Dominio;
public class ObtenerVariableLocalRespuesta(VariableLocal variableLocal)
{
    public Guid Id { get; set; } = variableLocal.Id;
    public string? Nombre { get; set; } = variableLocal.Nombre;
    public TipoDato? TipoReferencia { get; set; } = variableLocal.TipoReferencia;
    public string? Valor { get; set; } = variableLocal.Valor;
}
