using Dominio.Excepciones;

namespace Dominio;

public class Clase : Elemento
{
    private string? _nombre;
    private bool _esAbstract;
    private bool _esSealed;
    private List<Atributo>? _atributos;
    private List<Metodo>? _metodosInvocados;
    private Clase? _claseBase;
    private List<Interfaz>? _interfacesImplementadas;
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Nombre { get => _nombre; set => _nombre = value; }
    public bool EsAbstract { get => _esAbstract; set => _esAbstract = value; }
    public bool EsSealed { get => _esSealed; set => _esSealed = value; }
    public List<Atributo> Atributos { get => _atributos; set => _atributos = value; }
    public List<Metodo> MetodosInvocados { get => _metodosInvocados; set => _metodosInvocados = value; }
    public Clase? ClaseBase { get => _claseBase; set => _claseBase = value; }
    public List<Interfaz>? InterfacesImplementadas { get => _interfacesImplementadas; set => _interfacesImplementadas = value; }
    public Guid? ClaseBaseId { get; set; } = Guid.NewGuid();
    public Namespace? Namespace { get; set; }

    public Clase()
    {
        _atributos = new List<Atributo>();
        _metodosInvocados = new List<Metodo>();
    }

    public Clase(string nombre, bool esabstract, bool essealed, List<Atributo> atributos, List<Metodo> metodos, Clase? clasebase, Guid? claseBaseId, List<Interfaz> interfacesImplementadas)
    {
        Nombre = nombre;
        EsAbstract = esabstract;
        EsSealed = essealed;
        Atributos = atributos;
        MetodosInvocados = metodos;
        ClaseBase = clasebase;
        ClaseBaseId = claseBaseId;
        InterfacesImplementadas = interfacesImplementadas;
    }

    public override void Agregar(Elemento? elemento)
    {
        throw new NoImplementadoExcepcion("No implementado");
    }

    public override void Eliminar(Elemento? elemento)
    {
        throw new NoImplementadoExcepcion("No implementado");
    }

    public override Elemento? ObtenerElemento(Guid id)
    {
        return Id == id ? this : null;
    }
}
