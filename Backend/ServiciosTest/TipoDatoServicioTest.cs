using AccesoADatos.InterfacesRepositorios;
using Dominio;
using Dominio.Excepciones;
using Moq;
using Servicios;

namespace ServiciosTest;

[TestClass]
public class TipoDatoServicioTest
{
    private Mock<ITipoDatoRepositorio>? _tipoDatoRepositorioMock;
    private TipoDatoServicio? _tipoDatoServicio;
    private TipoDato _tipoDato = null;
    private Guid _id;

    [TestInitialize]
    public void Inicializar()
    {
        _tipoDatoRepositorioMock = new Mock<ITipoDatoRepositorio>(MockBehavior.Strict);
        _tipoDatoServicio = new TipoDatoServicio(_tipoDatoRepositorioMock.Object);
        _id = Guid.NewGuid();
        _tipoDato = new TipoDato
        {
            Id = _id,
            Nombre = "Nombre"
        };
    }

    [TestMethod]
    public void Agregar_CuandoTipoDatoNoExiste_DebeAgregarTipoDato()
    {
        _tipoDatoRepositorioMock?
            .Setup(repo => repo.Existe(_tipoDato.Nombre))
            .Returns(false);
        _tipoDatoRepositorioMock?.Setup(repo => repo.Agregar(It.IsAny<TipoDato>()));

        var resultado = _tipoDatoServicio?.CrearTipoDato(_tipoDato);

        _tipoDatoRepositorioMock?.VerifyAll();
        Assert.AreEqual(_tipoDato.Nombre, resultado?.Nombre);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadYaExisteExcepcion))]
    public void Agregar_CuandoTipoDatoYaExiste_DebeLanzarExcepcion()
    {
        _tipoDatoRepositorioMock?
            .Setup(repo => repo.Existe(_tipoDato.Nombre))
            .Returns(true);

        _tipoDatoServicio?.CrearTipoDato(_tipoDato);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void Agregar_CuandoTipoDatoEsNull_DebeLanzarExcepcion()
    {
        _tipoDatoServicio?.CrearTipoDato(null!);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void Agregar_CuandoNombreEsVacio_LanzaExcepcion()
    {
        var tipoDato = new TipoDato(string.Empty, null);
        _tipoDatoServicio!.CrearTipoDato(tipoDato);
    }

    [TestMethod]
    public void Existe_CuandoTipoDatoExiste_DebeDevolverTrue()
    {
        _tipoDatoRepositorioMock?
            .Setup(repo => repo.Existe(_tipoDato.Nombre))
            .Returns(true);

        var resultado = _tipoDatoServicio?.ExisteTipoDato(_tipoDato.Nombre!);

        Assert.IsTrue(resultado);
    }

    [TestMethod]
    public void Existe_CuandoTipoDatoNoExiste_DebeDevolverFalse()
    {
        _tipoDatoRepositorioMock?
            .Setup(repo => repo.Existe(_tipoDato.Nombre))
            .Returns(false);

        var resultado = _tipoDatoServicio?.ExisteTipoDato(_tipoDato.Nombre!);

        Assert.IsFalse(resultado);
    }

    [TestMethod]
    public void Obtener_CuandoTipoDatoExiste_DebeDevolverlo()
    {
        _tipoDatoRepositorioMock?.Setup(repo => repo.Obtener(_id))
            .Returns(_tipoDato);

        var resultado = _tipoDatoServicio?.ObtenerTipoDatoPoId(_id);

        Assert.IsTrue(_tipoDato.Id == resultado!.Id && _tipoDato.Nombre == resultado.Nombre);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNoEncontradaExcepcion))]
    public void Obtener_CuandoTipoDatoNoExiste_DebeLanzarExcepcion()
    {
        _tipoDatoRepositorioMock!
            .Setup(repo => repo.Obtener(_id))
            .Returns(((TipoDato?)null)!);

        _tipoDatoServicio!.ObtenerTipoDatoPoId(_id);
    }

    [TestMethod]
    public void ObtenerTodos_CuandoHayDatos_DebeRetornarListaDeTipoDatos()
    {
        var listaTipoDatos = new List<TipoDato>
        {
            new TipoDato { Id = Guid.NewGuid(), Nombre = "Dato 1" },
            new TipoDato { Id = Guid.NewGuid(), Nombre = "Dato 2" }
        };

        _tipoDatoRepositorioMock!
            .Setup(repo => repo.ObtenerTodos())
            .Returns(listaTipoDatos);

        var resultado = _tipoDatoServicio!.ObtenerTodosLosTipoDatos();

        Assert.IsTrue(
            resultado.Count == 2 &&
            resultado[0].Nombre == "Dato 1" &&
            resultado[1].Nombre == "Dato 2");
    }

    [TestMethod]
    public void ObtenerTodos_CuandoNoHayDatos_DebeRetornarListaVacia()
    {
        var listaVacia = new List<TipoDato>();

        _tipoDatoRepositorioMock!
            .Setup(repo => repo.ObtenerTodos())
            .Returns(listaVacia);

        var resultado = _tipoDatoServicio!.ObtenerTodosLosTipoDatos();

        Assert.AreEqual(0, resultado.Count);
    }

    [TestMethod]
    public void AgregarPadreExistenteTest()
    {
        var nombre = "Float";
        var padreId = Guid.NewGuid();
        var hijoId = Guid.NewGuid();
        var padre = new TipoDato("Número", null) { Id = padreId };
        var hijo = new TipoDato(nombre, padre) { Id = hijoId };

        _tipoDatoRepositorioMock?.Setup(r => r.Existe(hijo.Nombre))
            .Returns(false);
        _tipoDatoRepositorioMock?.Setup(r => r.Agregar(It.Is<TipoDato>(t => t == hijo)));
        _tipoDatoRepositorioMock?.Setup(r => r.Obtener(hijoId))
            .Returns(hijo);

        _tipoDatoRepositorioMock?.Setup(r => r.Obtener(padreId)).Returns(padre);
        _tipoDatoRepositorioMock?.Setup(r =>
            r.Agregar(It.Is<TipoDato>(t => t.Nombre == nombre && t.TipoPadre == padre)));
        var result = _tipoDatoServicio?.CrearTipoDato(hijo);
        Assert.AreEqual(padre, result?.TipoPadre);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNoEncontradaExcepcion))]
    public void AsignarPadre_TipoInexistenteTest()
    {
        var tipoId = Guid.NewGuid();
        var padreId = Guid.NewGuid();
        _tipoDatoRepositorioMock?.Setup(r => r.Obtener(tipoId)).Returns((TipoDato?)null);
        _tipoDatoServicio?.AsignarPadre(tipoId, padreId);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNoEncontradaExcepcion))]
    public void AsignarPadre_PadreInexistenteTest()
    {
        var tipoId = Guid.NewGuid();
        var padreId = Guid.NewGuid();
        var tipo = new TipoDato("A", null) { Id = tipoId };

        _tipoDatoRepositorioMock?.Setup(r => r.Obtener(tipoId)).Returns(tipo);
        _tipoDatoRepositorioMock?.Setup(r => r.Obtener(padreId)).Returns((TipoDato?)null);
        _tipoDatoServicio?.AsignarPadre(tipoId, padreId);
    }

    [TestMethod]
    public void AsignarPadreValidoTest()
    {
        var tipoId = Guid.NewGuid();
        var padreId = Guid.NewGuid();
        var tipo = new TipoDato("Hijo", null) { Id = tipoId };
        var padre = new TipoDato("Padre", null) { Id = padreId };

        _tipoDatoRepositorioMock?.Setup(r => r.Obtener(tipoId)).Returns(tipo);
        _tipoDatoRepositorioMock?.Setup(r => r.Obtener(padreId)).Returns(padre);
        _tipoDatoServicio?.AsignarPadre(tipoId, padreId);
        Assert.AreEqual(padre, tipo.TipoPadre);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNoEncontradaExcepcion))]
    public void EsCompatible_ReferenciaNoExisteTest()
    {
        var referencia = Guid.NewGuid();
        var instancia = Guid.NewGuid();
        _tipoDatoRepositorioMock?.Setup(r => r.Obtener(referencia)).Returns((TipoDato?)null);
        _tipoDatoServicio?.EsCompatible(referencia, instancia);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNoEncontradaExcepcion))]
    public void EsCompatible_InstanciaNoExisteTest()
    {
        var referencia = Guid.NewGuid();
        var instancia = Guid.NewGuid();
        var tipoReferencia = new TipoDato { Id = referencia, Nombre = "A", TipoPadre = null };

        _tipoDatoRepositorioMock?.Setup(r => r.Obtener(referencia)).Returns(tipoReferencia);
        _tipoDatoRepositorioMock?.Setup(r => r.Obtener(instancia)).Returns((TipoDato?)null);
        _tipoDatoServicio?.EsCompatible(referencia, instancia);
    }

    [TestMethod]
    public void EsCompatible_CasoCorrectoTest()
    {
        var referencia = Guid.NewGuid();
        var instancia = Guid.NewGuid();
        var padre = new TipoDato { Id = referencia, Nombre = "Padre", TipoPadre = null };
        var hijo = new TipoDato { Id = instancia, Nombre = "Hijo", TipoPadre = padre };

        _tipoDatoRepositorioMock?.Setup(r => r.Obtener(referencia)).Returns(padre);
        _tipoDatoRepositorioMock?.Setup(r => r.Obtener(instancia)).Returns(hijo);

        var respuesta = _tipoDatoServicio?.EsCompatible(referencia, instancia);
        Assert.IsTrue(respuesta);
    }

    [TestMethod]
    public void EsCompatible_CasoNoCorrectoTest()
    {
        var referencia = Guid.NewGuid();
        var instancia = Guid.NewGuid();
        var padre = new TipoDato { Id = referencia, Nombre = "Padre", TipoPadre = null };
        var hijo = new TipoDato { Id = instancia, Nombre = "Hijo", TipoPadre = null };

        _tipoDatoRepositorioMock?.Setup(r => r.Obtener(referencia)).Returns(padre);
        _tipoDatoRepositorioMock?.Setup(r => r.Obtener(instancia)).Returns(hijo);

        var respuesta = _tipoDatoServicio?.EsCompatible(referencia, instancia);
        Assert.IsFalse(respuesta);
    }

    [TestMethod]
    public void EsCompatible_SubclaseIndirectaTest()
    {
        var idRaiz = Guid.NewGuid();
        var idNivelUno = Guid.NewGuid();
        var idNivelDos = Guid.NewGuid();

        var raiz = new TipoDato { Id = idRaiz, Nombre = "raiz", TipoPadre = null };
        var nivelUno = new TipoDato { Id = idNivelUno, Nombre = "Uno", TipoPadre = raiz };
        var nivelDos = new TipoDato { Id = idNivelDos, Nombre = "Dos", TipoPadre = nivelUno };

        _tipoDatoRepositorioMock?.Setup(r => r.Obtener(idRaiz)).Returns(raiz);
        _tipoDatoRepositorioMock?.Setup(r => r.Obtener(idNivelDos)).Returns(nivelDos);

        var respuesta = _tipoDatoServicio?.EsCompatible(idRaiz, idNivelDos);
        Assert.IsTrue(respuesta);
    }

    [TestMethod]
    public void EliminarTipoDato_CuandoEsValido_LlamaAlRepositorio()
    {
        var tipo = new TipoDato { Id = Guid.NewGuid(), Nombre = "Eliminarme" };

        _tipoDatoRepositorioMock!
            .Setup(r => r.EliminarTipoDato(tipo))
            .Verifiable();

        _tipoDatoServicio!.EliminarTipoDato(tipo);

        _tipoDatoRepositorioMock.Verify(r => r.EliminarTipoDato(tipo), Times.Once);
    }
}
