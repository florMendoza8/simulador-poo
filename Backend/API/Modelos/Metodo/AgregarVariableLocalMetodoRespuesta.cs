namespace API.Modelos.Metodo;

public class AgregarVariableLocalMetodoRespuesta(Guid metodoId, Guid parametroId)
{
    public Guid MetodoId { get; } = metodoId;
    public Guid VariableLocalId { get; } = parametroId;
}
