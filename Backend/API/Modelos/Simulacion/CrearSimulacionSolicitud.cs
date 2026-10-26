using Dominio;

namespace API.Modelos.Simulacion;

public class CrearSimulacionSolicitud
{
    public Guid TipoReferenciaId { get; set; }
    public Guid TipoObjetoId { get; set; }
    public Guid MetodoReferenciaId { get; set; }
    public string? Transformador { get; set; }
}
