using Dominio;

namespace AccesoADatos.InterfacesRepositorios;

public interface INamespaceRepositorio
{
    void Agregar(Namespace nameNs);
    bool Existe(Namespace nameNs);
    Namespace? Obtener(Guid? id);
    List<Namespace> ObtenerTodos();
    void Eliminar(Namespace? nameNs);
    void Actualizar(Namespace? nameNs);
}
