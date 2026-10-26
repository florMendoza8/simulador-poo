using AccesoADatos.Repositorios;
using Dominio;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AccesoADatosTest;

[TestClass]
public class ClaseRepositorioTest
{
    private Guid _id;
    private IQueryable<Clase?>? _iqclase;
    private Mock<DbSet<Clase>>? _mock;
    private Clase? _clase;
    private ClaseRepositorio? _claseRepositorio;
    private Mock<DbContext>? _contexto;

    [TestInitialize]
    public void Inicializar()
    {
        _id = Guid.NewGuid();
        _clase = new Clase { Id = _id, Nombre = "clasePrueba", EsAbstract = false, EsSealed = false, Atributos = new List<Atributo>(), MetodosInvocados = new List<Metodo>() };
        _iqclase = new List<Clase?> { _clase }.AsQueryable();
        _mock = new Mock<DbSet<Clase>>();
        _mock.As<IQueryable<Clase>>().Setup(m => m.Provider).Returns(_iqclase.Provider);
        _mock.As<IQueryable<Clase>>().Setup(m => m.Expression).Returns(_iqclase.Expression);
        _mock.As<IQueryable<Clase>>().Setup(m => m.ElementType).Returns(_iqclase.ElementType);
        _mock.As<IQueryable<Clase>>().Setup(m => m.GetEnumerator()).Returns(() => _iqclase.GetEnumerator());
        _contexto = new Mock<DbContext>();
        _claseRepositorio = new ClaseRepositorio(_contexto.Object);
        _contexto.Setup(x => x.Set<Clase>()).Returns(_mock.Object);
    }

    [TestMethod]
    public void AgregarClaseTest()
    {
        _contexto.Setup(x => x.SaveChanges()).Returns(1);
        _claseRepositorio.Agregar(_clase!);
        _contexto.Verify(conectoSistema => conectoSistema.SaveChanges(), Times.Once());
    }

    [TestMethod]
    public void ExisteClaseTest()
    {
        var resultado = _claseRepositorio.Existe(_clase!);
        Assert.IsTrue(resultado);
    }

    [TestMethod]
    public void ObtenerClaseTest()
    {
        var resultado = _claseRepositorio.ObtenerClase(_clase.Id);
        Assert.AreEqual(_clase.Id, resultado.Id);
    }

    [TestMethod]
    public void ObtenerTodasLasClasesTest()
    {
        List<Clase?> resultado = _claseRepositorio.ObtenerTodasLasClases();
        Assert.IsTrue(resultado.Contains(_clase!));
        Assert.AreEqual(resultado.Count, 1);
    }

    [TestMethod]
    public void EliminarClaseTest()
    {
        _contexto.Setup(x => x.SaveChanges()).Returns(1);
        _claseRepositorio.EliminarClase(_clase!);
        _contexto.Verify(conectoSistema => conectoSistema.SaveChanges(), Times.Once());
    }

    [TestMethod]
    public void ActualizarClaseTest()
    {
        var claseActualizada = new Clase
        {
        Id = _id,
        Nombre = "ClaseModificada",
        EsAbstract = true,
        EsSealed = true,
        Atributos = new List<Atributo>(),
        MetodosInvocados = new List<Metodo>()
    };
    _claseRepositorio.Actualizar(claseActualizada);
    _contexto.Verify(c => c.Set<Clase>().Update(It.Is<Clase>(c => c.Id == _id)), Times.Once);
    _contexto.Verify(c => c.SaveChanges(), Times.Once);
    }
}
