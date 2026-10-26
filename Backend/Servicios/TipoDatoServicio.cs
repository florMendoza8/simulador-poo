using AccesoADatos.InterfacesRepositorios;
using Dominio;
using Dominio.Excepciones;
using Servicios.InterfacesServicios;

namespace Servicios;

public class TipoDatoServicio : ITipoDatoServicio
{
    private readonly ITipoDatoRepositorio _tipoDatoRepositorio;

    public TipoDatoServicio(ITipoDatoRepositorio tipoDatoRepositorio)
    {
        _tipoDatoRepositorio = tipoDatoRepositorio;
    }

    public TipoDato CrearTipoDato(TipoDato tipoDato)
    {
        if (tipoDato == null)
        {
            throw new EntidadNulaExcepcion("El tipo de dato no puede ser nulo.");
        }

        if (string.IsNullOrWhiteSpace(tipoDato.Nombre))
        {
            throw new EntidadNulaExcepcion("El nombre no puede ser nulo o vacío.");
        }

        var existeTipoDato = _tipoDatoRepositorio.Existe(tipoDato.Nombre);
        if (!existeTipoDato)
        {
            _tipoDatoRepositorio.Agregar(tipoDato);
        }

        else
        {
            throw new EntidadYaExisteExcepcion($"El tipo de dato con el nombre {tipoDato.Nombre} ya existe.");
        }

        if(tipoDato.TipoPadre != null)
        {
            AsignarPadre(tipoDato.Id, tipoDato.TipoPadre.Id);
        }

        return tipoDato;
    }

    public bool ExisteTipoDato(string? nombre)
    {
        return _tipoDatoRepositorio.Existe(nombre);
    }

    public TipoDato ObtenerTipoDatoPoId(Guid id)
    {
        var tipoDato = _tipoDatoRepositorio.Obtener(id);

        if (tipoDato == null)
        {
            throw new EntidadNoEncontradaExcepcion($"No se encontró un TipoDato con el ID: {id}");
        }

        return tipoDato;
    }

    public List<TipoDato> ObtenerTodosLosTipoDatos()
    {
        return _tipoDatoRepositorio.ObtenerTodos();
    }

    public void AsignarPadre(Guid tipoId, Guid padreId)
    {
        var tipo = _tipoDatoRepositorio.Obtener(tipoId);
        if(tipo == null)
        {
            throw new EntidadNoEncontradaExcepcion(
                $"No se encontró TipoDato con ID {tipoId}");
        }

        var padre = _tipoDatoRepositorio.Obtener(padreId);
        if(padre == null)
        {
            throw new EntidadNoEncontradaExcepcion(
                $"No se encontró TipoDato padre con ID {padreId}");
        }

        tipo.TipoPadre = padre;
    }

    public bool EsCompatible(Guid referenciaId, Guid instanciaId)
    {
        var tipoReferencia = _tipoDatoRepositorio.Obtener(referenciaId);
        if(tipoReferencia == null)
        {
            throw new EntidadNoEncontradaExcepcion($"TipoReferencia {referenciaId} no encontrada");
        }

        var tipoInstancia = _tipoDatoRepositorio.Obtener(instanciaId);
        if(tipoInstancia == null)
        {
            throw new EntidadNoEncontradaExcepcion($"TipoInstancia {instanciaId} no encontrada");
        }

        var tipo = tipoInstancia;
        while(tipo != null)
        {
            if(tipo.Id == tipoReferencia.Id)
            {
                return true;
            }

            tipo = tipo.TipoPadre;
        }

        return false;
    }

    public void EliminarTipoDato(TipoDato tipoDato)
    {
        _tipoDatoRepositorio.EliminarTipoDato(tipoDato);
    }
}
