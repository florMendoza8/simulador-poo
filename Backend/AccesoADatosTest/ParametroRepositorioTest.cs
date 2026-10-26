using AccesoADatos.Repositorios;
using Dominio;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AccesoADatosTest;

[TestClass]
public class ParametroRepositorioTest
{
    private Guid _id;
    private IQueryable<Parametro>? _data;
    private Mock<DbSet<Parametro>>? _mockSet;
    private Mock<DbContext>? _parametroManagerContextMock;
    private ParametroRepositorio? _parametroAccesoADatos;
    private Parametro _parametro = null;

    [TestInitialize]
    public void Inicializar()
    {
        _id = Guid.NewGuid();
        _parametro = new Parametro
        {
            Id = _id,
            Nombre = "Nombre",
            TipoDato = new TipoDato()
        };
        _data = new List<Parametro>
        {
            _parametro
        }.AsQueryable();
        _mockSet = new Mock<DbSet<Parametro>>();
        _mockSet.As<IQueryable<Parametro>>().Setup(m => m.Provider).Returns(_data.Provider);
        _mockSet.As<IQueryable<Parametro>>().Setup(m => m.Expression).Returns(_data.Expression);
        _mockSet.As<IQueryable<Parametro>>().Setup(m => m.ElementType).Returns(_data.ElementType);
        _mockSet.As<IQueryable<Parametro>>().Setup(m => m.GetEnumerator()).Returns(() => _data.GetEnumerator());
        _parametroManagerContextMock = new Mock<DbContext>();
        _parametroAccesoADatos = new ParametroRepositorio(_parametroManagerContextMock.Object);
        _parametroManagerContextMock.Setup(x => x.Set<Parametro>()).Returns(_mockSet.Object);
    }

    [TestMethod]
    public void AgregarParametroTest()
    {
        _parametroManagerContextMock.Setup(x => x.SaveChanges()).Returns(1);

        _parametroAccesoADatos.Agregar(_parametro);

        _parametroManagerContextMock.Verify(blogSystemContext => blogSystemContext.SaveChanges(), Times.Once());
    }

    [TestMethod]
    public void ExistsParametroTest()
    {
        var result = _parametroAccesoADatos.Existe(_parametro.Nombre!, _parametro.TipoDato!);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void ObtenerParametroTest()
    {
        var resultado = _parametroAccesoADatos.Obtener(_parametro.Id);

        Assert.AreEqual(_parametro.Nombre, resultado.Nombre);
    }

    [TestMethod]
    public void ObtenerTodosParametrosTest()
    {
        List<Parametro> resultado = _parametroAccesoADatos.ObtenerTodos();

        Assert.IsTrue(resultado.Contains(_parametro));
        Assert.AreEqual(resultado.Count, 1);
    }
}
