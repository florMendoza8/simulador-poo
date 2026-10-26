namespace API.Modelos.Clase;

public class AgregarMetodoRespuesta(Guid claseid, Guid metodoid)
{
    public Guid ClaseId { get; } = claseid;
    public Guid MetodoId { get; } = metodoid;
}
