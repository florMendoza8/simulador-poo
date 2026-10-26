using Dominio;

namespace AccesoADatos.InterfacesRepositorios;

public interface IAtributoRepositorio
{
    void Agregar(Atributo atributo);
    void Actualizar(Atributo atributo);
    void Eliminar(Atributo atributo);
    bool Existe(string? nombre, TipoDato tipoDato, Accesibilidad accesibilidad);
    Atributo? Obtener(Guid id);
    List<Atributo> ObtenerTodos();
}
