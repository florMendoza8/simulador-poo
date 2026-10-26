namespace API.Modelos.Namespace;
using Dominio;

public class CrearNamespaceRespuesta(Namespace name_space)
{
    public Guid Id { get; } = name_space.Id;
}
