namespace API.Modelos.Interfaz;
using Dominio;

public class ObtenerInterfazRespuesta
{
    public Guid Id { get; set; }
    public string? Nombre { get; set; }
    public List<Metodo> FirmaMetodos { get; set; } = [];

    public ObtenerInterfazRespuesta(Interfaz interfaz)
    {
        Id = interfaz.Id;
        Nombre = interfaz.Nombre;
        FirmaMetodos = interfaz.FirmaMetodos?
            .Select(m => new Metodo { Id = m.Id, Nombre = m.Nombre })
            .ToList() ?? [];
    }
}
