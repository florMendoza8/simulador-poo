using AccesoADatos.InterfacesRepositorios;
using Dominio;
using Dominio.Excepciones;
using Moq;
using Servicios;

namespace ServiciosTest;

[TestClass]
public class ParametroServicioTest
{
    private Mock<IParametroRepositorio>? _parametroRepositorioMock;
    private ParametroServicio? _parametroServicio;
    private Guid _id;
    private Parametro _parametro = null;

    [TestInitialize]
    public void Inicializar()
    {
        _parametroRepositorioMock = new Mock<IParametroRepositorio>(MockBehavior.Strict);
        _parametroServicio = new ParametroServicio(_parametroRepositorioMock.Object);
        _id = Guid.NewGuid();
        _parametro = new Parametro
        {
            Id = _id,
            Nombre = "Nombre",
            TipoDato = new TipoDato()
            {
                Id = Guid.NewGuid(),
                Nombre = "int",
            }
        };
    }

    [TestMethod]
    public void AgregarParametro_CuandoNoExiste_DebeLlamarAlRepositorioYDevolverElParametro()
    {
        _parametroRepositorioMock?
            .Setup(r => r.Agregar(_parametro));

        var resultado = _parametroServicio?.Agregar(_parametro);

        _parametroRepositorioMock?.Verify(r => r.Agregar(_parametro), Times.Once);
        Assert.AreEqual(_parametro, resultado);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void AgregarParametro_CuandoNombreEsNulo_DebeLanzarExcepcion()
    {
        var parametro = new Parametro
        {
            Id = Guid.NewGuid(),
            Nombre = null,
            TipoDato = new TipoDato()
        };

        _parametroServicio!.Agregar(parametro);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void AgregarParametro_CuandoTipoDatoEsNulo_DebeLanzarExcepcion()
    {
        var parametro = new Parametro
        {
            Id = Guid.NewGuid(),
            Nombre = "Nombre",
            TipoDato = null
        };

        _parametroServicio!.Agregar(parametro);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void AgregarParametro_CuandoEsNulo_DebeLanzarExcepcion()
    {
        _parametroServicio!.Agregar(null);
    }

    [TestMethod]
    public void ObtenerParametroPorId_Valido_DebeDevolverlo()
    {
        _parametroRepositorioMock!
            .Setup(repo => repo.Obtener(_parametro.Id))
            .Returns(_parametro);

        var resultado = _parametroServicio!.ObtenerParametroPorId(_parametro.Id);

        Assert.AreEqual(_parametro.Nombre, resultado.Nombre);
        _parametroRepositorioMock.Verify(r => r.Obtener(_parametro.Id), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNoEncontradaExcepcion))]
    public void ObtenerParametroPorId_ParametroNulo_DebeLanzarExcepcion()
    {
        var id = Guid.NewGuid();
        _parametroRepositorioMock!
            .Setup(repo => repo.Obtener(id))
            .Returns<Parametro>(null);

        _parametroServicio!.ObtenerParametroPorId(id);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void ObtenerParametroPorId_IdVacio_DebeLanzarExcepcion()
    {
        _parametroServicio!.ObtenerParametroPorId(Guid.Empty);
    }

    [TestMethod]
    public void ObtenerTodosParametros_CuandoHayParametros_DevuelveCantidadCorrecta()
    {
        var lista = new List<Parametro> { _parametro };
        _parametroRepositorioMock!
            .Setup(r => r.ObtenerTodos())
            .Returns(lista);

        var resultado = _parametroServicio!.ObtenerTodosParametros();

        Assert.AreEqual(1, resultado.Count);
    }

    [TestMethod]
    public void ObtenerTodosParametros_CuandoHayParametros_ContieneParametrosEsperado()
    {
        var lista = new List<Parametro> { _parametro };
        _parametroRepositorioMock!
            .Setup(r => r.ObtenerTodos())
            .Returns(lista);

        var resultado = _parametroServicio!.ObtenerTodosParametros();

        Assert.IsTrue(resultado.Contains(_parametro));
    }

    [TestMethod]
    public void ObtenerTodosParametros_CuandoNoHayParametros_DevuelveListaVacia()
    {
        var lista = new List<Parametro>();
        _parametroRepositorioMock!
            .Setup(r => r.ObtenerTodos())
            .Returns(lista);

        var resultado = _parametroServicio!.ObtenerTodosParametros();

        Assert.AreEqual(0, resultado.Count);
    }

    [TestMethod]
    public void ObtenerTodosParametros_LlamaAlRepositorioUnaVez()
    {
        _parametroRepositorioMock!
            .Setup(r => r.ObtenerTodos())
            .Returns(new List<Parametro>());

        _parametroServicio!.ObtenerTodosParametros();

        _parametroRepositorioMock.Verify(r => r.ObtenerTodos(), Times.Once);
    }
}
