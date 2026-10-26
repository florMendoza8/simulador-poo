namespace API.Modelos.Metodo;
using Dominio;

public class ObtenerMetodoRespuesta
{
    public Guid Id { get; set; }
    public string? Nombre { get; set; }
    public string? TipoRetorno { get; set; }
    public bool EsAbstract { get; set; }
    public bool? EsSealed { get; set; }
    public bool EsEstatico { get; set; }
    public bool EsVirtual { get; set; }
    public bool EsOverride { get; set; }
    public string? Accesibilidad { get; set; }
    public List<Parametro> Parametros { get; set; } = [];
    public List<VariableLocal> VariablesLocales { get; set; } = [];
    public List<Metodo> MetodosInvocados { get; set; } = [];

    public ObtenerMetodoRespuesta(Metodo metodo)
    {
        Id = metodo.Id;
        Nombre = metodo.Nombre;
        TipoRetorno = metodo.TipoRetorno?.Nombre;
        EsAbstract = metodo.EsAbstract;
        EsSealed = metodo.EsSealed ?? false;
        EsEstatico = metodo.EsEstatico;
        EsVirtual = metodo.EsVirtual;
        EsOverride = metodo.EsOverride;
        Accesibilidad = metodo.Accesibilidad?.ToString();
        Parametros = metodo.Parametros?
            .Select(p => new Parametro { Id = p.Id, Nombre = p.Nombre })
            .ToList() ?? [];

        VariablesLocales = metodo.VariablesLocales?
            .Select(v => new VariableLocal { Id = v.Id, Nombre = v.Nombre })
            .ToList() ?? [];

        MetodosInvocados = metodo.MetodosInvocados?
            .Select(m => new Metodo { Id = m.Id, Nombre = m.Nombre })
            .ToList() ?? [];
    }
}
