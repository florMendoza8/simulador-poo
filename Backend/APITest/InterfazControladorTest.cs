using API.Controladores;
using API.Modelos.Interfaz;
using Dominio;
using Dominio.Excepciones;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Servicios.InterfacesServicios;

namespace APITest;

[TestClass]
public class InterfazControladorTest
{
    private Mock<IInterfazServicio>? _interfazServicioMock;
    private InterfazControlador? _controlador;
    private Guid _idEsperado;
    private CrearInterfazSolicitud? _solicitud;
    private Interfaz? _interfazEsperada;

    [TestInitialize]
    public void Inicializar()
    {
        _interfazServicioMock = new Mock<IInterfazServicio>(MockBehavior.Strict);
        _controlador = new InterfazControlador(_interfazServicioMock.Object);
        _solicitud = new CrearInterfazSolicitud { Nombre = "InterfazPrueba" };
        _idEsperado = Guid.NewGuid();
        _interfazEsperada = new Interfaz("InterfazPrueba", new List<Metodo>())
        {
            Id = _idEsperado
        };
    }

    [TestMethod]
    public void Crear_SolicitudValida_RetornaCrearInterfazRespuesta()
    {
        _interfazServicioMock?.Setup(i => i.CrearInterfaz(It.IsAny<Interfaz>())).Returns(_interfazEsperada!);

        var respuesta = _controlador?.Crear(_solicitud!);

        Assert.AreEqual(_idEsperado, respuesta?.Id);
    }

    [TestMethod]
    public void Crear_ServicioLanzaEntidadNulaExcepcion_PropagaExcepcion()
    {
        _interfazServicioMock?.Setup(i => i.CrearInterfaz(It.IsAny<Interfaz>()))
            .Throws(new EntidadNulaExcepcion("La interfaz no puede ser nula."));

        Assert.ThrowsException<EntidadNulaExcepcion>(() => _controlador?.Crear(_solicitud!));
    }

    [TestMethod]
    public void Crear_ServicioLanzaCampoObligatorioExcepcion_PropagaExcepcion()
    {
        var solicitudInvalida = new CrearInterfazSolicitud { Nombre = string.Empty };

        _interfazServicioMock?.Setup(s => s.CrearInterfaz(It.IsAny<Interfaz>()))
            .Throws(new EntidadNulaExcepcion("El nombre no puede ser nulo ni vacío."));

        Assert.ThrowsException<EntidadNulaExcepcion>(() => _controlador?.Crear(solicitudInvalida));
    }

    [TestMethod]
    public void Eliminar_CuandoInterfazExiste_DebeEliminarla()
    {
        _interfazServicioMock?
            .Setup(m => m.ObtenerInterfazPorId(_interfazEsperada!.Id))
            .Returns(_interfazEsperada!);
        _interfazServicioMock?
            .Setup(m => m.EliminarInterfaz(_interfazEsperada!));

        var resultado = _controlador?.Eliminar(_interfazEsperada!.Id);

        Assert.IsInstanceOfType(resultado, typeof(NoContentResult));
        _interfazServicioMock?.VerifyAll();
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void Eliminar_CuandoInterfazNoExiste_DebeLanzarExcepcion()
    {
        var idInterfaz = Guid.NewGuid();
        _interfazServicioMock?
            .Setup(m => m.ObtenerInterfazPorId(idInterfaz))
            .Returns((Interfaz)null);

        _interfazServicioMock?
            .Setup(m => m.EliminarInterfaz(null))
            .Throws(new EntidadNulaExcepcion("La interfaz no puede ser nula"));

        _controlador?.Eliminar(idInterfaz);
    }

    [TestMethod]
    public void Obtener_InterfazExistente_RetornaRespuestaConIdEsperado()
    {
        _interfazServicioMock?.Setup(s => s.ObtenerInterfazPorId(_idEsperado)).Returns(_interfazEsperada!);

        var respuesta = _controlador?.Obtener(_idEsperado);

        Assert.AreEqual(_idEsperado, respuesta.Id);
    }

    [TestMethod]
    public void Obtener_ServicioLanzaEntidadNoEncontradaExcepcion_PropagaExcepcion()
    {
        var idInexistente = Guid.NewGuid();
        _interfazServicioMock?.Setup(i => i.ObtenerInterfazPorId(idInexistente))
            .Throws(new EntidadNoEncontradaExcepcion($"No se encontró una interfaz con el ID: {idInexistente}"));

        var ex = Assert.ThrowsException<EntidadNoEncontradaExcepcion>(() => _controlador?.Obtener(idInexistente));
        Assert.IsTrue(ex.Message.Contains(idInexistente.ToString()));
    }

    [TestMethod]
    public void ObtenerTodasLasInterfaces_CuandoHayDatos_RetornaListaConDatosCorrectos()
    {
        var listaEsperada = new List<Interfaz>
        {
            new Interfaz { Id = Guid.NewGuid(), Nombre = "Interfaz 1", FirmaMetodos = new List<Metodo>() },
            new Interfaz { Id = Guid.NewGuid(), Nombre = "Interfaz 2", FirmaMetodos = new List<Metodo>() }
        };

        _interfazServicioMock?.Setup(s => s.ObtenerTodasLasInterfaces()).Returns(listaEsperada);

        var respuesta = _controlador?.ObtenerTodas();

        Assert.AreEqual(2, respuesta?.Count);
    }

    [TestMethod]
    public void ObtenerTodasLasInterfaces_CuandoNoHayDatos_RetornaListaVacia()
    {
        _interfazServicioMock?.Setup(s => s.ObtenerTodasLasInterfaces()).Returns(new List<Interfaz>());

        var respuesta = _controlador?.ObtenerTodas();

        Assert.AreEqual(0, respuesta?.Count);
    }

    [TestMethod]
    public void AgregarFirmaMetodo_CuandoDatosSonValidos_RetornaRespuestaCorrecta()
    {
        var interfazId = Guid.NewGuid();
        var metodoId = Guid.NewGuid();
        var solicitud = new AgregarFirmaMetodoSolicitud { FirmaMetodoId = metodoId };

        _interfazServicioMock?
            .Setup(s => s.AgregarMetodoAFirmaMetodos(interfazId, metodoId));

        var resultado = _controlador?.AgregarFirmaMetodo(interfazId, solicitud);

        Assert.IsTrue(resultado?.InterfazId == interfazId && resultado.FirmaMetodoId == metodoId);

        _interfazServicioMock?.VerifyAll();
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void AgregarFirmaMetodo_CuandoInterfazIdEsEmpty_LanzaExcepcion()
    {
        var interfazId = Guid.Empty;
        var metodoId = Guid.NewGuid();
        var solicitud = new AgregarFirmaMetodoSolicitud { FirmaMetodoId = metodoId };

        _interfazServicioMock?
            .Setup(s => s.AgregarMetodoAFirmaMetodos(interfazId, metodoId))
            .Throws(new EntidadNulaExcepcion("El Id de la interfaz no puede estar vacío."));

        _controlador?.AgregarFirmaMetodo(interfazId, solicitud);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void AgregarFirmaMetodo_CuandoFirmaMetodoIdEsEmpty_LanzaExcepcion()
    {
        var interfazId = Guid.NewGuid();
        var metodoId = Guid.Empty;
        var solicitud = new AgregarFirmaMetodoSolicitud { FirmaMetodoId = metodoId };

        _interfazServicioMock?
            .Setup(s => s.AgregarMetodoAFirmaMetodos(interfazId, metodoId))
            .Throws(new EntidadNulaExcepcion("El Id del método no puede estar vacío."));

        _controlador?.AgregarFirmaMetodo(interfazId, solicitud);
    }
}
