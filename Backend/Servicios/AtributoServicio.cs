using AccesoADatos.InterfacesRepositorios;
using Dominio;
using Dominio.Excepciones;
using Servicios.InterfacesServicios;

namespace Servicios;

public class AtributoServicio : IAtributoServicio
{
    private readonly IAtributoRepositorio _atributoRepositorio;
    private readonly ITipoDatoServicio _tipoDatoServicio;

    public AtributoServicio(IAtributoRepositorio atributoRepositorio, ITipoDatoServicio tipoDatoServicio)
    {
        _atributoRepositorio = atributoRepositorio;
        _tipoDatoServicio = tipoDatoServicio;
    }

    public Atributo CrearAtributo(Atributo atributo)
    {
        ValidarDatosAtributo(atributo);

        _atributoRepositorio.Agregar(atributo);
        return atributo;
    }

    public void EliminarAtributo(Atributo atributo)
    {
        ValidarAtributoNoNulo(atributo);

        _atributoRepositorio.Eliminar(atributo);
    }

    public void ModificarAtributo(Atributo atributo)
    {
        ValidarDatosAtributo(atributo);
        _atributoRepositorio.Actualizar(atributo);
    }

    private void ValidarDatosAtributo(Atributo atributo)
    {
        ValidarAtributoNoNulo(atributo);

        if (string.IsNullOrWhiteSpace(atributo.Nombre))
        {
            throw new EntidadNulaExcepcion("El nombre no puede ser nulo o vacío");
        }

        if (atributo.TipoReferencia == null)
        {
            throw new EntidadNulaExcepcion("El tipo de dato no puede ser nulo");
        }

        if(atributo.TipoReferencia.Id == Guid.Empty)
        {
            throw new EntidadNulaExcepcion("El Id del tipo de dato no puede ser vacío");
        }

        if (!Enum.IsDefined(typeof(Accesibilidad), atributo.Accesibilidad))
        {
            throw new OperacionNoPermitidaExcepcion("Accesibilidad inválida");
        }
    }

    private static void ValidarAtributoNoNulo(Atributo atributo)
    {
        if (atributo == null)
        {
            throw new EntidadNulaExcepcion("El atributo no puede ser nulo");
        }
    }

    public bool ExisteAtributo(string? nombre, TipoDato tipoDato, Accesibilidad accesibilidad)
    {
        return _atributoRepositorio.Existe(nombre, tipoDato, accesibilidad);
    }

    public Atributo ObtenerAtributoPorId(Guid id)
    {
        var atributo = _atributoRepositorio.Obtener(id);

        if (atributo == null)
        {
            throw new EntidadNoEncontradaExcepcion($"No se encontró un atributo con el ID: {id}");
        }

        return atributo;
    }

    public List<Atributo> ObtenerTodosLosAtributos()
    {
        return _atributoRepositorio.ObtenerTodos();
    }
}
