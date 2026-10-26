using AccesoADatos.Repositorios;
using Dominio;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AccesoADatosTest;

[TestClass]
public class VariableLocalRepositorioTest
{
    private Guid _id;
    private IQueryable<VariableLocal>? _variable;
    private Mock<DbSet<VariableLocal>>? _mock;
    private VariableLocal? _variableLocal;
    private VariableLocalRepositorio? _repositorioVariable;
    private Mock<DbContext>? _contexto;

    [TestInitialize]
    public void Inicializar()
    {
        _id = Guid.NewGuid();
        _variableLocal = new VariableLocal { Id = _id, Nombre = "TestNombre", TipoReferencia = new TipoDato() };
        _variable = new List<VariableLocal> { _variableLocal }.AsQueryable();
        _mock = new Mock<DbSet<VariableLocal>>();
        _mock.As<IQueryable<VariableLocal>>().Setup(m => m.Provider).Returns(_variable.Provider);
        _mock.As<IQueryable<VariableLocal>>().Setup(m => m.Expression).Returns(_variable.Expression);
        _mock.As<IQueryable<VariableLocal>>().Setup(m => m.ElementType).Returns(_variable.ElementType);
        _mock.As<IQueryable<VariableLocal>>().Setup(m => m.GetEnumerator()).Returns(() => _variable.GetEnumerator());
        _contexto = new Mock<DbContext>();
        _repositorioVariable = new VariableLocalRepositorio(_contexto.Object);
        _contexto.Setup(x => x.Set<VariableLocal>()).Returns(_mock.Object);
    }

    [TestMethod]
    public void AgregarVariableTest()
    {
        _contexto!.Setup(x => x.SaveChanges()).Returns(1);
        _repositorioVariable!.Agregar(_variableLocal!);
        _contexto.Verify(contectoSistema => contectoSistema.SaveChanges(), Times.Once());
    }

    [TestMethod]
    public void ExisteTest()
    {
        var resultado = _repositorioVariable!.ExisteVariable(_variableLocal!.Nombre!);
        Assert.IsTrue(resultado);
    }

    [TestMethod]
    public void ObtenerVariableLocalTest()
    {
        var resultado = _repositorioVariable.ObtenerPorId(_variableLocal.Id);

        Assert.AreEqual(_variableLocal.Nombre, resultado.Nombre);
    }

    [TestMethod]
    public void ObtenerTodasLasVariablesLocalesTest()
    {
        List<VariableLocal> resultado = _repositorioVariable.ObtenerTodos();

        Assert.AreEqual(1, resultado.Count);
        Assert.IsTrue(resultado.Contains(_variableLocal!));
    }
}
