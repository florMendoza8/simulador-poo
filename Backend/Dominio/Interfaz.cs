using Dominio.Excepciones;

namespace Dominio;

public class Interfaz : Elemento
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string? Nombre { get; set; }
    public List<Metodo>? FirmaMetodos { get; set; }
    public Namespace? Namespace { get; set; }

    public Interfaz()
    {
    }

    public Interfaz(string? nombre, List<Metodo> firmaMetodos)
    {
        Nombre = nombre;
        FirmaMetodos = firmaMetodos;
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
