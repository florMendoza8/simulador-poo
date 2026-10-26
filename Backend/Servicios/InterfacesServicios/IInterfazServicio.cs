using Dominio;

namespace Servicios.InterfacesServicios;

public interface IInterfazServicio
{
    Interfaz CrearInterfaz(Interfaz interfaz);
    void EliminarInterfaz(Interfaz interfaz);
    Interfaz ObtenerInterfazPorId(Guid id);
    List<Interfaz> ObtenerTodasLasInterfaces();
    void AgregarMetodoAFirmaMetodos(Guid interfazId, Guid metodoId);
}
