using Dominio;

namespace Servicios.InterfacesServicios;

public interface IClaseServicio
{
    Clase? CrearClase(Clase? clase);
    void ModificarClase(Clase clase);
    void EliminarClase(Clase? clase);
    Clase? ObtenerClasePorId(Guid? clase);
    List<Clase?> ObtenerTodasLasClases();
    void AgregarAtributo(Guid idClase, Guid idAtributo);
    void AgregarMetodo(Guid idClase, Guid idMetodo);
    void QuitarAtributo(Guid claseId, Guid atributoId);
    void QuitarMetodo(Guid claseId, Guid metodoId);
}
