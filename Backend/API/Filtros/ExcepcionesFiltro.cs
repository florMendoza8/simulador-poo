using Dominio.Excepciones;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Filtros;

public class ExcepcionesFiltro : Attribute, IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var codigoDeEstado = 500;
        var respuesta = new RespuestaDto
        {
            EjecucionExitosa = false,
            Mensaje = "Error Interno del Servidor"
        };

        if (context.Exception is EntidadNulaExcepcion)
        {
            codigoDeEstado = 400;
            respuesta.Mensaje = context.Exception.Message;
        }

        if (context.Exception is EntidadYaExisteExcepcion)
        {
            codigoDeEstado = 409;
            respuesta.Mensaje = context.Exception.Message;
        }

        if (context.Exception is EntidadNoEncontradaExcepcion)
        {
            codigoDeEstado = 404;
            respuesta.Mensaje = context.Exception.Message;
        }

        if (context.Exception is OperacionNoPermitidaExcepcion)
        {
            codigoDeEstado = 403;
            respuesta.Mensaje = context.Exception.Message;
        }

        if(context.Exception is NoAutorizadoExcepcion)
        {
            codigoDeEstado = 401;
            respuesta.Mensaje = context.Exception.Message;
        }

        context.Result = new ObjectResult(respuesta)
        {
            StatusCode = codigoDeEstado
        };

        context.ExceptionHandled = true;
    }
}
