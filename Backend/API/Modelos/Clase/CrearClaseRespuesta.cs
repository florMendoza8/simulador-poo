namespace API.Modelos.Clase;
using Dominio;

public class CrearClaseRespuesta(Clase? clase)
{
    public Guid Id { get; set; } = clase.Id;
}
