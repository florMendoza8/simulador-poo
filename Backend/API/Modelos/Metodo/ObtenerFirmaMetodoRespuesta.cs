namespace API.Modelos.Metodo;
using Dominio;

public class ObtenerFirmaMetodoRespuesta
{
    public Guid Id { get; set; }
    public string? Nombre { get; set; }
    public string? TipoRetorno { get; set; }
    public List<Parametro> Parametros { get; set; } = [];

    public ObtenerFirmaMetodoRespuesta(Metodo metodo)
    {
        Id = metodo.Id;
        Nombre = metodo.Nombre;
        TipoRetorno = metodo.TipoRetorno?.Nombre;
        Parametros = metodo.Parametros?
            .Select(p => new Parametro { Id = p.Id, Nombre = p.Nombre })
            .ToList() ?? [];
    }
}
