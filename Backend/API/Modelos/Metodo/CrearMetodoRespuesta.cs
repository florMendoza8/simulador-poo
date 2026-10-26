namespace API.Modelos.Metodo;
using Dominio;

public class CrearMetodoRespuesta(Metodo metodo)
{
    public Guid Id { get; } = metodo.Id;
}
