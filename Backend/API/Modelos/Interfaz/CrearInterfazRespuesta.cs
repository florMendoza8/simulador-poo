namespace API.Modelos.Interfaz;
using Dominio;

public class CrearInterfazRespuesta(Interfaz interfaz)
{
    public Guid Id { get; } = interfaz.Id;
}
