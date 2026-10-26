namespace API.Modelos.Metodo;

public class AgregarParametroMetodoRespuesta(Guid metodoId, Guid parametroId)
{
    public Guid MetodoId { get; } = metodoId;
    public Guid ParametroId { get; } = parametroId;
}
