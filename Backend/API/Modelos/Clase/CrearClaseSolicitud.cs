namespace API.Modelos.Clase;

public class CrearClaseSolicitud
{
    public string? Nombre { get; set; }
    public bool EsAbstract { get; set; }
    public bool EsSealed { get; set; }
    public Guid? ClaseBaseId { get; set; }
}
