using Dominio;

namespace AccesoADatos.InterfacesRepositorios;

public interface IClaseRepositorio
{
    void Agregar(Clase? clase);
    bool Existe(Clase? clase);
    Clase? ObtenerClase(Guid? id);
    List<Clase> ObtenerTodasLasClases();
    void EliminarClase(Clase? clase);
    void Actualizar(Clase? clase);
}
