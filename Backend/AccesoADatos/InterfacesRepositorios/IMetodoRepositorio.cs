using Dominio;

namespace AccesoADatos.InterfacesRepositorios;

public interface IMetodoRepositorio
{
    void Agregar(Metodo metodo);

    bool Existe(string nombre);

    Metodo Obtener(Guid id);

    List<Metodo> ObtenerTodos();

    List<Metodo> ObtenerMetodosInvocados(Guid metodoId);

    void Actualizar(Metodo metodo);

    void Eliminar(Metodo metodo);
}
