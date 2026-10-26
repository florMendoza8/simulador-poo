using AccesoADatos.InterfacesRepositorios;
using Dominio;
using Dominio.Excepciones;
using Servicios.InterfacesServicios;

namespace Servicios;

public class NamespaceServicio : INamespaceServicio
{
    private readonly INamespaceRepositorio _repositorio;

    public NamespaceServicio(INamespaceRepositorio namespaceRepositorio)
    {
        _repositorio = namespaceRepositorio;
    }

    public Namespace Crear(Namespace nuevoNS)
    {
        ValidarNamespaceNoNula(nuevoNS);
        ValidarNombre(nuevoNS.Nombre);

        if(nuevoNS.Padre != null)
        {
           ValidarPadreExiste(nuevoNS.Padre.Id);
           ValidarSiNamespaceTieneMismoPadre(nuevoNS);
           ValidarNoAutoPadre(nuevoNS);
           ValidarHerenciaCircular(nuevoNS.Padre, nuevoNS.Id);
        }

        _repositorio.Agregar(nuevoNS);
        return nuevoNS;
    }

    public void Renombrar(Guid id, string nuevoNombre)
    {
        ValidarNombre(nuevoNombre);
        var ns = _repositorio.Obtener(id);
        ValidarNamespaceExiste(ns);

        if (ns.Padre != null)
        {
            var nombreAntiguo = ns.Nombre;
            ns.Nombre = nuevoNombre;
            ValidarSiNamespaceTieneMismoPadre(ns);
        }

        ns.Nombre = nuevoNombre;
        _repositorio.Actualizar(ns);
    }

    public void CambiarPadre(Guid id, Guid? nuevoPadreId)
    {
        var name_space = _repositorio.Obtener(id);
        ValidarNamespaceExiste(name_space);

        Namespace? nuevoPadre = null;
        if (nuevoPadreId.HasValue)
        {
            var idPadre = nuevoPadreId.Value;
            ValidarNoMismoID(id, idPadre);
            nuevoPadre = _repositorio.Obtener(idPadre);
            ValidarPadreExiste(idPadre);
            ValidarHerenciaCircular(nuevoPadre, id);

            name_space.Padre = nuevoPadre;
            ValidarNoAutoPadre(name_space);
            ValidarSiNamespaceTieneMismoPadre(name_space);
        }

        name_space.Padre = nuevoPadre;
        _repositorio.Actualizar(name_space);
    }

    public void Eliminar(Guid id)
    {
        var name_space = _repositorio.Obtener(id);
        ValidarNamespaceExiste(name_space);
        ValidarNamespaceConHijos(id);

        _repositorio.Eliminar(name_space);
    }

    public List<Namespace> ListarTodos()
    {
        return _repositorio.ObtenerTodos();
    }

    public Namespace Obtener(Guid id)
    {
        var resultado = _repositorio.Obtener(id);
        ValidarNamespaceExiste(resultado);
        return resultado;
    }

    public void AgregarElemento(Guid namespaceId, Elemento? elemento)
    {
        var ns = _repositorio.Obtener(namespaceId);
        ValidarNamespaceExiste(ns);

        if(elemento == null)
        {
            throw new EntidadNulaExcepcion("Elemento no puede ser vacio");
        }

        if(ns.ObtenerElementos().Contains(elemento))
        {
            throw new OperacionNoPermitidaExcepcion("elemento ya existe");
        }

        ns.Agregar(elemento);
        _repositorio.Actualizar(ns);
    }

    public void EliminarElemento(Guid namespaceId, Guid elementoId)
    {
        var ns = _repositorio.Obtener(namespaceId);
        ValidarNamespaceExiste(ns);
        var elem = ns.ObtenerElemento(elementoId);
        if(elem == null)
        {
            throw new EntidadNoEncontradaExcepcion("Elemento no encontrado en este namespace");
        }

        ns.Eliminar(elem);
        _repositorio.Actualizar(ns);
    }

    public Elemento ObtenerElementoEnArbol(Guid id)
    {
        foreach (var root in _repositorio.ObtenerTodos().Where(n => n.Padre == null))
        {
            var encontrado = root.ObtenerElemento(id);
            if(encontrado != null)
            {
                return encontrado;
            }
        }

        throw new EntidadNoEncontradaExcepcion("Elemento no encontrado en ningún namespace");
    }

    public void AgregarClase(Guid namespaceId, Clase? clase)
    {
        if(clase == null)
        {
            throw new EntidadNulaExcepcion("La clase no puedo ser vacia");
        }

        var ns = _repositorio.Obtener(namespaceId);
        ValidarNamespaceExiste(ns);
        ns.Agregar(clase);
        _repositorio.Actualizar(ns);
    }

    public void AgregarInterfaz(Guid namespaceId, Interfaz? interfaz)
    {
        if(interfaz == null)
        {
            throw new EntidadNulaExcepcion("La interfaz no puedo ser vacia");
        }

        var ns = _repositorio.Obtener(namespaceId);
        ValidarNamespaceExiste(ns);
        ns.Agregar(interfaz);
        _repositorio.Actualizar(ns);
    }

    public IEnumerable<Elemento?> ObtenerHijos(Guid namespaceId)
    {
        var ns = _repositorio.Obtener(namespaceId);
        if(ns == null)
        {
            throw new EntidadNoEncontradaExcepcion("Namespace no encontrado");
        }

        return ns.ObtenerElementos();
    }

    public IEnumerable<Namespace> ObtenerArbolCompleto()
    {
        var todos = _repositorio.ObtenerTodos() ?? Enumerable.Empty<Namespace>();
        return todos.Where(n => n.Padre == null);
    }

    private static void ValidarNombre(string? nombre)
    {
        if(string.IsNullOrWhiteSpace(nombre))
        {
            throw new EntidadNulaExcepcion("Nombre namespace no puede ser vacio");
        }
    }

    private static void ValidarNamespaceNoNula(Namespace nameNs)
    {
        if(nameNs == null)
        {
            throw new EntidadNulaExcepcion("Nombre namespace no puede ser nulo");
        }
    }

    private void ValidarPadreExiste(Guid padreId)
    {
        Namespace? nuevoPadre = _repositorio.Obtener(padreId);
        if(nuevoPadre == null)
        {
            throw new EntidadNoEncontradaExcepcion($"Padre {nuevoPadre} no encontrado");
        }
    }

    private bool ExisteCiclo(Namespace? candidato, Guid idHijo)
    {
        var actual = candidato;
        while (actual != null)
        {
            if(actual.Id == idHijo)
            {
                return true;
            }

            actual = actual.Padre;
        }

        return false;
    }

    private void ValidarHerenciaCircular(Namespace? candidato, Guid idHijo)
    {
        if(ExisteCiclo(candidato, idHijo))
        {
            throw new OperacionNoPermitidaExcepcion("Asignar este padre generaría una herencia circular");
        }
    }

    private void ValidarNoAutoPadre(Namespace ns)
    {
        if(ns.Padre != null && ns.Id == ns.Padre.Id)
        {
            throw new OperacionNoPermitidaExcepcion("Un namespace no puede ser padre de sí mismo");
        }
    }

    private void ValidarNamespaceExiste(Namespace? name_space)
    {
        if(name_space == null)
        {
            throw new EntidadNoEncontradaExcepcion($"Namespace no encontrado");
        }
    }

    private void ValidarNoMismoID(Guid? id, Guid? nuevoPadreId)
    {
        if(nuevoPadreId.HasValue)
        {
            if(nuevoPadreId.Value == id)
            {
                throw new OperacionNoPermitidaExcepcion("Un namespace no puede ser padre de si mismo");
            }
        }
    }

    private void ValidarNamespaceConHijos(Guid id)
    {
        var hijos = _repositorio.ObtenerTodos().Where(x => x.Padre != null && x.Padre.Id == id);
        if(hijos.Any())
        {
            throw new OperacionNoPermitidaExcepcion("No se puede borrar un namespace con hijos");
        }
    }

    private void ValidarSiNamespaceTieneMismoPadre(Namespace nuevoNS)
    {
        var padre = _repositorio.Obtener(nuevoNS.Padre!.Id)
                    ?? throw new EntidadNoEncontradaExcepcion("Padre no encontrado");
        var duplicado = padre.ObtenerElementos()
            .OfType<Namespace>()
            .Any(hijo =>
                hijo.Id != nuevoNS.Id &&
                string.Equals(hijo.Nombre, nuevoNS.Nombre, StringComparison.OrdinalIgnoreCase));

        if (duplicado)
        {
            throw new OperacionNoPermitidaExcepcion(
                $"Ya existe un namespace con nombre '{nuevoNS.Nombre}' bajo el mismo padre.");
        }
    }
}
