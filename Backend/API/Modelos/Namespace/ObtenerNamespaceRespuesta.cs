namespace API.Modelos.Namespace;

using Dominio;

public class ObtenerNamespaceRespuesta(Namespace name_space)
{
    public string? Nombre { get; set; } = name_space.Nombre;
    public Namespace? Padre { get; set; } = name_space.Padre;
}
