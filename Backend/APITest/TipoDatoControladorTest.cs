using API.Controladores;
using API.Modelos.TipoDato;
using Dominio;
using Dominio.Excepciones;
using Moq;
using Servicios.InterfacesServicios;

namespace APITest;

[TestClass]
public class TipoDatoControladorTest
{
    private Mock<ITipoDatoServicio>? _tipoDatoServicioMock;
    private TipoDatoControlador? _controlador;
    private TipoDato? _tipoPadre;
    private Guid _idTipoPadre;

    [TestInitialize]
    public void Inicializar()
    {
        _tipoDatoServicioMock = new Mock<ITipoDatoServicio>(MockBehavior.Strict);
        _controlador = new TipoDatoControlador(_tipoDatoServicioMock.Object);
        _idTipoPadre = Guid.NewGuid();
        _tipoPadre = new TipoDato("Padre", null) { Id = _idTipoPadre };
    }

    [TestMethod]
    public void Crear_SolicitudValida_RetornaCrearTipoDatoRespuesta()
    {
        var solicitud = new CrearTipoDatoSolicitud { Nombre = "DatoPrueba", TipoPadreId = _idTipoPadre };
        var idEsperado = Guid.NewGuid();
        var tipoDatoEsperado = new TipoDato("DatoPrueba", _tipoPadre!) { Id = idEsperado };

        _tipoDatoServicioMock?.Setup(s => s.ObtenerTipoDatoPoId(_tipoPadre!.Id)).Returns(_tipoPadre!);
        _tipoDatoServicioMock?.Setup(s => s.CrearTipoDato(It.IsAny<TipoDato>())).Returns(tipoDatoEsperado);

        var respuesta = _controlador?.Crear(solicitud);

        Assert.IsTrue(respuesta is not null && respuesta.Id == idEsperado);
    }

    [TestMethod]
    public void Crear_ServicioLanzaEntidadNulaExcepcion_PropagaExcepcion()
    {
        var solicitud = new CrearTipoDatoSolicitud { Nombre = "DatoPrueba", TipoPadreId = _idTipoPadre };

        _tipoDatoServicioMock?.Setup(s => s.ObtenerTipoDatoPoId(_tipoPadre!.Id)).Returns(_tipoPadre!);
        _tipoDatoServicioMock?
            .Setup(s => s.CrearTipoDato(It.IsAny<TipoDato>()))
            .Throws(new EntidadNulaExcepcion("El tipo de dato no puede ser nulo."));

        Assert.ThrowsException<EntidadNulaExcepcion>(() => _controlador?.Crear(solicitud));
    }

    [TestMethod]
    public void Crear_ServicioLanzaCampoObligatorioExcepcion_PropagaExcepcion()
    {
        var solicitud = new CrearTipoDatoSolicitud { Nombre = string.Empty, TipoPadreId = _idTipoPadre};

        _tipoDatoServicioMock?.Setup(s => s.ObtenerTipoDatoPoId(_tipoPadre!.Id)).Returns(_tipoPadre!);
        _tipoDatoServicioMock?
            .Setup(s => s.CrearTipoDato(It.IsAny<TipoDato>()))
            .Throws(new EntidadNulaExcepcion("El nombre no puede ser nulo o vacío."));

        Assert.ThrowsException<EntidadNulaExcepcion>(() => _controlador?.Crear(solicitud));
    }

    [TestMethod]
    public void Crear_ServicioLanzaEntidadYaExisteExcepcion_PropagaExcepcion()
    {
        var solicitud = new CrearTipoDatoSolicitud { Nombre = "DatoDuplicado", TipoPadreId = _idTipoPadre};

        _tipoDatoServicioMock?.Setup(s => s.ObtenerTipoDatoPoId(_tipoPadre!.Id)).Returns(_tipoPadre!);
        _tipoDatoServicioMock?
            .Setup(s => s.CrearTipoDato(It.IsAny<TipoDato>()))
            .Throws(new EntidadYaExisteExcepcion($"El tipo de dato con el nombre {solicitud.Nombre} ya existe."));

        var ex = Assert.ThrowsException<EntidadYaExisteExcepcion>(() => _controlador?.Crear(solicitud));
        Assert.IsTrue(ex.Message.Contains(solicitud.Nombre));
    }

    [TestMethod]
    public void Obtener_TipoDatoExistente_RetornaTipoDatoEsperado()
    {
        var idEsperado = Guid.NewGuid();
        var tipoDatoEsperado = new TipoDato("NombrePrueba", null) { Id = idEsperado };

        _tipoDatoServicioMock?
            .Setup(s => s.ObtenerTipoDatoPoId(idEsperado))
            .Returns(tipoDatoEsperado);

        var respuesta = _controlador?.Obtener(idEsperado);

        Assert.IsTrue(respuesta is not null && respuesta.Id == idEsperado);
    }

    [TestMethod]
    public void Obtener_ServicioLanzaEntidadNoEncontradaExcepcion_PropagaExcepcion()
    {
        var idInexistente = Guid.NewGuid();

        _tipoDatoServicioMock?
            .Setup(s => s.ObtenerTipoDatoPoId(idInexistente))
            .Throws(new EntidadNoEncontradaExcepcion($"No se encontró un TipoDato con el ID: {idInexistente}"));

        var ex = Assert.ThrowsException<EntidadNoEncontradaExcepcion>(() => _controlador?.Obtener(idInexistente));
        Assert.IsTrue(ex.Message.Contains(idInexistente.ToString()));
    }

    [TestMethod]
    public void ObtenerTodos_CuandoHayDatos_RetornaListaCorrecta()
    {
        var listaEsperada = new List<TipoDato>
        {
            new TipoDato { Id = Guid.NewGuid(), Nombre = "Dato 1" },
            new TipoDato { Id = Guid.NewGuid(), Nombre = "Dato 2" }
        };

        _tipoDatoServicioMock?
            .Setup(s => s.ObtenerTodosLosTipoDatos())
            .Returns(listaEsperada);

        var respuesta = _controlador?.ObtenerTodos();

        Assert.IsTrue(
            respuesta?.Count == 2 &&
            respuesta[0].Nombre == "Dato 1" &&
            respuesta[1].Nombre == "Dato 2");
    }

    [TestMethod]
    public void ObtenerTodos_CuandoNoHayDatos_RetornaListaVacia()
    {
        _tipoDatoServicioMock?
            .Setup(s => s.ObtenerTodosLosTipoDatos())
            .Returns(new List<TipoDato>());

        var respuesta = _controlador?.ObtenerTodos();

        Assert.IsTrue(respuesta?.Count == 0);
    }
}
