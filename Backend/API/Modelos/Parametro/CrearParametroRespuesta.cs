namespace API.Modelos.Parametro;
using Dominio;

public class CrearParametroRespuesta(Parametro parametro)
{
    public Guid Id { get; } = parametro.Id;
}
