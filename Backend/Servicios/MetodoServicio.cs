using AccesoADatos.InterfacesRepositorios;
using Dominio;
using Dominio.Excepciones;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Servicios.InterfacesServicios;

namespace Servicios;

public class MetodoServicio : IMetodoServicio
{
    private readonly IMetodoRepositorio _metodoRepositorio;
    private readonly IParametroServicio _parametroServicio;
    private readonly IVariableLocalServicio _variableLocalServicio;

    public MetodoServicio(IMetodoRepositorio metodoRepositorio, IParametroServicio parametroServicio,
        IVariableLocalServicio variableLocalServicio)
    {
        _metodoRepositorio = metodoRepositorio;
        _parametroServicio = parametroServicio;
        _variableLocalServicio = variableLocalServicio;
    }

    public Metodo AgregarMetodo(Metodo metodo)
    {
        ValidarMetodo(metodo);

        _metodoRepositorio.Agregar(metodo);
        return metodo;
    }

    public void EliminarMetodo(Metodo metodo)
    {
        ValidarMetodoNoNulo(metodo);
        _metodoRepositorio.Eliminar(metodo);
    }

    public Metodo ObtenerMetodoPorId(Guid metodoId)
    {
        if (metodoId == Guid.Empty)
        {
            throw new EntidadNulaExcepcion("El id del método no puede estar vacío.");
        }

        var metodo = _metodoRepositorio.Obtener(metodoId);
        if (metodo == null)
        {
            throw new EntidadNulaExcepcion($"No se encontró un método con id: {metodoId}");
        }

        return metodo;
    }

    public List<Metodo> ObtenerTodosMetodos()
    {
        return _metodoRepositorio.ObtenerTodos();
    }

    public void AgregarParametro(Guid metodoId, Guid parametroId)
    {
        if (metodoId == Guid.Empty)
        {
            throw new EntidadNulaExcepcion("El Id del metodo no puede estar vacio.");
        }

        if (parametroId == Guid.Empty)
        {
            throw new EntidadNulaExcepcion("El Id del parametro no puede estar vacio.");
        }

        var metodo = _metodoRepositorio.Obtener(metodoId);
        if (metodo == null)
        {
            throw new EntidadNoEncontradaExcepcion("El metodo no existe.");
        }

        if (metodo.Parametros == null)
        {
            metodo.Parametros = new List<Parametro>();
        }

        var parametro = _parametroServicio.ObtenerParametroPorId(parametroId);
        if (parametro == null)
        {
            throw new EntidadNoEncontradaExcepcion("El parametro no existe.");
        }

        if (metodo.Parametros!.Any(p => p.Nombre == parametro.Nombre))
        {
            throw new EntidadYaExisteExcepcion("Ya existe un parametro con igual nombre en el metodo.");
        }

        metodo.Parametros.Add(parametro);
        _metodoRepositorio.Actualizar(metodo);
    }

    public void AgregarVariableLocal(Guid metodoId, Guid variableLocalId)
    {
        if (metodoId == Guid.Empty)
        {
            throw new EntidadNulaExcepcion("El Id del metodo no puede estar vacio.");
        }

        if (variableLocalId == Guid.Empty)
        {
            throw new EntidadNulaExcepcion("El Id de la variable lcoal no puede estar vacio.");
        }

        var metodo = _metodoRepositorio.Obtener(metodoId);
        if (metodo == null)
        {
            throw new EntidadNoEncontradaExcepcion("El metodo no existe.");
        }

        if (metodo.VariablesLocales == null)
        {
            metodo.VariablesLocales = new List<VariableLocal>();
        }

        ValidarMetodoNoAbstracto(metodo);

        var variable = _variableLocalServicio.ObtenerVariableLocalPorId(variableLocalId);
        if (variable == null)
        {
            throw new EntidadNoEncontradaExcepcion("La variable local no existe.");
        }

        if (metodo.VariablesLocales!.Any(v => v.Nombre == variable.Nombre))
        {
            throw new EntidadYaExisteExcepcion("Ya existe una variable local con igual nombre en el metodo.");
        }

        metodo.VariablesLocales.Add(variable);
        _metodoRepositorio.Actualizar(metodo);
    }

    public void AgregarMetodoInvocado(Guid metodoOrigenId, Guid metodoInvocadoId)
    {
        if (metodoOrigenId == Guid.Empty)
        {
            throw new EntidadNulaExcepcion("El Id del metodo origen no puede estar vacio.");
        }

        if (metodoInvocadoId == Guid.Empty)
        {
            throw new EntidadNulaExcepcion("El Id del metodo invocado no puede estar vacio.");
        }

        var metodoOrigen = _metodoRepositorio.Obtener(metodoOrigenId);
        if (metodoOrigen == null)
        {
            throw new EntidadNoEncontradaExcepcion("El metodo origen no existe.");
        }

        if (metodoOrigen.MetodosInvocados == null)
        {
            metodoOrigen.MetodosInvocados = new List<Metodo>();
        }

        ValidarMetodoNoAbstracto(metodoOrigen);

        var metodoInvocado = _metodoRepositorio.Obtener(metodoInvocadoId);
        if (metodoInvocado == null)
        {
            throw new EntidadNoEncontradaExcepcion("El metodo invocado no existe.");
        }

        if (metodoOrigen.MetodosInvocados!.Any(m => m.Id == metodoInvocado.Id))
        {
            throw new EntidadYaExisteExcepcion("Ya existe un metodo invocado con igual id en el metodo origen.");
        }

        metodoOrigen.MetodosInvocados.Add(metodoInvocado);
        _metodoRepositorio.Actualizar(metodoOrigen);
    }

    public void ActualizarMetodo(Metodo metodo)
    {
        ValidarMetodo(metodo);
        _metodoRepositorio.Actualizar(metodo);
    }

    private static void ValidarMetodo(Metodo metodo)
    {
        ValidarMetodoNoNulo(metodo);
        ValidarNombreMetodo(metodo);
        ValidarTipoRetornoMetodo(metodo);
        ValidarAccesibilidadMetodo(metodo);
        ValidarEsSealedMetodo(metodo);
        ValidarParametrosNoNula(metodo);
        ValidarVariablesLocalesNoNula(metodo);
        ValidarMetodosInvocadosNoNula(metodo);
        ValidarMetodosInvocadosUnicosPorId(metodo);
        ValidarParametros(metodo.Parametros!);
        ValidarVariablesLocales(metodo.VariablesLocales!, metodo.Parametros!);
        ValidarMetodosInvocados(metodo);
        ValidarModificadoresMetodo(metodo);
    }

    public static void ValidarFirmaMetodo(Metodo metodo)
    {
        ValidarMetodoNoNulo(metodo);
        ValidarNombreMetodo(metodo);
        ValidarTipoRetornoMetodo(metodo);
        ValidarParametros(metodo.Parametros!);
    }

    private static void ValidarMetodoNoNulo(Metodo metodo)
    {
        if (metodo == null)
        {
            throw new EntidadNulaExcepcion("El metodo no puede ser nulo.");
        }
    }

    private static void ValidarNombreMetodo(Metodo metodo)
    {
        if(string.IsNullOrWhiteSpace(metodo.Nombre))
        {
            throw new EntidadNulaExcepcion("El nombre no puede ser nulo o vacío.");
        }
    }

    private static void ValidarTipoRetornoMetodo(Metodo metodo)
    {
        if(metodo.TipoRetorno == null)
        {
            throw new EntidadNulaExcepcion("El tipo de retorno no puede ser nulo.");
        }
    }

    private static void ValidarAccesibilidadMetodo(Metodo metodo)
    {
        if(metodo.Accesibilidad == null)
        {
            throw new EntidadNulaExcepcion("La accesibilidad no puede ser nula.");
        }
    }

    private static void ValidarEsSealedMetodo(Metodo metodo)
    {
        if(metodo.EsSealed == null)
        {
            throw new EntidadNulaExcepcion("Es sealed no puede ser nulo.");
        }
    }

    private static void ValidarParametrosNoNula(Metodo metodo)
    {
        if(metodo.Parametros == null)
        {
            throw new EntidadNulaExcepcion($"La lista de parametros no puede ser nula");
        }
    }

    private static void ValidarVariablesLocalesNoNula(Metodo metodo)
    {
        if(metodo.VariablesLocales == null)
        {
            throw new EntidadNulaExcepcion($"La lista de variables locales no puede ser nula");
        }
    }

    private static void ValidarMetodosInvocadosNoNula(Metodo metodo)
    {
        if(metodo.MetodosInvocados == null)
        {
            throw new EntidadNulaExcepcion($"La lista de metodos invocados no puede ser nula");
        }
    }

    private static void ValidarMetodosInvocadosUnicosPorId(Metodo metodo)
    {
        if (metodo.MetodosInvocados!.Any(m => m == null))
        {
            throw new EntidadNulaExcepcion("La lista de métodos invocados no puede contener elementos nulos.");
        }

        var duplicados = metodo.MetodosInvocados!
            .GroupBy(m => m.Id)
            .Where(g => g.Count() > 1)
            .Select(g => g.First().Nombre)
            .ToList();

        if (duplicados.Any())
        {
            var metodosDuplicados = string.Join(", ", duplicados);
            throw new EntidadYaExisteExcepcion($"Los métodos invocados están repetidos: {metodosDuplicados}");
        }
    }

    private static void ValidarParametros(List<Parametro> parametros)
    {
        var nombres = new HashSet<string>();

        foreach(var p in parametros)
        {
            if(p == null)
            {
                throw new EntidadNulaExcepcion("El parametro no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(p.Nombre))
            {
                throw new EntidadNulaExcepcion("Todos los parámetros deben tener nombre.");
            }

            if (p.TipoDato == null)
            {
                throw new EntidadNulaExcepcion($"El parametro {p.Nombre} no puede tener un tipo de dato nulo.");
            }

            if (!nombres.Add(p.Nombre))
            {
                throw new EntidadYaExisteExcepcion($"El parametro '{p.Nombre}' ya existe en la lista de parametros.");
            }
        }
    }

    private static void ValidarVariablesLocales(List<VariableLocal> variableLocal, List<Parametro> parametros)
    {
        var nombres = new HashSet<string>(parametros.Select(p => p.Nombre));

        foreach(var v in variableLocal)
        {
            if(v == null)
            {
                throw new EntidadNulaExcepcion("La variable local no puede ser nula");
            }

            if (string.IsNullOrWhiteSpace(v.Nombre))
            {
                throw new EntidadNulaExcepcion("Todas las variables locales deben tener nombre.");
            }

            if (v.TipoReferencia == null)
            {
                throw new EntidadNulaExcepcion($"La variable local {v.Nombre} no puede tener un tipo de dato nulo.");
            }

            if (!nombres.Add(v.Nombre))
            {
                throw new EntidadYaExisteExcepcion($"La variable local '{v.Nombre}' ya existe en la lista variables locales" +
                                                   $" y/o de parametros del metodo.");
            }
        }
    }

    private static void ValidarMetodosInvocados(Metodo metodo)
    {
        foreach(var metodoInvocado in metodo.MetodosInvocados)
        {
            if(metodoInvocado == null)
            {
                throw new EntidadNulaExcepcion("El metodo invocado no puede ser nulo");
            }

            ValidarMetodo(metodoInvocado);
        }
    }

    private static void ValidarModificadoresMetodo(Metodo metodo)
    {
        ValidarNoVirtualYOverride(metodo);
        ValidarEstaticoYNoOtroModificador(metodo);
        ValidarNoSealedYAbstract(metodo);
        ValidarNoAbstractYVirtualOOverride(metodo);
        ValidarAbstractSinImplementacion(metodo);
        ValidarSealedYOverride(metodo);
    }

    private static void ValidarSealedYOverride(Metodo metodo)
    {
        if(metodo.EsSealed == true && !metodo.EsOverride)
        {
            throw new OperacionNoPermitidaExcepcion("Un metodo sealed debe ser override");
        }
    }

    private static void ValidarAbstractSinImplementacion(Metodo metodo)
    {
        if(metodo.EsAbstract && (metodo.VariablesLocales!.Any() || metodo.MetodosInvocados!.Any()))
        {
            throw new OperacionNoPermitidaExcepcion("Un metodo abstracto no puede tener implementacion");
        }
    }

    private static void ValidarNoAbstractYVirtualOOverride(Metodo metodo)
    {
        if(metodo.EsAbstract && (metodo.EsVirtual || metodo.EsOverride))
        {
            throw new OperacionNoPermitidaExcepcion("Un metodo abstracto no puede ser virtual ni override");
        }
    }

    private static void ValidarNoSealedYAbstract(Metodo metodo)
    {
        if(metodo.EsSealed == true && metodo.EsAbstract)
        {
            throw new OperacionNoPermitidaExcepcion($"El metodo no puede ser Sealed y Abstract al mismo tiempo");
        }
    }

    private static void ValidarNoVirtualYOverride(Metodo metodo)
    {
        if(metodo.EsVirtual && metodo.EsOverride)
        {
            throw new OperacionNoPermitidaExcepcion("Un metodo no puede ser virtual y override al mismo tiempo");
        }
    }

    private static void ValidarEstaticoYNoOtroModificador(Metodo metodo)
    {
        if(metodo.EsEstatico && (metodo.EsVirtual || metodo.EsOverride || metodo.EsAbstract || metodo.EsSealed == true))
        {
            throw new OperacionNoPermitidaExcepcion("Un metodo estatico no puede ser virtual, override, abstract ni sealed");
        }
    }

    private static void ValidarMetodoNoAbstracto(Metodo metodo)
    {
        if (metodo.EsAbstract)
        {
            throw new OperacionNoPermitidaExcepcion("No se puede agregar implementación a un método abstracto.");
        }
    }

    public Metodo AgregarFirmaMetodo(Metodo metodo)
    {
        ValidarFirmaMetodo(metodo);

        _metodoRepositorio.Agregar(metodo);
        return metodo;
    }

    public List<Metodo> ObtenerTodasFirmaMetodos()
    {
        return _metodoRepositorio.ObtenerTodos()
            .Where(m =>
                m.Clase == null &&
                (m.VariablesLocales == null || !m.VariablesLocales.Any()) &&
                (m.MetodosInvocados == null || !m.MetodosInvocados.Any()) &&
                m.Accesibilidad == null &&
                m.EsAbstract == false &&
                m.EsEstatico == false &&
                m.EsVirtual == false &&
                m.EsOverride == false &&
                (m.EsSealed == null || m.EsSealed == false))
            .ToList();
    }
}
