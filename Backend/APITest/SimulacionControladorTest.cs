using System.Text.Json.Nodes;
using API.Controladores;
using API.Modelos.Simulacion;
using Dominio;
using Moq;
using Reflection;
using Reflection.InterfacesReflection;
using Servicios.InterfacesServicios;

namespace APITest;

[TestClass]
public class SimulacionControladorTest
{
    private Mock<ISimulacionServicio>? _simulacionServicioMock;
    private Mock<IClaseServicio>? _claseServicioMock;
    private Mock<IMetodoServicio>? _metodoServicioMock;
    private Mock<ICargadorDePlugins<ITransformador>>? _cargadorDePluginsMock;
    private SimulacionControlador? _controlador;
    private CrearSimulacionSolicitud? _solicitud;
    private Clase? _tipoReferencia;
    private Clase? _tipoObjeto;
    private Metodo? _metodoReferencia;

    [TestInitialize]
    public void Inicializar()
    {
        _simulacionServicioMock = new Mock<ISimulacionServicio>(MockBehavior.Strict);
        _claseServicioMock = new Mock<IClaseServicio>(MockBehavior.Strict);
        _metodoServicioMock = new Mock<IMetodoServicio>(MockBehavior.Strict);
        _cargadorDePluginsMock = new Mock<ICargadorDePlugins<ITransformador>>(MockBehavior.Strict);
        _controlador = new SimulacionControlador(_simulacionServicioMock.Object, _claseServicioMock.Object,
            _metodoServicioMock.Object, _cargadorDePluginsMock.Object);

        _tipoReferencia = new Clase();
        _tipoObjeto = new Clase
        {
            Nombre = "Clase1"
        };
        _metodoReferencia = new Metodo();

        _solicitud = new CrearSimulacionSolicitud
        {
            TipoReferenciaId = _tipoReferencia.Id,
            TipoObjetoId = _tipoObjeto.Id,
            MetodoReferenciaId = _metodoReferencia.Id,
            Transformador = "HTML"
        };
    }

    [TestMethod]
    public void Crear_CuandoDatosValidosYTransformador_DebeCrearSimulacionTransformada()
    {
        _simulacionServicioMock
            .Setup(s => s.EjecutarSimulacionTransformada(_tipoReferencia!, _tipoObjeto!,
                _metodoReferencia!, _tipoObjeto!.Nombre!, "HTML"))
            .Returns("<html>Resultado</html>");

        _claseServicioMock!
            .Setup(c => c.ObtenerClasePorId(_tipoReferencia!.Id))
            .Returns(_tipoReferencia!);

        _claseServicioMock!
            .Setup(c => c.ObtenerClasePorId(_tipoObjeto!.Id))
            .Returns(_tipoObjeto!);

        _metodoServicioMock!
            .Setup(m => m.ObtenerMetodoPorId(_metodoReferencia!.Id))
            .Returns(_metodoReferencia!);

        var respuesta = _controlador!.Crear(_solicitud!);

        Assert.AreEqual("<html>Resultado</html>", respuesta.Resultado);
    }

    [TestMethod]
    public void Crear_CuandoTransformadorEsNull_DebeCrearSimulacionSinTransformador()
    {
        _solicitud!.Transformador = null;

        _simulacionServicioMock!
            .Setup(s => s.EjecutarSimulacionTransformada(_tipoReferencia!, _tipoObjeto!, _metodoReferencia!, _tipoObjeto!.Nombre!, null))
            .Returns("{ \"LlamadaInicial\": \"this.Metodo()\" }");

        _claseServicioMock!
            .Setup(c => c.ObtenerClasePorId(_tipoReferencia!.Id))
            .Returns(_tipoReferencia!);

        _claseServicioMock!
            .Setup(c => c.ObtenerClasePorId(_tipoObjeto!.Id))
            .Returns(_tipoObjeto!);

        _metodoServicioMock!
            .Setup(m => m.ObtenerMetodoPorId(_metodoReferencia!.Id))
            .Returns(_metodoReferencia!);

        var respuesta = _controlador!.Crear(_solicitud!);

        Assert.IsTrue(respuesta.Resultado.Contains("LlamadaInicial"));
    }

    [TestMethod]
    public void ObtenerTransformadores_DeberiaDevolverListaDeTransformadores()
    {
        var transformadores = new List<string> { "TransformadorHtmlServicio", "TransformadorTextoPlano" };

        _cargadorDePluginsMock!
            .Setup(c => c.ObtenerImplementaciones())
            .Returns(transformadores);

        var resultado = _controlador!.ObtenerTransformadores();

        CollectionAssert.AreEqual(transformadores, resultado);
    }
}
