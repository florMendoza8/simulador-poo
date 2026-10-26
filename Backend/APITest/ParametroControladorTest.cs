using API.Controladores;
using API.Modelos.Parametro;
using Dominio;
using Dominio.Excepciones;
using Moq;
using Servicios.InterfacesServicios;

namespace APITest;

[TestClass]
public class ParametroControladorTest
{
    private Mock<IParametroServicio>? _parametroServicioMock;
    private Mock<ITipoDatoServicio>? _tipoDatoServicioMock;
    private ParametroControlador? _controlador;
    private Guid _idEsperado;
    private CrearParametroSolicitud? _solicitud;
    private Parametro? _parametroEsperado;
    private TipoDato? _tipoDato;

    [TestInitialize]
    public void Inicializar()
    {
        _parametroServicioMock = new Mock<IParametroServicio>(MockBehavior.Strict);
        _tipoDatoServicioMock = new Mock<ITipoDatoServicio>(MockBehavior.Strict);
        _tipoDato = new TipoDato { Id = Guid.NewGuid(), Nombre = "Int" };

        _controlador = new ParametroControlador(_parametroServicioMock.Object, _tipoDatoServicioMock.Object);
        _solicitud = new CrearParametroSolicitud
        {
            Nombre = "DatoPrueba",
            TipoDatoId = _tipoDato.Id
        };
        _idEsperado = Guid.NewGuid();
        _parametroEsperado = new Parametro
        {
            Id = _idEsperado,
            Nombre = "DatoPrueba",
            TipoDato = _tipoDato
        };
    }

    [TestMethod]
    public void CrearParametro_CuandoSolicitudCorrecta_DebeCrearParametro()
    {
        _tipoDatoServicioMock!.Setup(s => s.ObtenerTipoDatoPoId(_tipoDato!.Id)).Returns(_tipoDato!);
        _parametroServicioMock!.Setup(s => s.Agregar(It.IsAny<Parametro>())).Returns(_parametroEsperado!);

        var respuesta = _controlador!.Crear(_solicitud!);

        Assert.AreEqual(_idEsperado, respuesta.Id);
        _tipoDatoServicioMock.Verify(s => s.ObtenerTipoDatoPoId(_tipoDato!.Id), Times.Once);
        _parametroServicioMock.Verify(s => s.Agregar(It.IsAny<Parametro>()), Times.Once);
    }

    [TestMethod]
    public void CrearParametro_CuandoEsNulo_LanzaExcepcion()
    {
        _tipoDatoServicioMock!.Setup(s => s.ObtenerTipoDatoPoId(_tipoDato!.Id)).Returns(_tipoDato!);
        _parametroServicioMock!.Setup(s => s.Agregar(It.IsAny<Parametro>())).Throws(new ArgumentNullException());

        Assert.ThrowsException<ArgumentNullException>(() => _controlador!.Crear(_solicitud!));
    }

    [TestMethod]
    public void CrearParametro_CuandoNombreNulo_LanzaExcepcion()
    {
        var solicitudInvalida = new CrearParametroSolicitud { Nombre = null, TipoDatoId = _tipoDato!.Id };
        _tipoDatoServicioMock!.Setup(s => s.ObtenerTipoDatoPoId(_tipoDato.Id)).Returns(_tipoDato!);
        _parametroServicioMock!.Setup(s => s.Agregar(It.IsAny<Parametro>()))
            .Throws(new ArgumentException("El nombre no puede ser vacío."));

        Assert.ThrowsException<ArgumentException>(() => _controlador!.Crear(solicitudInvalida));
    }

    [TestMethod]
    public void CrearParametro_CuandoNombreVacio_LanzaExcepcion()
    {
        var solicitudInvalida = new CrearParametroSolicitud { Nombre = string.Empty, TipoDatoId = _tipoDato!.Id };
        _tipoDatoServicioMock!.Setup(s => s.ObtenerTipoDatoPoId(_tipoDato.Id)).Returns(_tipoDato!);
        _parametroServicioMock!.Setup(s => s.Agregar(It.IsAny<Parametro>()))
            .Throws(new ArgumentException("El nombre no puede ser vacío."));

        Assert.ThrowsException<ArgumentException>(() => _controlador!.Crear(solicitudInvalida));
    }

    [TestMethod]
    public void CrearParametro_CuandoTipoDatoNoExiste_LanzaExcepcion()
    {
        var solicitudInvalida = new CrearParametroSolicitud { Nombre = "Dato", TipoDatoId = Guid.NewGuid() };
        _tipoDatoServicioMock!.Setup(s => s.ObtenerTipoDatoPoId(It.IsAny<Guid>()))
            .Throws(new EntidadNoEncontradaExcepcion("TipoDato"));

        Assert.ThrowsException<EntidadNoEncontradaExcepcion>(() => _controlador!.Crear(solicitudInvalida));
    }

    [TestMethod]
    public void ObtenerTodos_CuandoNoHayParametros_RetornaListaVacia()
    {
        _parametroServicioMock!
            .Setup(s => s.ObtenerTodosParametros())
            .Returns(new List<Parametro>());

        var resultado = _controlador!.ObtenerTodos();

        Assert.AreEqual(0, resultado.Count);
    }

    [TestMethod]
    public void ObtenerTodos_CuandoHayUnParametro_RetornaListaConUnElemento()
    {
        var parametro = new Parametro
        {
            Id = Guid.NewGuid(),
            Nombre = "Parametro1",
            TipoDato = _tipoDato
        };

        _parametroServicioMock!
            .Setup(s => s.ObtenerTodosParametros())
            .Returns(new List<Parametro> { parametro });

        var resultado = _controlador!.ObtenerTodos();

        Assert.AreEqual(1, resultado.Count);
    }

    [TestMethod]
    public void ObtenerParametroRespuesta_ConDatosValidos_SeConstruyeCorrectamente()
    {
        var parametro = new Parametro
        {
            Id = Guid.NewGuid(),
            Nombre = "Param1",
            TipoDato = _tipoDato
        };

        var respuesta = new ObtenerParametroRespuesta(parametro);

        Assert.AreEqual(parametro.Id, respuesta.Id);
    }

    [TestMethod]
    public void ObtenerParametroRespuesta_CuandoNombreEsNull_NoLanzaExcepcion()
    {
        var parametro = new Parametro
        {
            Id = Guid.NewGuid(),
            Nombre = null,
            TipoDato = _tipoDato
        };

        var respuesta = new ObtenerParametroRespuesta(parametro);

        Assert.AreEqual(parametro.Id, respuesta.Id);
    }

    [TestMethod]
    public void ObtenerParametroRespuesta_CuandoTipoDatoEsNull_NoLanzaExcepcion()
    {
        var parametro = new Parametro
        {
            Id = Guid.NewGuid(),
            Nombre = "Parametro",
            TipoDato = null
        };

        var respuesta = new ObtenerParametroRespuesta(parametro);

        Assert.AreEqual(parametro.Id, respuesta.Id);
    }

    [TestMethod]
    public void ObtenerParametroRespuesta_GetNombre_DevuelveNombreEsperado()
    {
        var parametro = new Parametro
        {
            Id = Guid.NewGuid(),
            Nombre = "ParamVisible",
            TipoDato = _tipoDato
        };

        var respuesta = new ObtenerParametroRespuesta(parametro);

        Assert.AreEqual("ParamVisible", respuesta.Nombre);
    }

    [TestMethod]
    public void ObtenerParametroRespuesta_GetTipoDato_DevuelveTipoDatoEsperado()
    {
        var parametro = new Parametro
        {
            Id = Guid.NewGuid(),
            Nombre = "ParamConTipoDato",
            TipoDato = _tipoDato
        };

        var respuesta = new ObtenerParametroRespuesta(parametro);

        Assert.AreEqual(_tipoDato, respuesta.TipoDato);
    }

    [TestMethod]
    public void ObtenerParametro_ConIdValido_RetornaParametroRespuesta()
    {
        var parametro = new Parametro
        {
            Id = Guid.NewGuid(),
            Nombre = "ParamConTipoDato",
            TipoDato = _tipoDato
        };

        _parametroServicioMock!
            .Setup(s => s.ObtenerParametroPorId(parametro.Id))
            .Returns(parametro);

        var resultado = _controlador!.ObtenerParametro(parametro.Id);

        Assert.AreEqual(parametro.Id, resultado.Value!.Id);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void ObtenerParametro_ConGuidEmpty_LanzaEntidadNulaExcepcion()
    {
        _parametroServicioMock!
            .Setup(s => s.ObtenerParametroPorId(Guid.Empty))
            .Throws(new EntidadNulaExcepcion("El id del parametro no puede estar vacío."));

        _controlador!.ObtenerParametro(Guid.Empty);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void ObtenerParametro_CuandoMetodoNoExiste_LanzaEntidadNulaExcepcion()
    {
        var parametroId = Guid.NewGuid();

        _parametroServicioMock!
            .Setup(s => s.ObtenerParametroPorId(parametroId))
            .Throws(new EntidadNulaExcepcion($"No se encontro un parametro con id: {parametroId}"));

        _controlador.ObtenerParametro(parametroId);
    }
}
