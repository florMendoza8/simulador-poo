using AccesoADatos.Repositorios;
using Dominio;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AccesoADatosTest;

[TestClass]
public class InterfazRepositorioTest
{
    private Guid _id;
    private IQueryable<Interfaz>? _data;
    private Mock<DbSet<Interfaz>>? _mockSet;
    private Mock<DbContext>? _interfazManagerContextMock;
    private InterfazRepositorio? _interfazRepositorio;
    private Interfaz _interfaz = null;

    [TestInitialize]
    public void Inicializar()
    {
        _id = Guid.NewGuid();
        _interfaz = new Interfaz
        {
            Id = _id,
            Nombre = "Nombre",
            FirmaMetodos = new List<Metodo>()
        };
        _data = new List<Interfaz>
        {
            _interfaz
        }.AsQueryable();
        _mockSet = new Mock<DbSet<Interfaz>>();
        _mockSet.As<IQueryable<Interfaz>>().Setup(m => m.Provider).Returns(_data.Provider);
        _mockSet.As<IQueryable<Interfaz>>().Setup(m => m.Expression).Returns(_data.Expression);
        _mockSet.As<IQueryable<Interfaz>>().Setup(m => m.ElementType).Returns(_data.ElementType);
        _mockSet.As<IQueryable<Interfaz>>().Setup(m => m.GetEnumerator()).Returns(() => _data.GetEnumerator());
        _interfazManagerContextMock = new Mock<DbContext>();
        _interfazRepositorio = new InterfazRepositorio(_interfazManagerContextMock.Object);
        _interfazManagerContextMock.Setup(x => x.Set<Interfaz>()).Returns(_mockSet.Object);
    }

    [TestMethod]
    public void AgregarInterfazTest()
    {
        _interfazManagerContextMock?.Setup(x => x.SaveChanges()).Returns(1);

        _interfazRepositorio?.Agregar(_interfaz);

        _interfazManagerContextMock?.Verify(blogSystemContext => blogSystemContext.SaveChanges(), Times.Once());
    }

    [TestMethod]
    public void EliminarInterfazTest()
    {
        _interfazManagerContextMock?.Setup(x => x.SaveChanges()).Returns(1);

        _interfazRepositorio?.Eliminar(_interfaz);

        _interfazManagerContextMock?.Verify(blogSystemContext => blogSystemContext.SaveChanges(), Times.Once());
    }

    [TestMethod]
    public void ObtenerInterfazTest()
    {
        var resultado = _interfazRepositorio?.Obtener(_id);

        Assert.AreEqual(_interfaz.Nombre, resultado?.Nombre);
    }

    [TestMethod]
    public void ActualizarInterfazTest()
    {
        _interfaz.Nombre = "NuevoNombre";
        _interfazManagerContextMock?.Setup(x => x.SaveChanges()).Returns(1);
        _mockSet?.Setup(x => x.Update(_interfaz));

        _interfazRepositorio.Actualizar(_interfaz);

        _mockSet?.Verify(x => x.Update(_interfaz), Times.Once());
        _interfazManagerContextMock?.Verify(x => x.SaveChanges(), Times.Once());
    }

    [TestMethod]
    public void ObtenerTodasLasInterfacesTest()
    {
        List<Interfaz> resultado = _interfazRepositorio.ObtenerTodos();

        Assert.AreEqual(1, resultado.Count);
        Assert.IsTrue(resultado.Contains(_interfaz));
    }
}
