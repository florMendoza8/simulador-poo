namespace API.Modelos.VariableLocal;
using Dominio;

public class CrearVariableSolicitud
{
    public string? Nombre { get; set; }
    public Guid TipoReferenciaId { get; set; }
    public string? Valor { get; set; }
}
