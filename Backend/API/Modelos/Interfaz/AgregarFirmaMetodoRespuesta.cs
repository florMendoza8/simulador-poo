namespace API.Modelos.Interfaz;

public class AgregarFirmaMetodoRespuesta(Guid interfazId, Guid firmaMetodoId)
{
    public Guid InterfazId { get; } = interfazId;
    public Guid FirmaMetodoId { get; } = firmaMetodoId;
}
