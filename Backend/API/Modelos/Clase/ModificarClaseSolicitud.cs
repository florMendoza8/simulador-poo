namespace API.Modelos.Clase;

public class ModificarClaseSolicitud
{
    public string Nombre { get; set; } = string.Empty;
    public bool EsAbstract { get; set; }
    public bool EsSealed { get; set; }
    public Guid? ClaseBaseId { get; set; }
}
