using AccesoADatos.Repositorios;
using Dominio;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AccesoADatosTest;

[TestClass]
public class NamespaceRepositorioTest
{
    private Guid _id;
    private IQueryable<Namespace?>? _iqnamespace;
    private Mock<DbSet<Namespace>>? _mock;
    private Namespace? _namespace;
    private Namespace? _hijo;
    private NamespaceRepositorio? _namespaceRepositorio;
    private Mock<DbContext>? _contexto;

    [TestInitialize]
    public void Inicializar()
    {
        _id = Guid.NewGuid();
        _namespace = new Namespace("padre", null) { Id = _id };
        _hijo = new Namespace("Child", _namespace) { Id = Guid.NewGuid() };
        _iqnamespace = new List<Namespace?> { _namespace, _hijo }.AsQueryable();

         _mock = new Mock<DbSet<Namespace>>();
        _mock.As<IQueryable<Namespace>>().Setup(m => m.Provider).Returns(_iqnamespace.Provider);
        _mock.As<IQueryable<Namespace>>().Setup(m => m.Expression).Returns(_iqnamespace.Expression);
        _mock.As<IQueryable<Namespace>>().Setup(m => m.ElementType).Returns(_iqnamespace.ElementType);
        _mock.As<IQueryable<Namespace>>().Setup(m => m.GetEnumerator()).Returns(() => _iqnamespace.GetEnumerator());

        _contexto = new Mock<DbContext>();
        _namespaceRepositorio = new NamespaceRepositorio(_contexto.Object);
        _contexto.Setup(x => x.Set<Namespace>()).Returns(_mock.Object);
    }

    [TestMethod]
    public void AgregarTest()
    {
        _contexto?.Setup(x => x.SaveChanges()).Returns(1);
        _namespaceRepositorio?.Agregar(_namespace!);
        _contexto?.Verify(conectoSistema => conectoSistema.SaveChanges(), Times.Once());
    }

    [TestMethod]
    public void ExisteTest()
    {
        var resultado = _namespaceRepositorio?.Existe(_namespace!);
        Assert.IsTrue(resultado);
    }

    [TestMethod]
    public void ObtenerTest()
    {
        var resultado = _namespaceRepositorio?.Obtener(_namespace?.Id);
        Assert.AreEqual(_namespace?.Id, resultado?.Id);
    }

    [TestMethod]
    public void ObtenerTodosTest()
    {
        List<Namespace> resultado = _namespaceRepositorio.ObtenerTodos();
        Assert.IsTrue(resultado?.Contains(_namespace!));
        Assert.AreEqual(resultado?.Count, 2);
    }

    [TestMethod]
    public void EliminarTest()
    {
        _contexto?.Setup(x => x.SaveChanges()).Returns(1);
        _namespaceRepositorio?.Eliminar(_namespace!);
        _contexto?.Verify(conectoSistema => conectoSistema.SaveChanges(), Times.Once());
    }

    [TestMethod]
    public void ActualizarTest()
    {
        _id = Guid.NewGuid();
        _namespace = new Namespace("padre", null) { Id = _id };
        _hijo = new Namespace("hijo", _namespace) { Id = Guid.NewGuid() };
        _namespaceRepositorio.Actualizar(_hijo);
        _contexto?.Verify(c => c.Set<Namespace>().Update(It.Is<Namespace>(c => c.Id == _hijo.Id)), Times.Once);
        _contexto?.Verify(c => c.SaveChanges(), Times.Once);
    }
}
