using API.Controladores;
using API.Modelos.Atributo;
using Dominio;
using Dominio.Excepciones;
using Moq;
using Servicios.InterfacesServicios;

namespace APITest;

[TestClass]
public class AtributoControladorTest
{
    private Mock<IAtributoServicio>? _atributoServicioMock;
    private Mock<ITipoDatoServicio>? _tipoDatoServicioMock;
    private AtributoControlador? _controlador;
    private Guid _idEsperado;
    private CrearAtributoSolicitud? _solicitud;
    private Atributo? _atributoEsperado;
    private TipoDato? _tipoReferencia;

    [TestInitialize]
    public void Inicializar()
    {
        _atributoServicioMock = new Mock<IAtributoServicio>(MockBehavior.Strict);
        _tipoDatoServicioMock = new Mock<ITipoDatoServicio>(MockBehavior.Strict);
        _controlador = new AtributoControlador(_atributoServicioMock.Object);
        _tipoReferencia = new TipoDato("Animal", null);
        _solicitud = new CrearAtributoSolicitud
        {
            Nombre = "DatoPrueba",
            TipoReferenciaId = _tipoReferencia.Id,
            Accesibilidad = Accesibilidad.Private,
            EsEstatico = false
        };
        _idEsperado = Guid.NewGuid();
        _atributoEsperado = new Atributo("DatoPrueba", _tipoReferencia, null, Accesibilidad.Private, false)
        {
            Id = _idEsperado
        };
    }

    [TestMethod]
    public void Crear_SolicitudValida_RetornaCrearAtributoRespuesta()
    {
        _tipoDatoServicioMock?.Setup(s => s.ObtenerTipoDatoPoId(_tipoReferencia!.Id)).Returns(_tipoReferencia!);
        _atributoServicioMock?.Setup(s => s.CrearAtributo(It.IsAny<Atributo>())).Returns(_atributoEsperado!);

        var respuesta = _controlador?.CrearAtributo(_solicitud!, _tipoDatoServicioMock.Object);

        Assert.AreEqual(_idEsperado, respuesta.Id);
    }

    [TestMethod]
    public void Crear_ServicioLanzaEntidadNulaExcepcion_PropagaExcepcion()
    {
        _tipoDatoServicioMock?.Setup(s => s.ObtenerTipoDatoPoId(_tipoReferencia!.Id)).Returns(_tipoReferencia!);
        _atributoServicioMock?.Setup(s => s.CrearAtributo(It.IsAny<Atributo>()))
            .Throws(new EntidadNulaExcepcion("El atributo no puede ser nulo"));

        Assert.ThrowsException<EntidadNulaExcepcion>(() => _controlador?.CrearAtributo(_solicitud!, _tipoDatoServicioMock.Object));
    }

    [TestMethod]
    public void Crear_ServicioLanzaCampoObligatorioExcepcion_PropagaExcepcion()
    {
        var solicitudInvalida = new CrearAtributoSolicitud
        {
            Nombre = string.Empty,
            TipoReferenciaId = _tipoReferencia.Id,
            Accesibilidad = Accesibilidad.Private,
            EsEstatico = false
        };

        _tipoDatoServicioMock?.Setup(s => s.ObtenerTipoDatoPoId(_tipoReferencia.Id)).Returns(_tipoReferencia);
        _atributoServicioMock?.Setup(s => s.CrearAtributo(It.IsAny<Atributo>()))
            .Throws(new EntidadNulaExcepcion("El nombre no puede ser nulo o vacío"));

        Assert.ThrowsException<EntidadNulaExcepcion>(() => _controlador?.CrearAtributo(solicitudInvalida, _tipoDatoServicioMock.Object));
    }

    [TestMethod]
    public void Modificar_SolicitudValida_ModificaAtributo()
    {
        var solicitud = new ModificarAtributoSolicitud { Nombre = "DatoPrueba", TipoReferenciaId = _tipoReferencia.Id, Accesibilidad = Accesibilidad.Private, EsEstatico = false };

        _atributoServicioMock?.Setup(s => s.ModificarAtributo(It.Is<Atributo>(a =>
            a.Nombre == solicitud.Nombre && a.TipoReferencia == _tipoReferencia && a.Accesibilidad == solicitud.Accesibilidad && a.EsEstatico == solicitud.EsEstatico)));
        _atributoServicioMock?.Setup(s => s.ObtenerAtributoPorId(_idEsperado)).Returns(new Atributo { Id = _idEsperado });
        _tipoDatoServicioMock?.Setup(s => s.ObtenerTipoDatoPoId(_tipoReferencia!.Id)).Returns(_tipoReferencia!);

        _controlador?.ModificarAtributo(_idEsperado, solicitud, _tipoDatoServicioMock.Object);
        _atributoServicioMock?.Verify(s => s.ModificarAtributo(It.Is<Atributo>(a =>
            a.Nombre == solicitud.Nombre &&
            a.Accesibilidad == solicitud.Accesibilidad &&
            a.EsEstatico == solicitud.EsEstatico) ), Times.Once);
    }

    [TestMethod]
    public void Modificar_ServicioLanzaEntidadNulaExcepcion_PropagaExcepcion()
    {
        var solicitud = new ModificarAtributoSolicitud();

        _tipoDatoServicioMock?.Setup(s => s.ObtenerTipoDatoPoId(Guid.Empty)).Returns((TipoDato)null!);
        _atributoServicioMock?.Setup(s => s.ObtenerAtributoPorId(_idEsperado)).Returns(new Atributo { Id = _idEsperado });
        _atributoServicioMock?.Setup(s => s.ModificarAtributo(It.IsAny<Atributo>()))
            .Throws(new EntidadNulaExcepcion("El atributo no puede ser nulo"));

        Assert.ThrowsException<EntidadNulaExcepcion>(() => _controlador?.ModificarAtributo(_idEsperado, solicitud, _tipoDatoServicioMock.Object));
    }

    [TestMethod]
    public void Modificar_ServicioLanzaCampoObligatorioExcepcion_PropagaExcepcion()
    {
        var solicitudInvalida = new ModificarAtributoSolicitud
        {
            Nombre = string.Empty,
            TipoReferenciaId = _tipoReferencia.Id,
            Accesibilidad = Accesibilidad.Private,
            EsEstatico = false
        };

        _atributoServicioMock?.Setup(s => s.ObtenerAtributoPorId(_idEsperado)).Returns(new Atributo { Id = _idEsperado });
        _tipoDatoServicioMock?.Setup(s => s.ObtenerTipoDatoPoId(_tipoReferencia!.Id)).Returns(_tipoReferencia!);
        _atributoServicioMock?.Setup(s => s.ModificarAtributo(It.IsAny<Atributo>()))
            .Throws(new EntidadNulaExcepcion("El nombre no puede ser nulo o vacío"));

        Assert.ThrowsException<EntidadNulaExcepcion>(() => _controlador?.ModificarAtributo(_idEsperado, solicitudInvalida, _tipoDatoServicioMock.Object));
    }

    [TestMethod]
    public void Eliminar_AtributoExistente_EliminaAtributo()
    {
        _atributoServicioMock?.Setup(s => s.ObtenerAtributoPorId(_idEsperado)).Returns(_atributoEsperado!);
        _atributoServicioMock?.Setup(s => s.EliminarAtributo(_atributoEsperado!));
        _controlador?.EliminarAtributo(_atributoEsperado.Id);
        _atributoServicioMock?.VerifyAll();
    }

    [TestMethod]
    public void Obtener_AtributoExistente_RetornaRespuestaConIdEsperado()
    {
        _atributoServicioMock?.Setup(s => s.ObtenerAtributoPorId(_idEsperado)).Returns(_atributoEsperado!);

        var respuesta = _controlador?.ObtenerAtributoPorId(_idEsperado);

        Assert.AreEqual(_idEsperado, respuesta?.Id);
    }

    [TestMethod]
    public void Obtener_ServicioLanzaEntidadNoEncontradaExcepcion_PropagaExcepcion()
    {
        var idInexistente = Guid.NewGuid();
        _atributoServicioMock?.Setup(s => s.ObtenerAtributoPorId(idInexistente))
            .Throws(new EntidadNoEncontradaExcepcion($"No se encontró un atributo con el ID: {idInexistente}"));

        var ex = Assert.ThrowsException<EntidadNoEncontradaExcepcion>(() => _controlador?.ObtenerAtributoPorId(idInexistente));
        Assert.IsTrue(ex.Message.Contains(idInexistente.ToString()));
    }

    [TestMethod]
    public void ObtenerTodos_CuandoHayDatos_RetornaListaConDatosCorrectos()
    {
        var listaEsperada = new List<Atributo>
        {
            new Atributo { Id = Guid.NewGuid(), Nombre = "Atributo 1", TipoReferencia = _tipoReferencia!, Accesibilidad = Accesibilidad.Private, EsEstatico = false},
            new Atributo { Id = Guid.NewGuid(), Nombre = "Atributo 2", TipoReferencia = _tipoReferencia!, Accesibilidad = Accesibilidad.Public, EsEstatico = false}
        };

        _atributoServicioMock?.Setup(s => s.ObtenerTodosLosAtributos()).Returns(listaEsperada);

        var respuesta = _controlador?.ObtenerTodos();

        Assert.AreEqual(2, respuesta?.Count);
    }

    [TestMethod]
    public void ObtenerTodos_CuandoNoHayDatos_RetornaListaVacia()
    {
        _atributoServicioMock?.Setup(s => s.ObtenerTodosLosAtributos()).Returns(new List<Atributo>());

        var respuesta = _controlador?.ObtenerTodos();

        Assert.AreEqual(0, respuesta?.Count);
    }
}
