using API.Controladores;
using API.Modelos.Clase;
using Dominio;
using Dominio.Excepciones;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Servicios.InterfacesServicios;

namespace APITest;

[TestClass]
public class ClaseControladorTest
{
    private Mock<IClaseServicio>? _claseServicio;
    private ClaseControlador? _controlador;
    private CrearClaseSolicitud? _solicitud;
    private Clase? _claseEsperada;
    private Guid _idEsperado;

    [TestInitialize]
    public void Inicializar()
    {
        _claseServicio = new Mock<IClaseServicio>(MockBehavior.Strict);
        _controlador = new ClaseControlador(_claseServicio.Object);

        _solicitud = new CrearClaseSolicitud { Nombre = "Persona", EsAbstract = true, ClaseBaseId = Guid.NewGuid() };
        _idEsperado = Guid.NewGuid();
        _claseEsperada = new Clase { Id = _idEsperado, Nombre = "DatoPrueba", EsAbstract = false, EsSealed = false, ClaseBase = new Clase { Id = Guid.NewGuid() } };
    }

    [TestMethod]
    public void CrearClase_ServicioLanzaEntidadNulaExcepcion_PropagaExcepcionTest()
    {
        _claseServicio?.Setup(s => s.CrearClase(It.IsAny<Clase>())).Throws(new EntidadNulaExcepcion("Clase"));
        Assert.ThrowsException<EntidadNulaExcepcion>(() => _controlador?.Crear(_solicitud));
    }

    [TestMethod]
    public void CrearClase_ServicioLanzaEntidadYaExisteExcepcion_PropagaExcepcionTest()
    {
        _claseServicio?.Setup(s => s.CrearClase(It.IsAny<Clase>())).Throws(new EntidadYaExisteExcepcion("Ya existe"));
        Assert.ThrowsException<EntidadYaExisteExcepcion>(() => _controlador?.Crear(_solicitud));
    }

    [TestMethod]
    public void Crear_SolicitudValida_RetornaCrearClaseRespuestaTest()
    {
        _claseServicio?.Setup(s => s.CrearClase(It.IsAny<Clase>())).Returns(_claseEsperada);
        var respuesta = _controlador?.Crear(_solicitud);
        Assert.AreEqual(_idEsperado, respuesta?.Id);
    }

    [TestMethod]
    public void Obtener_ClaseExiste_RetornaRespuestaTest()
    {
        _claseServicio?.Setup(s => s.ObtenerClasePorId(_idEsperado)).Returns(_claseEsperada);
        var respuesta = _controlador?.Obtener(_idEsperado);
        Assert.AreEqual(_idEsperado, respuesta?.Id);
    }

    [TestMethod]
    public void Obtener_ClaseNoExiste_RetornaRespuestaTest()
    {
        var idInexistente = Guid.NewGuid();
        _claseServicio?.Setup(s => s.ObtenerClasePorId(idInexistente)).Throws(new EntidadYaExisteExcepcion($"No se encontró la clase con Id '{idInexistente}'"));
        var respuesta = Assert.ThrowsException<EntidadYaExisteExcepcion>(() => _controlador?.Obtener(idInexistente));
        Console.WriteLine(">> mensaje real: " + respuesta.Message);
        Assert.IsTrue(respuesta.Message.Contains(idInexistente.ToString()));
    }

    [TestMethod]
    public void ObtenerTodos_CuandoHayDatos_RetornaListaTest()
    {
        var listaEsperada = new List<Clase>
        {
            new Clase
            {
                Id = _idEsperado,
                Nombre = "DatoPrueba",
                EsAbstract = false,
                EsSealed = false,
                ClaseBase = new Clase { Id = Guid.NewGuid() }
            },
            new Clase
            {
                Id = _idEsperado,
                Nombre = "DatoPrueba2",
                EsAbstract = false,
                EsSealed = false,
                ClaseBase = new Clase { Id = Guid.NewGuid() }
            }
        };
        _claseServicio?.Setup(s => s.ObtenerTodasLasClases()).Returns(listaEsperada!);
        var respuesta = _controlador?.ObtenerTodas();
        Assert.AreEqual(2, respuesta?.Count);
    }

    [TestMethod]
    public void ObtenerTodos_CuandoNoHayDatos_RetornaListaVaciaTest()
    {
        _claseServicio?.Setup(s => s.ObtenerTodasLasClases()).Returns([]);
        var respuesta = _controlador?.ObtenerTodas();
        Assert.AreEqual(0, respuesta?.Count);
    }

    [TestMethod]
    public void Eliminar_ClaseNoExisteTest()
    {
        var idclase = Guid.NewGuid();
        _claseServicio?
            .Setup(s => s.ObtenerClasePorId(idclase))
            .Returns((Clase?)null);

        _claseServicio?
            .Setup(s => s.EliminarClase(null))
            .Throws(new EntidadNulaExcepcion("Clase"));

        Assert.ThrowsException<EntidadNulaExcepcion>(
            () => _controlador?.Eliminar(idclase));
    }

    [TestMethod]
    public void Eliminar_ClaseExisteTest()
    {
        _claseServicio?.Setup(s => s.ObtenerClasePorId(_idEsperado)).Returns(_claseEsperada);
        _claseServicio?.Setup(s => s.EliminarClase(_claseEsperada));
        _controlador?.Eliminar(_claseEsperada.Id);
        _claseServicio?.VerifyAll();
    }

    [TestMethod]
    public void Modificar_ModificacionValidaTest()
    {
        var solicitud = new ModificarClaseSolicitud
        {
            Nombre = "X",
            EsAbstract = false,
            EsSealed = false,
            ClaseBaseId = null
        };

        _claseServicio
            .Setup(s => s.ObtenerClasePorId(_idEsperado))
            .Returns(_claseEsperada);

        _claseServicio
            .Setup(s => s.ModificarClase(It.Is<Clase>(c =>
                    c.Id == _idEsperado &&
                    c.Nombre == solicitud.Nombre &&
                    c.EsAbstract == solicitud.EsAbstract &&
                    c.EsSealed == solicitud.EsSealed &&
                    c.ClaseBaseId == solicitud.ClaseBaseId)))
            .Verifiable();

        var result = _controlador?.Modificar(_idEsperado, solicitud) as NoContentResult;

        _claseServicio.Verify();
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void Modificar_IdNoEncontradoTest()
    {
        var nuevoId = Guid.NewGuid();
        var solicitud = new ModificarClaseSolicitud { Nombre = "X", EsAbstract = false, EsSealed = false };
        _claseServicio
            .Setup(s => s.ObtenerClasePorId(nuevoId))
            .Throws(new EntidadNoEncontradaExcepcion("Clase"));
        _claseServicio?
            .Setup(s => s.ModificarClase(It.IsAny<Clase>()))
            .Throws(new EntidadNoEncontradaExcepcion("Clase"));
        Assert.ThrowsException<EntidadNoEncontradaExcepcion>(
            () => _controlador?.Modificar(nuevoId, solicitud));
    }

    [TestMethod]
    public void AgregarMetodoValido_RetornoCorrectoTest()
    {
        var metodoEspereado = Guid.NewGuid();
        var solicitud = new AgregarMetodoSolicitud { MetodoId = metodoEspereado };
        _claseServicio?.Setup(s => s.AgregarMetodo(_idEsperado, metodoEspereado));
        _controlador?.AgregarMetodo(_idEsperado, solicitud);
        _claseServicio?.VerifyAll();
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNoEncontradaExcepcion))]
    public void AgregarMetodo_ClaseNoExiste_LanzaExcepcionTest()
    {
        var metodoEspereado = Guid.NewGuid();
        var solicitud = new AgregarMetodoSolicitud { MetodoId = metodoEspereado };
        _claseServicio?.Setup(s => s.AgregarMetodo(_idEsperado, metodoEspereado)).Throws(new EntidadNoEncontradaExcepcion("Clase no encontrada"));
        _controlador?.AgregarMetodo(_idEsperado, solicitud);
        _claseServicio?.VerifyAll();
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void AgregarMetodo_MetodoInvalido_LanzaExcepcionTest()
    {
        var metodoEspereado = Guid.NewGuid();
        var solicitud = new AgregarMetodoSolicitud { MetodoId = metodoEspereado };
        _claseServicio?.Setup(s => s.AgregarMetodo(_idEsperado, metodoEspereado)).Throws(new OperacionNoPermitidaExcepcion("Método inválido"));
        _controlador?.AgregarMetodo(_idEsperado, solicitud);
        _claseServicio?.VerifyAll();
    }

    [TestMethod]
    public void AgregarAtributoValido_RetornoCorrectoTest()
    {
        var atributoEspereado = Guid.NewGuid();
        var solicitud = new AgregarAtributoSolicitud { AtributoId = atributoEspereado };
        _claseServicio?.Setup(s => s.AgregarAtributo(_idEsperado, atributoEspereado));
        _controlador?.AgregarAtributo(_idEsperado, solicitud);
        _claseServicio?.VerifyAll();
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNoEncontradaExcepcion))]
    public void AgregarAtributo_ClaseNoExiste_LanzaExcepcionTest()
    {
        var atributoEspereado = Guid.NewGuid();
        var solicitud = new AgregarAtributoSolicitud { AtributoId = atributoEspereado };
        _claseServicio?.Setup(s => s.AgregarAtributo(_idEsperado, atributoEspereado)).Throws(new EntidadNoEncontradaExcepcion("Clase no encontrada"));
        _controlador?.AgregarAtributo(_idEsperado, solicitud);
        _claseServicio?.VerifyAll();
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void AgregarAtributo_AtributoInvalido_LanzaExcepcionTest()
    {
        var atributoEspereado = Guid.NewGuid();
        var solicitud = new AgregarAtributoSolicitud { AtributoId = atributoEspereado };
        _claseServicio?.Setup(s => s.AgregarAtributo(_idEsperado, atributoEspereado)).Throws(new OperacionNoPermitidaExcepcion("Clase no encontrada"));
        _controlador?.AgregarAtributo(_idEsperado, solicitud);
        _claseServicio?.VerifyAll();
    }

    [TestMethod]
    public void QuitarAtributo_AtributoExiste_DevuelveNoContent()
    {
        var atributoId = Guid.NewGuid();
        _claseServicio
            .Setup(s => s.ObtenerClasePorId(_idEsperado))
            .Returns(_claseEsperada);

        _claseServicio?
            .Setup(s => s.QuitarAtributo(_idEsperado, atributoId))
            .Verifiable();

        IActionResult resultado = _controlador.QuitarAtributo(_idEsperado, atributoId);

        Assert.IsInstanceOfType(resultado, typeof(NoContentResult));
        _claseServicio?.Verify();
    }

    [TestMethod]
    public void QuitarAtributo_AtributoNoExiste_LanzaEntidadNoEncontradaExcepcion()
    {
        var atributoId = Guid.NewGuid();
        _claseServicio
            .Setup(s => s.ObtenerClasePorId(_idEsperado))
            .Returns(_claseEsperada);

        _claseServicio?
            .Setup(s => s.QuitarAtributo(_idEsperado, atributoId))
            .Throws(new EntidadNoEncontradaExcepcion("Atributo"));

        Assert.ThrowsException<EntidadNoEncontradaExcepcion>(
            () => _controlador?.QuitarAtributo(_idEsperado, atributoId));
    }

    [TestMethod]
    public void QuitarMetodo_MetodoExiste_DevuelveNoContent()
    {
        var metodoId = Guid.NewGuid();
        _claseServicio
            .Setup(s => s.ObtenerClasePorId(_idEsperado))
            .Returns(_claseEsperada);

        _claseServicio
            .Setup(s => s.QuitarMetodo(_idEsperado, metodoId))
            .Verifiable();

        IActionResult resultado = _controlador.QuitarMetodo(_idEsperado, metodoId);

        Assert.IsInstanceOfType(resultado, typeof(NoContentResult));
        _claseServicio.Verify();
    }

    [TestMethod]
    public void QuitarMetodo_MetodoNoExiste_LanzaEntidadNoEncontradaExcepcion()
    {
        var metodoId = Guid.NewGuid();
        _claseServicio
            .Setup(s => s.ObtenerClasePorId(_idEsperado))
            .Returns(_claseEsperada);

        _claseServicio
            .Setup(s => s.QuitarMetodo(_idEsperado, metodoId))
            .Throws(new EntidadNoEncontradaExcepcion("Método"));

        Assert.ThrowsException<EntidadNoEncontradaExcepcion>(
            () => _controlador?.QuitarMetodo(_idEsperado, metodoId));
    }
}
