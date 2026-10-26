using System.Text.Json.Nodes;
using Dominio;
using Dominio.Excepciones;
using Reflection;
using Reflection.InterfacesReflection;
using Servicios.InterfacesServicios;

namespace Servicios;

public class SimulacionServicio : ISimulacionServicio
{
    private readonly IMetodoServicio _metodoServicio;
    private readonly IClaseServicio _claseServicio;
    private readonly ICargadorDePlugins<ITransformador> _cargadorDePlugins;

    public SimulacionServicio(IClaseServicio claseServicio, IMetodoServicio metodoServicio,
        ICargadorDePlugins<ITransformador> cargadorDePlugins)
    {
        _claseServicio = claseServicio;
        _metodoServicio = metodoServicio;
        _cargadorDePlugins = cargadorDePlugins;
    }

    public string EjecutarSimulacionTransformada(Clase? tipoReferencia, Clase? tipoObjeto, Metodo metodoReferencia,
        string prefijo, string? nombreTransformador)
    {
        var resultado = EjecutarSimulacion(tipoReferencia, tipoObjeto, metodoReferencia, prefijo);

        if(string.IsNullOrWhiteSpace(nombreTransformador))
        {
            return FormatearSimulacion(resultado);
        }

        var nombres = _cargadorDePlugins.ObtenerImplementaciones();
        var indice = nombres.FindIndex(n => n == nombreTransformador);

        if(indice == -1)
        {
            throw new OperacionNoPermitidaExcepcion($"No se encontro el transformador '{nombreTransformador}'");
        }

        var transformador = _cargadorDePlugins.ObtenerImplementacion(indice);
        return transformador.Transformar(resultado);
    }

    public JsonObject EjecutarSimulacion(Clase? tipoReferencia, Clase? tipoObjeto, Metodo metodoReferencia,
        string prefijo, bool validarInstanciacion = true)
    {
        if(validarInstanciacion)
        {
            ValidarEntradas(tipoReferencia, tipoObjeto);
        }

        var llamadaInicial = ConstruirLlamadaInicial(prefijo, metodoReferencia);
        JsonArray metodosEjecutados = ConstruirMetodosEjecutados(tipoObjeto, metodoReferencia);
        return new JsonObject
        {
            ["LlamadaInicial"] = llamadaInicial,
            ["MetodosEjecutados"] = metodosEjecutados
        };
    }

    private void ValidarEntradas(Clase? tipoReferencia, Clase? tipoObjeto)
    {
        if (tipoReferencia == null)
        {
            throw new EntidadNulaExcepcion("La clase de referencia no puede ser nula.");
        }

        if (tipoObjeto == null)
        {
            throw new EntidadNulaExcepcion("La clase del objeto a instanciar no puede ser nula.");
        }

        if (tipoObjeto.EsAbstract)
        {
            throw new OperacionNoPermitidaExcepcion("No se puede instanciar una clase abstracta.");
        }
    }

    private string ConstruirLlamadaInicial(string prefijo, Metodo metodo)
    {
        var llamada = prefijo + "." + metodo.Nombre + "(";
        var cantidadParametros = metodo.Parametros.Count;

        for (var i = 0; i < cantidadParametros; i++)
        {
            var parametro = metodo.Parametros[i];
            llamada += parametro.TipoDato.Nombre + " " + parametro.Nombre;

            if (cantidadParametros > 1 && i != cantidadParametros - 1)
            {
                llamada += ", ";
            }
        }

        llamada += ")";
        return llamada;
    }

    private JsonArray ConstruirMetodosEjecutados(Clase? tipoObjeto, Metodo metodoReferencia)
    {
        JsonArray resultado = [];

        foreach (var metodoInvocado in metodoReferencia.MetodosInvocados)
        {
            var prefijo = DeterminarPrefijo(tipoObjeto, metodoReferencia, metodoInvocado);
            resultado.Add(EjecutarSimulacion(metodoInvocado.Clase!, metodoInvocado.Clase!,
                metodoInvocado, prefijo, validarInstanciacion: false));
        }

        return resultado;
    }

    private string DeterminarPrefijo(Clase? tipoObjeto, Metodo metodoReferencia, Metodo metodoInvocado)
    {
        if (metodoInvocado.Clase.Nombre == tipoObjeto.Nombre)
        {
            return "this";
        }

        if (tipoObjeto.ClaseBase != null && metodoInvocado.Clase.Nombre == tipoObjeto.ClaseBase.Nombre)
        {
            return "base";
        }

        var coincidencia = metodoReferencia.Parametros!
            .FirstOrDefault(p => p.TipoDato.Nombre == metodoInvocado.Clase.Nombre);

        return coincidencia?.Nombre ?? metodoInvocado.Clase.Nombre;
    }

    private string FormatearSimulacion(JsonObject resultado, int nivel = 0)
    {
        var indent = new string(' ', nivel * 4);
        var llamada = resultado["LlamadaInicial"]?.ToString() ?? string.Empty;
        var ejecutados = resultado["MetodosEjecutados"]?.AsArray() ?? [];

        var lineas = new List<string> { indent + llamada };

        foreach (var ejecutado in ejecutados)
        {
            if (ejecutado is JsonObject obj)
            {
                lineas.Add(FormatearSimulacion(obj, nivel + 1));
            }
        }

        return string.Join("\n", lineas);
    }
}
