namespace API.Modelos.Clase;

public class AgregarAtributoRespuesta(Guid claseid, Guid atributoid)
{
    public Guid ClaseId { get; } = claseid;
    public Guid AtributoId { get; } = atributoid;
}
