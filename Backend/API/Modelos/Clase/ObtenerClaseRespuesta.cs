namespace API.Modelos.Clase;
using Dominio;

public class ObtenerClaseRespuesta
{
    public Guid Id { get; set; }
    public string? Nombre { get; set; }
    public bool EsAbstract { get; set; }
    public bool EsSealed { get; set; }
    public Guid? ClaseBaseId { get; set; }

    public List<Metodo> Metodos { get; set; } = new();
    public List<Atributo> Atributos { get; set; } = new();

    public ObtenerClaseRespuesta(Clase? clase)
    {
        Id = clase.Id;
        Nombre = clase.Nombre;
        EsAbstract = clase.EsAbstract;
        EsSealed = clase.EsSealed;
        ClaseBaseId = clase.ClaseBase?.Id;
        Metodos = clase.MetodosInvocados
            .Select(m => new Metodo
            {
                Id = m.Id,
                Nombre = m.Nombre
            })
            .ToList();

        Atributos = clase.Atributos
            .Select(a => new Atributo
            {
                Id = a.Id,
                Nombre = a.Nombre
            })
            .ToList();
    }
}
