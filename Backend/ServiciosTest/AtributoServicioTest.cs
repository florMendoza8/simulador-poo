using AccesoADatos.InterfacesRepositorios;
using Dominio;
using Dominio.Excepciones;
using Moq;
using Servicios;
using Servicios.InterfacesServicios;

namespace ServiciosTest;

[TestClass]
public class AtributoServicioTest
{
    private Mock<IAtributoRepositorio>? _atributoRepositorioMock;
    private Mock<ITipoDatoServicio>? _tipoDatoServicioMock;
    private AtributoServicio? _atributoServicio;
    private Atributo _atributo = null;
    private Guid _id;
    private TipoDato? _tipoDato;

    [TestInitialize]
    public void Inicializar()
    {
        _tipoDatoServicioMock = new Mock<ITipoDatoServicio>();
        _atributoRepositorioMock = new Mock<IAtributoRepositorio>(MockBehavior.Strict);
        _atributoServicio = new AtributoServicio(_atributoRepositorioMock.Object, _tipoDatoServicioMock.Object);
        _id = Guid.NewGuid();
        _tipoDato = new TipoDato();
        _atributo = new Atributo
        {
            Id = _id,
            Nombre = "Nombre",
            TipoReferencia = _tipoDato,
            TipoInstancia = _tipoDato,
            Accesibilidad = Accesibilidad.Private
        };
    }

    [TestMethod]
    public void Crear_CuandoAtributoNoExiste_DebeAgregarAtributo()
    {
        _tipoDatoServicioMock?
            .Setup(s => s.EsCompatible(_tipoDato.Id, _tipoDato.Id))
            .Returns(true);
        _atributoRepositorioMock?.Setup(repo => repo.Agregar(It.IsAny<Atributo>()));

        var resultado = _atributoServicio?.CrearAtributo(_atributo);

        _atributoRepositorioMock?.VerifyAll();
        Assert.AreEqual(_atributo.Id, resultado?.Id);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void Crear_CuandoAtributoEsNull_LanzaExcepcion()
    {
        _atributoServicio?.CrearAtributo(null);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void Crear_CuandoNombreEsVacio_LanzaExcepcion()
    {
        var atributo = new Atributo(string.Empty, new TipoDato(), new TipoDato(), Accesibilidad.Public, false);
        _atributoServicio?.CrearAtributo(atributo);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void Crear_CuandoTipoDatoEsNull_LanzaExcepcion()
    {
        var atributo = new Atributo("Nombre", null, new TipoDato(), Accesibilidad.Public, false);
        _atributoServicio?.CrearAtributo(atributo);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void Crear_CuandoTipoDatoIdEsEmpty_LanzaExcepcion()
    {
        var atributo = new Atributo("Nombre", new TipoDato { Id = Guid.Empty }, new TipoDato(), Accesibilidad.Public, false);
        _atributoServicio?.CrearAtributo(atributo);
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void Crear_CuandoAccesibilidadInvalida_LanzaExcepcion()
    {
        var atributo = new Atributo
        {
            Nombre = "MiAtributo",
            TipoReferencia = new TipoDato { Id = Guid.NewGuid() },
            TipoInstancia = new TipoDato { Id = Guid.NewGuid() },
            Accesibilidad = (Accesibilidad)999,
            EsEstatico = false
        };

        _atributoServicio?.CrearAtributo(atributo);
    }

    [TestMethod]
    public void Eliminar_CuandoAtributoEsValido_DebeEliminarAtributo()
    {
        _atributoRepositorioMock?
            .Setup(r => r.Eliminar(_atributo));

        _atributoServicio?.EliminarAtributo(_atributo);

        _atributoRepositorioMock?.Verify(r => r.Eliminar(_atributo), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void Eliminar_CuandoInterfazEsNull_LanzaExcepcion()
    {
        _atributoServicio?.EliminarAtributo(null);
    }

    [TestMethod]
    public void Modificar_CuandoAtributoEsValido_DebeActualizar()
    {
        var nuevoTipoDato = new TipoDato();
        var atributoExistente = new Atributo("Existe", nuevoTipoDato, nuevoTipoDato, Accesibilidad.Private, false);
        _tipoDatoServicioMock?
            .Setup(s => s.EsCompatible(nuevoTipoDato.Id, nuevoTipoDato.Id))
            .Returns(true);
        _atributoRepositorioMock?.Setup(repo => repo.Obtener(atributoExistente.Id)).Returns(atributoExistente);
        _atributoRepositorioMock?.Setup(repo => repo.Actualizar(It.IsAny<Atributo>()));

        _atributoServicio?.ModificarAtributo(atributoExistente);

        _atributoRepositorioMock?.Verify(repo => repo.Actualizar(It.IsAny<Atributo>()), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void Modificar_CuandoAtributoEsNull_LanzaExcepcion()
    {
        _atributoServicio?.ModificarAtributo(null);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void Modificar_CuandoNombreEsVacio_LanzaExcepcion()
    {
        var atributo = new Atributo(string.Empty, new TipoDato(), new TipoDato(), Accesibilidad.Public, false);
        _atributoServicio?.ModificarAtributo(atributo);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void Modificar_CuandoTipoDatoEsNull_LanzaExcepcion()
    {
        var atributo = new Atributo("Nombre", null, new TipoDato(), Accesibilidad.Public, false);
        _atributoServicio?.ModificarAtributo(atributo);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void Modificar_CuandoTipoDatoIdEsEmpty_LanzaExcepcion()
    {
        var atributo = new Atributo("Nombre", new TipoDato { Id = Guid.Empty }, new TipoDato(), Accesibilidad.Public, false);
        _atributoServicio?.ModificarAtributo(atributo);
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void Modificar_CuandoAccesibilidadInvalida_LanzaExcepcion()
    {
        var atributo = new Atributo
        {
            Nombre = "MiAtributo",
            TipoReferencia = new TipoDato { Id = Guid.NewGuid() },
            TipoInstancia = new TipoDato { Id = Guid.NewGuid() },
            Accesibilidad = (Accesibilidad)999,
            EsEstatico = false
        };

        _atributoServicio?.ModificarAtributo(atributo);
    }

    [TestMethod]
    public void Existe_CuandoAtributoExiste_DebeDevolverTrue()
    {
        _atributoRepositorioMock?
            .Setup(repo => repo.Existe(_atributo.Nombre, _atributo.TipoReferencia!, _atributo.Accesibilidad))
            .Returns(true);

        var resultado = _atributoServicio?.ExisteAtributo(_atributo.Nombre, _atributo.TipoReferencia!, _atributo.Accesibilidad);

        Assert.IsTrue(resultado);
    }

    [TestMethod]
    public void Existe_CuandoAtributoNoExiste_DebeDevolverFalse()
    {
        _atributoRepositorioMock?
            .Setup(repo => repo.Existe(_atributo.Nombre, _atributo.TipoReferencia!, _atributo.Accesibilidad))
            .Returns(false);

        var resultado = _atributoServicio?.ExisteAtributo(_atributo.Nombre, _atributo.TipoReferencia!, _atributo.Accesibilidad);

        Assert.IsFalse(resultado);
    }

    [TestMethod]
    public void Obtener_CuandoAtributoExiste_DebeDevolverlo()
    {
        _atributoRepositorioMock?.Setup(repo => repo.Obtener(_id))
            .Returns(_atributo);

        var resultado = _atributoServicio?.ObtenerAtributoPorId(_id);

        Assert.AreEqual(_atributo.Id, resultado?.Id);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNoEncontradaExcepcion))]
    public void Obtener_CuandoAtributoNoExiste_DebeLanzarExcepcion()
    {
        _atributoRepositorioMock?.Setup(repo => repo.Obtener(_id))
            .Returns(((Atributo)null));

        _atributoServicio?.ObtenerAtributoPorId(_id);
    }

    [TestMethod]
    public void ObtenerTodos_CuandoHayDatos_DebeRetornarListaDeAtributos()
    {
        var lista = new List<Atributo>
        {
            new Atributo { Id = Guid.NewGuid(), Nombre = "Atributo 1" },
            new Atributo { Id = Guid.NewGuid(), Nombre = "Atributo 2" }
        };

        _atributoRepositorioMock?
            .Setup(repo => repo.ObtenerTodos())
            .Returns(lista);

        var resultado = _atributoServicio?.ObtenerTodosLosAtributos();

        Assert.IsTrue(
            resultado?.Count == 2 &&
            resultado[0].Nombre == "Atributo 1" &&
            resultado[1].Nombre == "Atributo 2");
    }

    [TestMethod]
    public void ObtenerTodos_CuandoNoHayDatos_DebeRetornarListaVacia()
    {
        var listaVacia = new List<Atributo>();

        _atributoRepositorioMock?
            .Setup(repo => repo.ObtenerTodos())
            .Returns(listaVacia);

        var resultado = _atributoServicio?.ObtenerTodosLosAtributos();

        Assert.AreEqual(0, resultado?.Count);
    }
}
