using Dominio;

namespace Servicios.InterfacesServicios;

public interface ITipoDatoServicio
{
    TipoDato CrearTipoDato(TipoDato tipoDato);
    bool ExisteTipoDato(string? nombre);
    TipoDato ObtenerTipoDatoPoId(Guid id);
    List<TipoDato> ObtenerTodosLosTipoDatos();
    bool EsCompatible(Guid referenciaId, Guid instanciaId);
    void EliminarTipoDato(TipoDato tipoDato);
}
