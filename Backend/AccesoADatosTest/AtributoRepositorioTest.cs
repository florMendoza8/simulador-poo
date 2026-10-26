using AccesoADatos.Repositorios;
using Dominio;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AccesoADatosTest;

[TestClass]
public class AtributoRepositorioTest
{
    private Guid _id;
    private IQueryable<Atributo>? _data;
    private Mock<DbSet<Atributo>>? _mockSet;
    private Mock<DbContext>? _atributoManagerContextMock;
    private AtributoRepositorio? _atributoRepositorio;
    private Atributo _atributo = null;

    [TestInitialize]
    public void Inicializar()
    {
        _id = Guid.NewGuid();
        _atributo = new Atributo
        {
            Id = _id,
            Nombre = "Nombre",
            TipoReferencia = new TipoDato(),
            Accesibilidad = Accesibilidad.Private
        };
        _data = new List<Atributo>
        {
            _atributo
        }.AsQueryable();
        _mockSet = new Mock<DbSet<Atributo>>();
        _mockSet.As<IQueryable<Atributo>>().Setup(m => m.Provider).Returns(_data.Provider);
        _mockSet.As<IQueryable<Atributo>>().Setup(m => m.Expression).Returns(_data.Expression);
        _mockSet.As<IQueryable<Atributo>>().Setup(m => m.ElementType).Returns(_data.ElementType);
        _mockSet.As<IQueryable<Atributo>>().Setup(m => m.GetEnumerator()).Returns(() => _data.GetEnumerator());
        _atributoManagerContextMock = new Mock<DbContext>();
        _atributoRepositorio = new AtributoRepositorio(_atributoManagerContextMock.Object);
        _atributoManagerContextMock.Setup(x => x.Set<Atributo>()).Returns(_mockSet.Object);
    }

    [TestMethod]
    public void AgregarAtributoTest()
    {
        _atributoManagerContextMock?.Setup(x => x.SaveChanges()).Returns(1);

        _atributoRepositorio?.Agregar(_atributo);

        _atributoManagerContextMock?.Verify(blogSystemContext => blogSystemContext.SaveChanges(), Times.Once());
    }

    [TestMethod]
    public void ActualizarAtributoTest()
    {
        var atributoActualizado = new Atributo
        {
            Id = _id,
            Nombre = "ClaseModificada",
            TipoReferencia = new TipoDato("nuevoTipoDato", null),
            Accesibilidad = Accesibilidad.Private
        };

        _atributoRepositorio?.Actualizar(atributoActualizado);

        _atributoManagerContextMock?.Verify(c => c.Set<Atributo>().Update(It.Is<Atributo>(f => f.Id == _id)), Times.Once);
        _atributoManagerContextMock?.Verify(context => context.SaveChanges(), Times.Once());
    }

    [TestMethod]
    public void EliminarAtributoTest()
    {
        _atributoManagerContextMock?.Setup(x => x.SaveChanges()).Returns(1);

        _atributoRepositorio?.Eliminar(_atributo);

        _atributoManagerContextMock?.Verify(blogSystemContext => blogSystemContext.SaveChanges(), Times.Once());
    }

    [TestMethod]
    public void ExisteAtributoTest()
    {
        if (_atributo.TipoReferencia != null)
        {
            var resultado = _atributoRepositorio?.Existe(_atributo.Nombre, _atributo.TipoReferencia, _atributo.Accesibilidad);

            Assert.IsTrue(resultado);
        }
    }

    [TestMethod]
    public void ObtenerAtributoTest()
    {
        var resultado = _atributoRepositorio?.Obtener(_id);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(_atributo.Nombre, resultado.Nombre);
        Assert.AreEqual(_atributo.TipoReferencia, resultado.TipoReferencia);
        Assert.AreEqual(_atributo.Accesibilidad, resultado.Accesibilidad);
    }

    [TestMethod]
    public void ObtenerTodosLosAtributosTest()
    {
        List<Atributo> resultado = _atributoRepositorio.ObtenerTodos();

        Assert.AreEqual(1, resultado.Count);
        Assert.IsTrue(resultado.Contains(_atributo));
    }
}
