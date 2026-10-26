using AccesoADatos.Repositorios;
using Dominio;
using Microsoft.EntityFrameworkCore;

namespace AccesoADatos.InterfacesRepositorios;

public interface IInterfazRepositorio
{
    void Agregar(Interfaz interfaz);
    void Eliminar(Interfaz interfaz);
    void Actualizar(Interfaz interfaz);
    Interfaz Obtener(Guid id);
    List<Interfaz> ObtenerTodos();
}
