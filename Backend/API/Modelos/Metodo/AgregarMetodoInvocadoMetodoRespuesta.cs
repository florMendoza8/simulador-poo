namespace API.Modelos.Metodo;

public class AgregarMetodoInvocadoMetodoRespuesta(Guid metodoId, Guid metodoInvocadoId)
{
    public Guid MetodoId { get; } = metodoId;
    public Guid MetodoInvocadoId { get; } = metodoInvocadoId;
}
