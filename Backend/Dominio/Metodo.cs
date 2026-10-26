namespace Dominio;

public class Metodo
{
    public Guid Id { get; set; } = Guid.NewGuid();
    private string? _nombre;
    private TipoDato? _tipoRetorno;
    private bool _esAbstract;
    private bool? _esSealed;
    private bool _esEstatico;
    private bool _esVirtual;
    private bool _esOverride;
    private Accesibilidad? _accesibilidad;
    private Clase? _clase;
    private List<Parametro>? _parametros;
    private List<VariableLocal>? _variablesLocales;
    private List<Metodo>? _metodosInvocados;

    public Metodo()
    {
        Parametros = [];
        VariablesLocales = [];
        MetodosInvocados = [];
    }

    public Metodo(string nombre, TipoDato tipoRetorno, bool esAbstract, bool esSealed, Accesibilidad accesiblidad,
        Clase? clase, List<Parametro> parametros, List<VariableLocal> variablesLocales, List<Metodo> metodosInvocados)
    {
        Nombre = nombre;
        TipoRetorno = tipoRetorno;
        EsAbstract = esAbstract;
        EsSealed = esSealed;
        Accesibilidad = accesiblidad;
        Clase = clase;
        Parametros = parametros;
        VariablesLocales = variablesLocales;
        MetodosInvocados = metodosInvocados;
    }

    public Metodo(string nombre, TipoDato tipoRetorno, List<Parametro> parametros)
    {
        Nombre = nombre;
        TipoRetorno = tipoRetorno;
        Parametros = parametros;
    }

    public string? Nombre { get => _nombre; set => _nombre = value; }

    public TipoDato? TipoRetorno { get => _tipoRetorno; set => _tipoRetorno = value; }

    public bool EsAbstract { get => _esAbstract; set => _esAbstract = value; }

    public bool? EsSealed { get => _esSealed; set => _esSealed = value; }

    public bool EsEstatico { get => _esEstatico; set => _esEstatico = value; }

    public bool EsVirtual { get => _esVirtual; set => _esVirtual = value; }

    public bool EsOverride { get => _esOverride; set => _esOverride = value; }

    public Accesibilidad? Accesibilidad { get => _accesibilidad; set => _accesibilidad = value; }

    public Clase? Clase { get => _clase; set => _clase = value; }

    public List<Parametro>? Parametros { get => _parametros; set => _parametros = value; }

    public List<VariableLocal>? VariablesLocales { get => _variablesLocales; set => _variablesLocales = value; }

    public List<Metodo>? MetodosInvocados { get => _metodosInvocados; set => _metodosInvocados = value; }
}
