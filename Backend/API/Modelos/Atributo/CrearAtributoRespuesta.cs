namespace API.Modelos.Atributo;
using Dominio;

public class CrearAtributoRespuesta(Atributo atributo)
{
    public Guid Id { get; set; } = atributo.Id;
}
