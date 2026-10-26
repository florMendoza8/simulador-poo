using AccesoADatos.InterfacesRepositorios;
using Dominio;
using Microsoft.EntityFrameworkCore;

namespace AccesoADatos.Repositorios;

public class NamespaceRepositorio : INamespaceRepositorio
{
    private readonly DbContext _contexto;

    public NamespaceRepositorio(DbContext contexto)
    {
        _contexto = contexto;
    }

    public void Agregar(Namespace nameNs)
    {
        _contexto.Set<Namespace>().Add(nameNs);
        _contexto.SaveChanges();
    }

    public bool Existe(Namespace nameNs)
    {
        return _contexto.Set<Namespace>()
            .Any(c => c.Id == nameNs.Id && c.Nombre == nameNs.Nombre && c.Padre == nameNs.Padre);
    }

    public Namespace? Obtener(Guid? id)
    {
        return _contexto.Set<Namespace>()
            .Include(n => n.Padre)
            .Include(n => n.Clases)
            .Include(n => n.Interfazes)
            .Include(n => n.NamespacesHijos)
            .FirstOrDefault(n => n.Id == id);
    }

    public List<Namespace> ObtenerTodos()
    {
        return _contexto.Set<Namespace>()
            .Include(n => n.Padre)
            .Include(n => n.Clases)
            .Include(n => n.Interfazes)
            .Include(n => n.NamespacesHijos)
            .ToList();
    }

    public void Eliminar(Namespace? nameNs)
    {
        var aEliminar = _contexto.Set<Namespace>().FirstOrDefault(a => a.Id == nameNs.Id);
        _contexto.Set<Namespace>().Remove(aEliminar!);
        _contexto.SaveChanges();
    }

    public void Actualizar(Namespace nameNs)
    {
        _contexto.Set<Namespace>().Update(nameNs);
        _contexto.SaveChanges();
    }
}
