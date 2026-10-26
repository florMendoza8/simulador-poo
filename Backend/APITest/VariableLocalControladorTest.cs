using API.Controladores;
using API.Modelos.VariableLocal;
using Dominio;
using Dominio.Excepciones;
using Moq;
using Servicios.InterfacesServicios;

namespace APITest;

[TestClass]
public class VariableLocalControladorTest
{
    private Mock<IVariableLocalServicio>? _variableLocalServicioMock;
    private Mock<ITipoDatoServicio>? _tipoDatoServicioMock;
    private VariableLocalControlador? _controlador;
    private Guid _tipoReferenciaId;
    private Guid _tipoInstanciaId;
    private TipoDato? _tipoReferencia;

    [TestInitialize]
    public void Inicializar()
    {
        _variableLocalServicioMock = new Mock<IVariableLocalServicio>(MockBehavior.Strict);
        _tipoDatoServicioMock = new Mock<ITipoDatoServicio>(MockBehavior.Strict);
        _controlador = new VariableLocalControlador(_variableLocalServicioMock.Object);
        _tipoReferenciaId = Guid.NewGuid();
        _tipoInstanciaId = Guid.NewGuid();
        _tipoReferencia = new TipoDato("int", null) { Id = _tipoReferenciaId };
    }

    [TestMethod]
    public void CrearVariableLocal_SolicitudValida_EjecutaAgregar()
    {
        var solicitud = new CrearVariableSolicitud
        {
            Nombre = "VariablePrueba",
            TipoReferenciaId = _tipoReferenciaId,
            Valor = "25"
        };
        var idEsperado = Guid.NewGuid();
        var variableEsperada = new VariableLocal("VariablePrueba", _tipoReferencia!, null, "25") { Id = idEsperado };

        _tipoDatoServicioMock?.Setup(s => s.ObtenerTipoDatoPoId(_tipoReferenciaId)).Returns(_tipoReferencia!);
        _variableLocalServicioMock?.Setup(s => s.Agregar(It.IsAny<VariableLocal>())).Returns(variableEsperada);

        var respuesta = _controlador?.CrearVariable(solicitud, _tipoDatoServicioMock.Object);

        Assert.AreEqual(idEsperado, respuesta?.Id);
    }

    [TestMethod]
    public void CrearVariableLocal_EntidadNula_LanzaExcepcion()
    {
        var solicitud = new CrearVariableSolicitud
        {
            Nombre = "VariablePrueba",
            TipoReferenciaId = _tipoReferenciaId,
            Valor = "25"
        };

        _tipoDatoServicioMock?.Setup(s => s.ObtenerTipoDatoPoId(_tipoReferenciaId)).Returns(_tipoReferencia!);
        _variableLocalServicioMock?.Setup(s => s.Agregar(It.IsAny<VariableLocal>())).Throws(new EntidadNulaExcepcion("La variable local no puede ser nula."));

        Assert.ThrowsException<EntidadNulaExcepcion>(() => _controlador?.CrearVariable(solicitud, _tipoDatoServicioMock.Object));
    }

    [TestMethod]
    public void CrearVariableLocal_NombreVacio_LanzaCampoObligatorioExcepcion()
    {
        var solicitud = new CrearVariableSolicitud
        {
            Nombre = "VariablePrueba",
            TipoReferenciaId = _tipoReferenciaId,
            Valor = "25"
        };

        _tipoDatoServicioMock?.Setup(s => s.ObtenerTipoDatoPoId(_tipoReferenciaId)).Returns(_tipoReferencia!);
        _variableLocalServicioMock?.Setup(s => s.Agregar(It.IsAny<VariableLocal>())).Throws(new EntidadNulaExcepcion("El nombre no puede ser nulo ni vacío."));

        Assert.ThrowsException<EntidadNulaExcepcion>(() => _controlador?.CrearVariable(solicitud, _tipoDatoServicioMock.Object));
    }

    [TestMethod]
    public void CrearVariableLocal_TipoDatoInexistente_LanzaEntidadNoEncontradaExcepcion()
    {
        var solicitud = new CrearVariableSolicitud
        {
            Nombre = "VariablePrueba",
            TipoReferenciaId = _tipoReferenciaId,
            Valor = "25"
        };

        _tipoDatoServicioMock?.Setup(s => s.ObtenerTipoDatoPoId(_tipoReferenciaId)).Throws(new EntidadNoEncontradaExcepcion("No se encontró un TipoDato con el ID: 123"));
        _tipoDatoServicioMock?.Setup(s => s.ObtenerTipoDatoPoId(_tipoInstanciaId)).Throws(new EntidadNoEncontradaExcepcion("No se encontró un TipoDato con el ID: 123"));

        Assert.ThrowsException<EntidadNoEncontradaExcepcion>(() => _controlador?.CrearVariable(solicitud, _tipoDatoServicioMock.Object));
    }

    [TestMethod]
        public void ObtenerTodasLasVariablesLocales_CuandoHayDatos_RetornaListaConDatosCorrectos()
        {
            var listaEsperada = new List<VariableLocal>
            {
                new VariableLocal { Id = Guid.NewGuid(), Nombre = "VariableLocal 1", TipoInstancia = new TipoDato(), TipoReferencia = new TipoDato(), Valor = "valor 1" },
                new VariableLocal { Id = Guid.NewGuid(), Nombre = "VariableLocal 2", TipoInstancia = new TipoDato(), TipoReferencia = new TipoDato(), Valor = "valor 2" }
            };

            _variableLocalServicioMock?.Setup(s => s.ObtenerTodasLasVariablesLocales()).Returns(listaEsperada);

            var respuesta = _controlador?.ObtenerTodas();

            Assert.AreEqual(2, respuesta?.Count);
        }

        [TestMethod]
        public void ObtenerTodasLasVariablesLocales_CuandoNoHayDatos_RetornaListaVacia()
        {
            _variableLocalServicioMock?.Setup(s => s.ObtenerTodasLasVariablesLocales()).Returns(new List<VariableLocal>());

            var respuesta = _controlador?.ObtenerTodas();

            Assert.AreEqual(0, respuesta?.Count);
        }

        [TestMethod]
        public void ObtenerPorId_CuandoExisteTest()
        {
            var id = Guid.NewGuid();
            var variable = new VariableLocal("x", _tipoReferencia!, null, "5") { Id = id };
            _variableLocalServicioMock?
                .Setup(s => s.ObtenerVariableLocalPorId(id))
                .Returns(variable);

            var resultado = _controlador?.ObtenerPorId(id);
            Assert.AreEqual(variable.Id, resultado?.Id);
        }

        [TestMethod]
        public void ObtenerPorId_CuandoNoExisteTest()
        {
            var id = Guid.NewGuid();
            _variableLocalServicioMock?
                .Setup(s => s.ObtenerVariableLocalPorId(id))
                .Throws(new EntidadNoEncontradaExcepcion("no existe"));

            Assert.ThrowsException<EntidadNoEncontradaExcepcion>(() => _controlador?.ObtenerPorId(id));
        }

        [TestMethod]
        public void ObtenerPorId_VacioTest()
        {
            var id = Guid.Empty;
            _variableLocalServicioMock?
                .Setup(s => s.ObtenerVariableLocalPorId(id))
                .Throws(new EntidadNulaExcepcion("El id no puede estar vacío."));

            Assert.ThrowsException<EntidadNulaExcepcion>(() => _controlador?.ObtenerPorId(id));
        }
}
