using Dominio;

namespace AccesoADatos.InterfacesRepositorios;

public interface ITipoDatoRepositorio
{
    void Agregar(TipoDato tipoDato);
    bool Existe(string? nombre);
    TipoDato Obtener(Guid id);
    List<TipoDato> ObtenerTodos();
    void EliminarTipoDato(TipoDato tipoDato);
}
