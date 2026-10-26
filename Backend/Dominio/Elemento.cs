namespace Dominio;

public abstract class Elemento
{
    public abstract void Agregar(Elemento? elemento);
    public abstract void Eliminar(Elemento? elemento);
    public abstract Elemento? ObtenerElemento(Guid id);
}
