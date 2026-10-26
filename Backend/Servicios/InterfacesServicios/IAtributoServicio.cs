using Dominio;

namespace Servicios.InterfacesServicios;

public interface IAtributoServicio
{
    Atributo CrearAtributo(Atributo atributo);
    void ModificarAtributo(Atributo atributo);
    void EliminarAtributo(Atributo atributo);
    bool ExisteAtributo(string? nombre, TipoDato tipoDato, Accesibilidad accesibilidad);
    Atributo ObtenerAtributoPorId(Guid id);
    List<Atributo> ObtenerTodosLosAtributos();
}
