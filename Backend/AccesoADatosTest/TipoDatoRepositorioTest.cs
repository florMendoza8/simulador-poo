using AccesoADatos.Repositorios;
using Dominio;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AccesoADatosTest;

[TestClass]
public class TipoDatoRepositorioTest
{
    private Guid _id;
    private IQueryable<TipoDato>? _data;
    private Mock<DbSet<TipoDato>>? _mockSet;
    private Mock<DbContext>? _tipoDatoManagerContextMock;
    private TipoDatoRepositorio? _tipoDatoRepositorio;
    private TipoDato _tipoDato = null;

    [TestInitialize]
    public void Inicializar()
    {
        _id = Guid.NewGuid();
        _tipoDato = new TipoDato
        {
            Id = _id,
            Nombre = "Nombre"
        };
        _data = new List<TipoDato>
        {
            _tipoDato
        }.AsQueryable();
        _mockSet = new Mock<DbSet<TipoDato>>();
        _mockSet.As<IQueryable<TipoDato>>().Setup(m => m.Provider).Returns(_data.Provider);
        _mockSet.As<IQueryable<TipoDato>>().Setup(m => m.Expression).Returns(_data.Expression);
        _mockSet.As<IQueryable<TipoDato>>().Setup(m => m.ElementType).Returns(_data.ElementType);
        _mockSet.As<IQueryable<TipoDato>>().Setup(m => m.GetEnumerator()).Returns(() => _data.GetEnumerator());
        _tipoDatoManagerContextMock = new Mock<DbContext>();
        _tipoDatoRepositorio = new TipoDatoRepositorio(_tipoDatoManagerContextMock.Object);
        _tipoDatoManagerContextMock.Setup(x => x.Set<TipoDato>()).Returns(_mockSet.Object);
    }

    [TestMethod]
    public void AgregarTipoDatoTest()
    {
        _tipoDatoManagerContextMock?.Setup(x => x.SaveChanges()).Returns(1);

        _tipoDatoRepositorio?.Agregar(_tipoDato);

        _tipoDatoManagerContextMock?.Verify(blogSystemContext => blogSystemContext.SaveChanges(), Times.Once());
    }

    [TestMethod]
    public void ExisteTipoDatoTest()
    {
        var resultado = _tipoDatoRepositorio?.Existe(_tipoDato.Nombre!);

        Assert.IsTrue(resultado);
    }

    [TestMethod]
    public void ObtenerTipoDatoTest()
    {
        var resultado = _tipoDatoRepositorio?.Obtener(_id);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(_tipoDato.Nombre, resultado.Nombre);
    }

    [TestMethod]
    public void ObtenerTodosLosTipoDatosTest()
    {
        var resultado = _tipoDatoRepositorio?.ObtenerTodos();

        if(resultado != null)
        {
            Assert.AreEqual(1, resultado.Count);
            Assert.IsTrue(resultado.Contains(_tipoDato));
        }
    }

    [TestMethod]
    public void EliminarTipoDatoTest()
    {
        _tipoDatoManagerContextMock!
            .Setup(x => x.SaveChanges())
            .Returns(1)
            .Verifiable();

        _mockSet!
            .Setup(m => m.Remove(_tipoDato))
            .Verifiable();

        _tipoDatoRepositorio!.EliminarTipoDato(_tipoDato);

        _mockSet.Verify(m => m.Remove(_tipoDato), Times.Once);
        _tipoDatoManagerContextMock.Verify(c => c.SaveChanges(), Times.Once);
    }
}
