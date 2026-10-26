using Dominio;

namespace Servicios.InterfacesServicios;

public interface INamespaceServicio
{
    Namespace Crear(Namespace nuevoNS);
    void Renombrar(Guid id, string nuevoNombre);
    void CambiarPadre(Guid id, Guid? nuevoPadreId);
    void Eliminar(Guid id);
    List<Namespace> ListarTodos();
    Namespace Obtener(Guid id);
    void AgregarElemento(Guid namespaceId, Elemento? elemento);
    void EliminarElemento(Guid namespaceId, Guid elementoId);
    IEnumerable<Elemento?> ObtenerHijos(Guid namespaceId);
    void AgregarClase(Guid namespaceId, Clase? clase);
    void AgregarInterfaz(Guid namespaceId, Interfaz? interfaz);
}
