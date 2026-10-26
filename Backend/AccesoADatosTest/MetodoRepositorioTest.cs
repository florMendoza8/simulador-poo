using AccesoADatos.Repositorios;
using Dominio;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AccesoADatosTest;

[TestClass]
public class MetodoRepositorioTest
{
    private Guid _id;
    private IQueryable<Metodo>? _data;
    private Mock<DbSet<Metodo>>? _mockSet;
    private Mock<DbContext>? _metodoManagerContextMock;
    private MetodoRepositorio? _metodoAccesoADatos;
    private Metodo _metodo = null;

    [TestInitialize]
    public void Inicializar()
    {
        _id = Guid.NewGuid();
        _metodo = new Metodo
        {
            Id = _id,
            Nombre = "Nombre",
            TipoRetorno = new TipoDato(),
            EsAbstract = false,
            EsSealed = false,
            Accesibilidad = Accesibilidad.Public,
            Parametros = [],
            VariablesLocales = [],
            MetodosInvocados = []
        };
        _data = new List<Metodo>
        {
            _metodo
        }.AsQueryable();
        _mockSet = new Mock<DbSet<Metodo>>();
        _mockSet.As<IQueryable<Metodo>>().Setup(m => m.Provider).Returns(_data.Provider);
        _mockSet.As<IQueryable<Metodo>>().Setup(m => m.Expression).Returns(_data.Expression);
        _mockSet.As<IQueryable<Metodo>>().Setup(m => m.ElementType).Returns(_data.ElementType);
        _mockSet.As<IQueryable<Metodo>>().Setup(m => m.GetEnumerator()).Returns(() => _data.GetEnumerator());
        _metodoManagerContextMock = new Mock<DbContext>();
        _metodoAccesoADatos = new MetodoRepositorio(_metodoManagerContextMock.Object);
        _metodoManagerContextMock.Setup(x => x.Set<Metodo>()).Returns(_mockSet.Object);
    }

    [TestMethod]
    public void AgregarMetodoTest()
    {
        _metodoManagerContextMock.Setup(x => x.SaveChanges()).Returns(1);

        _metodoAccesoADatos.Agregar(_metodo);

        _metodoManagerContextMock.Verify(blogSystemContext => blogSystemContext.SaveChanges(), Times.Once());
    }

    [TestMethod]
    public void ExisteMetodoTest()
    {
        var resultado = _metodoAccesoADatos.Existe(_metodo.Nombre!);

        Assert.IsTrue(resultado);
    }

    [TestMethod]
    public void ObtenerMetodoPorIdTest()
    {
        var resultado = _metodoAccesoADatos.Obtener(_metodo.Id);

        Assert.AreEqual(_metodo.Nombre, resultado.Nombre);
    }

    [TestMethod]
    public void ObtenerTodosMetodosTest()
    {
        List<Metodo> resultado = _metodoAccesoADatos.ObtenerTodos();

        Assert.IsTrue(resultado.Contains(_metodo));
        Assert.AreEqual(resultado.Count, 1);
    }

    [TestMethod]
    public void EliminarMetodoTest()
    {
        _metodoManagerContextMock.Setup(x => x.SaveChanges()).Returns(1);

        _metodoAccesoADatos.Eliminar(_metodo);

        _metodoManagerContextMock.Verify(blogSystemContext => blogSystemContext.SaveChanges(), Times.Once());
    }

    [TestMethod]
    public void ObtenerMetodosInvocadosTest()
    {
        var metodoInvocado1 = new Metodo { Id = Guid.NewGuid(), Nombre = "Metodo1" };
        var metodoInvocado2 = new Metodo { Id = Guid.NewGuid(), Nombre = "Metodo2" };

        _metodo.MetodosInvocados = new List<Metodo> { metodoInvocado1, metodoInvocado2 };

        var metodosInvocados = _metodoAccesoADatos.ObtenerMetodosInvocados(_metodo.Id);

        Assert.AreEqual(2, metodosInvocados.Count);
        Assert.IsTrue(metodosInvocados.Any(m => m.Nombre == "Metodo1"));
    }

    [TestMethod]
    public void ActualizarMetodoTest()
    {
        _metodo.Nombre = "NuevoNombre";
        _metodoManagerContextMock!.Setup(x => x.SaveChanges()).Returns(1);
        _mockSet!.Setup(x => x.Update(_metodo));

        _metodoAccesoADatos!.Actualizar(_metodo);

        _mockSet.Verify(x => x.Update(_metodo), Times.Once());
        _metodoManagerContextMock.Verify(x => x.SaveChanges(), Times.Once());
    }
}
