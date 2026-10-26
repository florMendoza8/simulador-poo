namespace Dominio;

public class Namespace : Elemento
{
    private string? _nombre;
    private Namespace? _padre;
    private readonly List<Elemento?> _elementos = [];

    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Nombre { get => _nombre; set => _nombre = value; }
    public Namespace? Padre { get => _padre;  set => _padre = value; }
    public List<Clase> Clases { get; set; } = new();
    public List<Interfaz> Interfazes { get; set; } = new();
    public List<Namespace> NamespacesHijos { get; set; } = new();

    public Namespace()
    {
    }

    public Namespace(string? nombre, Namespace? padre)
    {
        Nombre = nombre;
        Padre = padre;
    }

    public override void Agregar(Elemento? elemento)
    {
        _elementos.Add(elemento);
    }

    public override void Eliminar(Elemento? elemento)
    {
        _elementos.Remove(elemento);
    }

    public override Elemento? ObtenerElemento(Guid id)
    {
        if (Id == id)
        {
            return this;
        }

        foreach (var hijo in _elementos)
        {
            var resultado = hijo.ObtenerElemento(id);
            if (resultado != null)
            {
                return resultado;
            }
        }

        return null;
    }

    public List<Elemento?> ObtenerElementos()
    {
        return _elementos;
    }
}
