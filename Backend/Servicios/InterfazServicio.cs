using AccesoADatos.InterfacesRepositorios;
using Dominio;
using Dominio.Excepciones;
using Servicios.InterfacesServicios;

namespace Servicios;

public class InterfazServicio : IInterfazServicio
{
    private readonly IInterfazRepositorio _interfazRepositorio;
    private readonly IMetodoServicio _metodoServicio;

    public InterfazServicio(IInterfazRepositorio interfazRepositorio, IMetodoServicio metodoServicio)
    {
        _interfazRepositorio = interfazRepositorio;
        _metodoServicio = metodoServicio;
    }

    public Interfaz CrearInterfaz(Interfaz interfaz)
    {
        ValidarInterfaz(interfaz);
        try
        {
            _interfazRepositorio.Agregar(interfaz);
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR EF Core:");
            Console.WriteLine(ex.ToString());
            throw;
        }

        return interfaz;
    }

    public void ValidarInterfaz(Interfaz interfaz)
    {
        if(interfaz == null)
        {
            throw new EntidadNulaExcepcion("La interfaz no puede ser nula");
        }

        if(string.IsNullOrWhiteSpace(interfaz.Nombre))
        {
            throw new EntidadNulaExcepcion("El nombre no puede ser nulo o vacío");
        }
    }

    public void EliminarInterfaz(Interfaz interfaz)
    {
        if(interfaz == null)
        {
            throw new EntidadNulaExcepcion("La interfaz no puede ser nula");
        }

        _interfazRepositorio.Eliminar(interfaz);
    }

    public Interfaz ObtenerInterfazPorId(Guid id)
    {
        var interfaz = _interfazRepositorio.Obtener(id);

        if(interfaz == null)
        {
            throw new EntidadNoEncontradaExcepcion($"No se encontró una interfaz con el ID: {id}");
        }

        return interfaz;
    }

    public List<Interfaz> ObtenerTodasLasInterfaces()
    {
        return _interfazRepositorio.ObtenerTodos();
    }

    public void AgregarMetodoAFirmaMetodos(Guid interfazId, Guid metodoId)
    {
        if (interfazId == Guid.Empty)
        {
            throw new EntidadNulaExcepcion("El Id de la interfaz no puede estar vacio.");
        }

        if (metodoId == Guid.Empty)
        {
            throw new EntidadNulaExcepcion("El Id del metodo no puede estar vacio.");
        }

        var interfaz = _interfazRepositorio.Obtener(interfazId);
        if(interfaz == null)
        {
            throw new EntidadNulaExcepcion("La interfaz no puede ser nula");
        }

        var metodo = _metodoServicio.ObtenerMetodoPorId(metodoId);
        if(metodo == null)
        {
            throw new EntidadNulaExcepcion("El método no puede ser nulo");
        }

        if(interfaz.FirmaMetodos == null)
        {
            interfaz.FirmaMetodos = new List<Metodo>();
        }

        ValidarSiFirmaMetodoYaExiste(interfaz, metodo);
        interfaz.FirmaMetodos.Add(metodo);
        _interfazRepositorio.Actualizar(interfaz);
    }

    private static void ValidarSiFirmaMetodoYaExiste(Interfaz interfaz, Metodo metodo)
    {
        if(interfaz.FirmaMetodos != null && interfaz.FirmaMetodos.Any(m => FirmaMetodosSonIguales(m, metodo)))
        {
            throw new EntidadYaExisteExcepcion(
                $"Ya existe una firma de método con el nombre '{metodo.Nombre}' y los mismos parametros");
        }
    }

    private static bool FirmaMetodosSonIguales(Metodo primerMetodo, Metodo segundoMetodo)
    {
        if(primerMetodo.Nombre != segundoMetodo.Nombre)
        {
            return false;
        }

        if(primerMetodo.Parametros == null || segundoMetodo.Parametros == null)
        {
            return false;
        }

        if(segundoMetodo.Parametros != null && primerMetodo.Parametros != null &&
           primerMetodo.Parametros.Count != segundoMetodo.Parametros.Count)
        {
            return false;
        }

        return segundoMetodo.Parametros != null && primerMetodo.Parametros != null && primerMetodo.Parametros
            .Zip(segundoMetodo.Parametros, (p1, p2) => p1.TipoDato != null && p1.TipoDato.Equals(p2.TipoDato))
            .All(x => x);
    }
}
