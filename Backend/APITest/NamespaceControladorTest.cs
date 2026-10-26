using API.Controladores;
using API.Modelos.Namespace;
using Dominio;
using Dominio.Excepciones;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Servicios.InterfacesServicios;

namespace APITest;

[TestClass]
public class NamespaceControladorTest
{
    private Mock<INamespaceServicio>? _namespaceServicio;
    private NamespaceControlador? _controlador;
    private CrearNamespaceSolicitud? _solicitud;

    private Guid _namespaceId;
    private Elemento? _elemento;
    private Guid _elementoId;
    private Clase? _clase;
    private Interfaz? _interfaz;

    [TestInitialize]
    public void Inicializar()
    {
        _namespaceServicio = new Mock<INamespaceServicio>(MockBehavior.Strict);
        _controlador = new NamespaceControlador(_namespaceServicio.Object);

        _solicitud = new CrearNamespaceSolicitud { Nombre = "variable", Padre = null};

        _namespaceId = Guid.NewGuid();
        _elementoId = Guid.NewGuid();
        _elemento = new Clase { Id = _elementoId, Nombre = "TestClase" };
        _clase = new Clase { Id = Guid.NewGuid(), Nombre = "NuevaClase" };
        _interfaz = new Interfaz { Id = Guid.NewGuid(), Nombre = "INuevaInterfaz" };
    }

    [TestMethod]
    public void CrearNamespace_ServicioLanzaEntidadNulaExcepcion_PropagaExcepcionTest()
    {
        _namespaceServicio?.Setup(s => s.Crear(It.IsAny<Namespace>())).Throws(new EntidadNulaExcepcion("Namespace no encontrado"));
        Assert.ThrowsException<EntidadNulaExcepcion>(() => _controlador?.Crear(_solicitud));
    }

    [TestMethod]
    public void CrearNamespace_ServicioLanzaEntidadYaExisteExcepcion_PropagaExcepcionTest()
    {
        _namespaceServicio?.Setup(s => s.Crear(It.IsAny<Namespace>())).Throws(new EntidadYaExisteExcepcion("Ya existe"));
        Assert.ThrowsException<EntidadYaExisteExcepcion>(() => _controlador?.Crear(_solicitud));
    }

    [TestMethod]
    public void CrearNamespace_ServicioDevuelveNamespaceTest()
    {
        var idEsperado = Guid.NewGuid();
        var nsMock = new Namespace("x", null) { Id = idEsperado };
        _namespaceServicio?
            .Setup(s => s.Crear(It.IsAny<Namespace>()))
            .Returns(nsMock);

        var resultado = _controlador?.Crear(_solicitud);

        _namespaceServicio?.VerifyAll();
        Assert.AreEqual(idEsperado, resultado?.Id);
    }

    [TestMethod]
    public void Renombrar_ServicioNoLanzaExcepcionTest()
    {
        var id = Guid.NewGuid();
        var nuevoNombre = "nuevo";
        _namespaceServicio?
            .Setup(s => s.Renombrar(id, nuevoNombre));

        _controlador?.Renombrar(id, nuevoNombre);
    }

    [TestMethod]
    public void Renombrar_ServicioLanzaExcepcion_PropagaTest()
    {
        var id = Guid.NewGuid();
        _namespaceServicio?
            .Setup(s => s.Renombrar(id, It.IsAny<string>()))
            .Throws(new OperacionNoPermitidaExcepcion("Esta operación no esta permitida"));

        Assert.ThrowsException<OperacionNoPermitidaExcepcion>(
            () => _controlador?.Renombrar(id, "x"));
    }

    [TestMethod]
    public void CambiarPadre_ServicioNoLanzaExcepcionTest()
    {
        var id = Guid.NewGuid();
        var nuevoPadreId = Guid.NewGuid();
        _namespaceServicio?
            .Setup(s => s.CambiarPadre(id, nuevoPadreId));

        var resultado = _controlador?.CambiarPadre(id, nuevoPadreId);
        Assert.IsInstanceOfType(resultado, typeof(NoContentResult));
    }

    [TestMethod]
    public void CambiarPadre_ServicioPropagaExcepcionTest()
    {
        var id = Guid.NewGuid();
        var nuevoPadreId = Guid.NewGuid();
        _namespaceServicio?
            .Setup(s => s.CambiarPadre(id, nuevoPadreId))
            .Throws(new OperacionNoPermitidaExcepcion("genera dependencia circular o mala modificacion"));

        Assert.ThrowsException<OperacionNoPermitidaExcepcion>(
            () => _controlador?.CambiarPadre(id, nuevoPadreId));
    }

    [TestMethod]
    public void Eliminar_ServicioNoLanzaTest()
    {
        var id = Guid.NewGuid();
        _namespaceServicio?
            .Setup(s => s.Eliminar(id));

        var resultado = _controlador?.Eliminar(id);

        Assert.IsInstanceOfType(resultado, typeof(NoContentResult));
    }

    [TestMethod]
    public void Eliminar_PropagaExcepcionTest()
    {
        var id = Guid.NewGuid();
        _namespaceServicio?
            .Setup(s => s.Eliminar(id))
            .Throws(new OperacionNoPermitidaExcepcion("tiene hijos"));

        Assert.ThrowsException<OperacionNoPermitidaExcepcion>(
            () => _controlador?.Eliminar(id));
    }

    [TestMethod]
    public void Listar_InvocaServicioYRetornaOkConListaTest()
    {
        var lista = new List<Namespace> { new Namespace("Diseño1", null), new Namespace("Diseño2", null) };
        _namespaceServicio?
            .Setup(s => s.ListarTodos())
            .Returns(lista);

        var respuesta = _controlador?.Listar();
        Assert.AreEqual(2, respuesta?.Count);
    }

    [TestMethod]
    public void Obtener_ServicioDevuelveNamespaceTest()
    {
        var id = Guid.NewGuid();
        var ns = new Namespace("X", null) { Id = id };
        _namespaceServicio?
            .Setup(s => s.Obtener(id))
            .Returns(ns);

        _controlador?.Obtener(id);
    }

    [TestMethod]
    public void Obtener_ServicioLanzaNoEncontrada_PropagaExcepcionTest()
    {
        var id = Guid.NewGuid();
        _namespaceServicio?
            .Setup(s => s.Obtener(id))
            .Throws(new EntidadNoEncontradaExcepcion("no existe"));

        Assert.ThrowsException<EntidadNoEncontradaExcepcion>(() => _controlador?.Obtener(id));
    }

    /*
    [TestMethod]
    public void AgregarElemento_DeberiaInvocarServicioYDevolverNoContent()
    {
        _namespaceServicio?
            .Setup(s => s.AgregarElemento(_namespaceId, _elemento!))
            .Verifiable();

        var result = _controlador.AgregarElemento(_namespaceId, _elemento);

        Assert.IsInstanceOfType(result, typeof(NoContentResult));
        _namespaceServicio?.Verify();
    }

     [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AgregarElemento_ServicioLanzaExcepcion_Propaga()
        {
            _namespaceServicio
                .Setup(s => s.AgregarElemento(_namespaceId, _elemento))
                .Throws(new Exception("Error interno"));

            _controlador.AgregarElemento(_namespaceId, _elemento);
        }

    */

    [TestMethod]
    public void EliminarElemento_DeberiaInvocarServicioYDevolverNoContent()
    {
        _namespaceServicio?
            .Setup(s => s.EliminarElemento(_namespaceId, _elementoId))
            .Verifiable();

        var result = _controlador?.EliminarElemento(_namespaceId, _elementoId);

        Assert.IsInstanceOfType(result, typeof(NoContentResult));
        _namespaceServicio?.Verify();
    }

    [TestMethod]
    public void EliminarElemento_ServicioLanzaExcepcion_Propaga()
    {
        _namespaceServicio?
            .Setup(s => s.EliminarElemento(_namespaceId, _elementoId))
            .Throws(new EntidadNoEncontradaExcepcion("Error interno"));

        var ex = Assert.ThrowsException<EntidadNoEncontradaExcepcion>(
            () => _controlador?.EliminarElemento(_namespaceId, _elementoId));
        Assert.AreEqual("Error interno", ex.Message);
    }

    [TestMethod]
    public void ObtenerHijos_DeberiaRetornarOkConDatos()
    {
        var hijos = new List<Elemento> { _elemento! };
        _namespaceServicio?
            .Setup(s => s.ObtenerHijos(_namespaceId))
            .Returns(hijos)
            .Verifiable();

        var actionResult = _controlador?.ObtenerHijos(_namespaceId);
        var okResult = actionResult as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(hijos, okResult.Value);
        _namespaceServicio?.Verify();
    }

    [TestMethod]
    public void ObtenerHijos_ServicioLanzaExcepcion_Propaga()
    {
        _namespaceServicio?
            .Setup(s => s.ObtenerHijos(_namespaceId))
            .Throws(new EntidadNoEncontradaExcepcion("Error interno"));

        var ex = Assert.ThrowsException<EntidadNoEncontradaExcepcion>(
            () => _controlador?.ObtenerHijos(_namespaceId));
        Assert.AreEqual("Error interno", ex.Message);
    }

    [TestMethod]
    public void AgregarClase_DeberiaInvocarServicioYDevolverNoContent()
    {
        _namespaceServicio?
            .Setup(s => s.AgregarClase(_namespaceId, _clase))
            .Verifiable();

        var result = _controlador?.AgregarClase(_namespaceId, _clase);

        Assert.IsInstanceOfType(result, typeof(NoContentResult));
        _namespaceServicio?.Verify();
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void AgregarClase_ServicioLanzaExcepcion_Propaga()
    {
        _namespaceServicio?
            .Setup(s => s.AgregarClase(_namespaceId, null))
            .Throws(new EntidadNulaExcepcion("La clase no puedo ser vacia"));

        _controlador?.AgregarClase(_namespaceId, null);
    }

    [TestMethod]
    public void AgregarInterfaz_DeberiaInvocarServicioYDevolverNoContent()
    {
        _namespaceServicio?
            .Setup(s => s.AgregarInterfaz(_namespaceId, _interfaz))
            .Verifiable();

        var result = _controlador?.AgregarInterfaz(_namespaceId, _interfaz!);

        Assert.IsInstanceOfType(result, typeof(NoContentResult));
        _namespaceServicio?.Verify();
    }

    [TestMethod]
    public void AgregarInterfaz_ServicioLanzaExcepcion_Propaga()
    {
        _namespaceServicio?
            .Setup(s => s.AgregarInterfaz(_namespaceId, null))
            .Throws(new EntidadNulaExcepcion("La interfaz no puedo ser vacia"));

        var ex = Assert.ThrowsException<EntidadNulaExcepcion>(
            () => _controlador?.AgregarInterfaz(_namespaceId, null));
        Assert.AreEqual("La interfaz no puedo ser vacia", ex.Message);
    }
}
