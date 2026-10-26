using API.Controladores;
using API.Modelos.Metodo;
using Dominio;
using Dominio.Excepciones;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Servicios.InterfacesServicios;

namespace APITest;

[TestClass]
public class MetodoControladorTest
{
    private Mock<IMetodoServicio>? _metodoServicioMock;
    private Mock<ITipoDatoServicio>? _tipoDatoServicioMock;
    private Mock<IClaseServicio>? _claseServicioMock;
    private MetodoControlador? _controlador;
    private CrearMetodoSolicitud? _solicitud;
    private TipoDato? _tipoDato;
    private Clase? _clase;

    [TestInitialize]
    public void Inicializar()
    {
        _metodoServicioMock = new Mock<IMetodoServicio>(MockBehavior.Strict);
        _tipoDatoServicioMock = new Mock<ITipoDatoServicio>(MockBehavior.Strict);
        _claseServicioMock = new Mock<IClaseServicio>(MockBehavior.Strict);
        _controlador = new MetodoControlador(_metodoServicioMock.Object, _tipoDatoServicioMock.Object);
        _tipoDato = new TipoDato("int", null);
        _clase = new Clase();
        _solicitud = new CrearMetodoSolicitud
        {
            Nombre = "CrearNuevoMetodo",
            TipoRetornoId = _tipoDato.Id,
            EsAbstracto = false,
            EsSealed = false,
            EsEstatico = true,
            EsVirtual = false,
            EsOverride = false,
            Accesibilidad = Accesibilidad.Public
        };
    }

    [TestMethod]
    public void Crear_CuandoSolicitudNula_DebeLanzarException()
    {
        _tipoDatoServicioMock?.Setup(s => s.ObtenerTipoDatoPoId(_tipoDato!.Id)).Returns(_tipoDato!);
        _claseServicioMock?
            .Setup(c => c.ObtenerClasePorId(_clase.Id))
            .Returns(_clase!);
        _metodoServicioMock!.Setup(s => s.AgregarMetodo(It.IsAny<Metodo>()))
            .Throws(new EntidadNulaExcepcion("El metodo no puede ser nulo."));

        Assert.ThrowsException<EntidadNulaExcepcion>(() => _controlador?.Crear(_solicitud!));
    }

    [TestMethod]
    public void Crear_CuandoDatosValidos_DebeCrearMetodo()
    {
        var metodoEsperado = new Metodo(_solicitud.Nombre!, _tipoDato!, _solicitud.EsAbstracto,
            _solicitud.EsSealed, _solicitud.Accesibilidad, new Clase(), new List<Parametro>(),
            new List<VariableLocal>(), new List<Metodo>());
        metodoEsperado.EsEstatico = _solicitud.EsEstatico;
        metodoEsperado.EsVirtual = _solicitud.EsVirtual;
        metodoEsperado.EsOverride = _solicitud.EsOverride;

        _metodoServicioMock?
            .Setup(m => m.AgregarMetodo(It.Is<Metodo>(m =>
                m.EsEstatico == _solicitud.EsEstatico &&
                m.EsVirtual == _solicitud.EsVirtual &&
                m.EsOverride == _solicitud.EsOverride)))
            .Returns(metodoEsperado);

        _tipoDatoServicioMock?
            .Setup(t => t.ObtenerTipoDatoPoId(_tipoDato.Id))
            .Returns(_tipoDato!);

        _claseServicioMock?
            .Setup(c => c.ObtenerClasePorId(_clase.Id))
            .Returns(_clase!);

        var respuesta = _controlador?.Crear(_solicitud);

        _metodoServicioMock?.VerifyAll();
        Assert.AreEqual(respuesta?.Id, metodoEsperado.Id);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void Eliminar_CuandoMetodoNoExiste_DeberiaRetornarNotFound()
    {
        var idMetodo = Guid.NewGuid();
        _metodoServicioMock?
            .Setup(m => m.ObtenerMetodoPorId(idMetodo))
            .Returns((Metodo)null);

        _metodoServicioMock?
            .Setup(m => m.EliminarMetodo(null))
            .Throws(new EntidadNulaExcepcion("El metodo no puede ser nulo.."));

        _controlador?.Eliminar(idMetodo);
    }

    [TestMethod]
    public void Eliminar_CuandoMetodoExiste_DebeEliminarlo()
    {
        var metodo = new Metodo
        {
            Nombre = "CrearNuevoMetodo",
            TipoRetorno = new TipoDato("int", null),
            EsAbstract = false,
            EsSealed = false,
            Accesibilidad = Accesibilidad.Public
        };

        _metodoServicioMock?
            .Setup(m => m.ObtenerMetodoPorId(metodo.Id))
            .Returns(metodo);
        _metodoServicioMock?
            .Setup(m => m.EliminarMetodo(metodo));

        var resultado = _controlador?.Eliminar(metodo.Id);

        Assert.IsInstanceOfType(resultado, typeof(NoContentResult));
        _metodoServicioMock?.VerifyAll();
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void AgregarParametro_CuandoMetodoNoExiste_DebeLanzarExcepcion()
    {
        var metodoId = Guid.NewGuid();
        var parametroId = Guid.NewGuid();
        var solicitud = new AgregarParametroMetodoSolicitud{ParametroId = parametroId};

        _metodoServicioMock!
            .Setup(m => m.AgregarParametro(metodoId, parametroId))
            .Throws(new OperacionNoPermitidaExcepcion("El metodo no existe."));

        _controlador?.AgregarParametro(metodoId, solicitud);
    }

    [TestMethod]
    public void AgregarParametro_CuandoValido_DebeRetornarRespuesta()
    {
        var metodoEsperado = new Metodo(_solicitud.Nombre!, _tipoDato!, _solicitud.EsAbstracto,
            _solicitud.EsSealed, _solicitud.Accesibilidad, new Clase(), new List<Parametro>(),
            new List<VariableLocal>(), new List<Metodo>());
        metodoEsperado.EsEstatico = _solicitud.EsEstatico;
        metodoEsperado.EsVirtual = _solicitud.EsVirtual;
        metodoEsperado.EsOverride = _solicitud.EsOverride;

        var parametroId = Guid.NewGuid();
        var solicitud = new AgregarParametroMetodoSolicitud{ParametroId = parametroId};

        _metodoServicioMock?
            .Setup(m => m.AgregarParametro(It.IsAny<Guid>(), parametroId));

        _tipoDatoServicioMock?
            .Setup(t => t.ObtenerTipoDatoPoId(_tipoDato.Id))
            .Returns(_tipoDato!);

        var respuesta = _controlador?.AgregarParametro(metodoEsperado.Id, solicitud);

        Assert.AreEqual(respuesta?.ParametroId, parametroId);
        Assert.AreEqual(respuesta?.MetodoId, metodoEsperado.Id);
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void AgregarVariableLocal_CuandoMetodoNoExiste_DebeLanzarExcepcion()
    {
        var metodoId = Guid.NewGuid();
        var variableLocalId = Guid.NewGuid();
        var solicitud = new AgregarVariableLocalMetodoSolicitud{VariableLocalId = variableLocalId};

        _metodoServicioMock!
            .Setup(m => m.AgregarVariableLocal(metodoId, variableLocalId))
            .Throws(new OperacionNoPermitidaExcepcion("El metodo no existe."));

        _controlador?.AgregarVariableLocal(metodoId, solicitud);
    }

    [TestMethod]
    public void AgregarVariableLocal_CuandoValido_DebeRetornarRespuesta()
    {
        var metodoEsperado = new Metodo(_solicitud.Nombre!, _tipoDato!, _solicitud.EsAbstracto,
            _solicitud.EsSealed, _solicitud.Accesibilidad, new Clase(), new List<Parametro>(),
            new List<VariableLocal>(), new List<Metodo>());
        metodoEsperado.EsEstatico = _solicitud.EsEstatico;
        metodoEsperado.EsVirtual = _solicitud.EsVirtual;
        metodoEsperado.EsOverride = _solicitud.EsOverride;

        var variableLocalId = Guid.NewGuid();
        var solicitud = new AgregarVariableLocalMetodoSolicitud{VariableLocalId = variableLocalId};

        _metodoServicioMock?
            .Setup(m => m.AgregarVariableLocal(It.IsAny<Guid>(), variableLocalId));

        _tipoDatoServicioMock?
            .Setup(t => t.ObtenerTipoDatoPoId(_tipoDato.Id))
            .Returns(_tipoDato!);

        var respuesta = _controlador?.AgregarVariableLocal(metodoEsperado.Id, solicitud);

        Assert.AreEqual(respuesta?.VariableLocalId, variableLocalId);
        Assert.AreEqual(respuesta?.MetodoId, metodoEsperado.Id);
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void AgregarMetodoInvocado_CuandoMetodoOrigenNoExiste_DebeLanzarExcepcion()
    {
        var metodoId = Guid.NewGuid();
        var metodoInvocadoId = Guid.NewGuid();
        var solicitud = new AgregarMetodoInvocadoMetodoSolicitud { MetodoInvocadoId = metodoInvocadoId };

        _metodoServicioMock?
            .Setup(m => m.AgregarMetodoInvocado(metodoId, metodoInvocadoId))
            .Throws(new OperacionNoPermitidaExcepcion("El metodo no existe."));

        _controlador?.AgregarMetodoInvocado(metodoId, solicitud);
    }

    [TestMethod]
    public void AgregarMetodoInvocado_CuandoValido_DebeRetornarRespuesta()
    {
        var metodoEsperado = new Metodo(_solicitud.Nombre!, _tipoDato!, _solicitud.EsAbstracto,
            _solicitud.EsSealed, _solicitud.Accesibilidad, new Clase(), new List<Parametro>(),
            new List<VariableLocal>(), new List<Metodo>());
        metodoEsperado.EsEstatico = _solicitud.EsEstatico;
        metodoEsperado.EsVirtual = _solicitud.EsVirtual;
        metodoEsperado.EsOverride = _solicitud.EsOverride;

        var metodoInvocadoId = Guid.NewGuid();
        var solicitud = new AgregarMetodoInvocadoMetodoSolicitud{MetodoInvocadoId = metodoInvocadoId};

        _metodoServicioMock?
            .Setup(m => m.AgregarMetodoInvocado(It.IsAny<Guid>(), metodoInvocadoId));

        _tipoDatoServicioMock?
            .Setup(t => t.ObtenerTipoDatoPoId(_tipoDato.Id))
            .Returns(_tipoDato!);

        var respuesta = _controlador?.AgregarMetodoInvocado(metodoEsperado.Id, solicitud);

        Assert.AreEqual(respuesta?.MetodoInvocadoId, metodoInvocadoId);
        Assert.AreEqual(respuesta?.MetodoId, metodoEsperado.Id);
    }

    [TestMethod]
    public void ObtenerTodos_CuandoNoHayMetodos_RetornaListaVacia()
    {
        _metodoServicioMock!
            .Setup(s => s.ObtenerTodosMetodos())
            .Returns(new List<Metodo>());

        var resultado = _controlador!.ObtenerTodos();

        Assert.AreEqual(0, resultado.Count);
    }

    [TestMethod]
    public void ObtenerTodos_CuandoHayMetodos_RetornaListaConElementos()
    {
        var metodo = new Metodo("Metodo1", _tipoDato!, false, false, Accesibilidad.Public,
            new Clase(), new List<Parametro>(), new List<VariableLocal>(), new List<Metodo>())
        {
            EsEstatico = true,
            EsVirtual = false,
            EsOverride = false
        };

        _metodoServicioMock!
            .Setup(s => s.ObtenerTodosMetodos())
            .Returns(new List<Metodo> { metodo });

        var resultado = _controlador!.ObtenerTodos();

        Assert.AreEqual(1, resultado.Count);
    }

    [TestMethod]
    public void ObtenerTodos_DevuelveDatosMapeadosCorrectamente()
    {
        var metodo = new Metodo("Metodo1", _tipoDato!, true, true, Accesibilidad.Private,
            new Clase(), new List<Parametro>(), new List<VariableLocal>(), new List<Metodo>())
        {
            EsEstatico = true,
            EsVirtual = true,
            EsOverride = true
        };

        _metodoServicioMock!
            .Setup(s => s.ObtenerTodosMetodos())
            .Returns(new List<Metodo> { metodo });

        var resultado = _controlador!.ObtenerTodos();
        var respuesta = resultado.First();

        Assert.AreEqual(metodo.Id, respuesta.Id);
    }

    [TestMethod]
    public void ObtenerMetodoRespuesta_CuandoNombreEsNull_NoLanzaError()
    {
        var metodo = new Metodo
        {
            Id = Guid.NewGuid(),
            Nombre = null,
            TipoRetorno = _tipoDato,
            EsAbstract = false,
            EsSealed = false,
            EsEstatico = false,
            EsVirtual = false,
            EsOverride = false,
            Accesibilidad = Accesibilidad.Public
        };

        var respuesta = new ObtenerMetodoRespuesta(metodo);

        Assert.IsNull(respuesta.Nombre);
    }

    [TestMethod]
    public void ObtenerMetodoRespuesta_CuandoTipoRetornoEsNull_NoLanzaError()
    {
        var metodo = new Metodo
        {
            Id = Guid.NewGuid(),
            Nombre = "Metodo",
            TipoRetorno = null,
            EsAbstract = false,
            EsSealed = false,
            EsEstatico = false,
            EsVirtual = false,
            EsOverride = false,
            Accesibilidad = Accesibilidad.Public
        };

        var respuesta = new ObtenerMetodoRespuesta(metodo);

        Assert.IsNull(respuesta.TipoRetorno);
    }

    [TestMethod]
    public void ObtenerMetodoRespuesta_CuandoEsSealedEsNull_NoLanzaError()
    {
        var metodo = new Metodo
        {
            Id = Guid.NewGuid(),
            Nombre = "Metodo",
            TipoRetorno = _tipoDato,
            EsAbstract = false,
            EsSealed = null,
            EsEstatico = false,
            EsVirtual = false,
            EsOverride = false,
            Accesibilidad = Accesibilidad.Public
        };

        var respuesta = new ObtenerMetodoRespuesta(metodo);

        Assert.IsFalse(respuesta.EsSealed);
    }

    [TestMethod]
    public void ObtenerMetodoRespuesta_CuandoAccesibilidadEsNull_NoLanzaError()
    {
        var metodo = new Metodo
        {
            Id = Guid.NewGuid(),
            Nombre = "Metodo",
            TipoRetorno = _tipoDato,
            EsAbstract = false,
            EsSealed = false,
            EsEstatico = false,
            EsVirtual = false,
            EsOverride = false,
            Accesibilidad = null
        };

        var respuesta = new ObtenerMetodoRespuesta(metodo);

        Assert.IsNull(respuesta.Accesibilidad);
    }

    [TestMethod]
    public void ObtenerMetodoRespuesta_EsAbstractTrue_SeMapeaCorrectamente()
    {
        var metodo = new Metodo
        {
            Id = Guid.NewGuid(),
            Nombre = "Metodo",
            TipoRetorno = new TipoDato("int", null),
            EsAbstract = true,
            EsSealed = false,
            EsEstatico = false,
            EsVirtual = false,
            EsOverride = false,
            Accesibilidad = Accesibilidad.Public
        };

        var respuesta = new ObtenerMetodoRespuesta(metodo);

        Assert.IsTrue(respuesta.EsAbstract);
    }

    [TestMethod]
    public void ObtenerMetodoRespuesta_EsEstaticoTrue_SeMapeaCorrectamente()
    {
        var metodo = new Metodo
        {
            Id = Guid.NewGuid(),
            Nombre = "Metodo",
            TipoRetorno = new TipoDato("int", null),
            EsAbstract = false,
            EsSealed = false,
            EsEstatico = true,
            EsVirtual = false,
            EsOverride = false,
            Accesibilidad = Accesibilidad.Public
        };

        var respuesta = new ObtenerMetodoRespuesta(metodo);

        Assert.IsTrue(respuesta.EsEstatico);
    }

    [TestMethod]
    public void ObtenerMetodoRespuesta_EsVirtualTrue_SeMapeaCorrectamente()
    {
        var metodo = new Metodo
        {
            Id = Guid.NewGuid(),
            Nombre = "Metodo",
            TipoRetorno = new TipoDato("int", null),
            EsAbstract = false,
            EsSealed = false,
            EsEstatico = false,
            EsVirtual = true,
            EsOverride = false,
            Accesibilidad = Accesibilidad.Public
        };

        var respuesta = new ObtenerMetodoRespuesta(metodo);

        Assert.IsTrue(respuesta.EsVirtual);
    }

    [TestMethod]
    public void ObtenerMetodoRespuesta_EsOverrideTrue_SeMapeaCorrectamente()
    {
        var metodo = new Metodo
        {
            Id = Guid.NewGuid(),
            Nombre = "Metodo",
            TipoRetorno = new TipoDato("int", null),
            EsAbstract = false,
            EsSealed = false,
            EsEstatico = false,
            EsVirtual = false,
            EsOverride = true,
            Accesibilidad = Accesibilidad.Public
        };

        var respuesta = new ObtenerMetodoRespuesta(metodo);

        Assert.IsTrue(respuesta.EsOverride);
    }

    [TestMethod]
    public void Constructor_ConParametros_DeberiaIncluirTodosLosParametros()
    {
        var parametro = new Parametro { Id = Guid.NewGuid(), Nombre = "param1" };
        var metodo = new Metodo(
            "test",
            new TipoDato("int"),
            false,
            false,
            Accesibilidad.Public,
            null!,
            new List<Parametro> { parametro },
            new List<VariableLocal>(),
            new List<Metodo>());

        var respuesta = new ObtenerMetodoRespuesta(metodo);

        Assert.AreEqual("param1", respuesta.Parametros[0].Nombre);
    }

    [TestMethod]
    public void Constructor_ConVariablesLocales_DeberiaIncluirTodasLasVariablesLocales()
    {
        var variable = new VariableLocal { Id = Guid.NewGuid(), Nombre = "var1" };
        var metodo = new Metodo(
            "test",
            new TipoDato("int"),
            false,
            false,
            Accesibilidad.Public,
            null!,
            new List<Parametro>(),
            new List<VariableLocal> { variable },
            new List<Metodo>());

        var respuesta = new ObtenerMetodoRespuesta(metodo);

        Assert.AreEqual("var1", respuesta.VariablesLocales[0].Nombre);
    }

    [TestMethod]
    public void Constructor_ConMetodosInvocados_DeberiaIncluirTodosLosMetodosInvocados()
    {
        var invocado = new Metodo(
            "invocado",
            new TipoDato("void"),
            false,
            false,
            Accesibilidad.Private,
            null!,
            new List<Parametro>(),
            new List<VariableLocal>(),
            new List<Metodo>());

        var metodo = new Metodo(
            "test",
            new TipoDato("int"),
            false,
            false,
            Accesibilidad.Public,
            null!,
            new List<Parametro>(),
            new List<VariableLocal>(),
            new List<Metodo> { invocado });

        var respuesta = new ObtenerMetodoRespuesta(metodo);

        Assert.AreEqual("invocado", respuesta.MetodosInvocados[0].Nombre);
    }

    [TestMethod]
    public void ObtenerMetodo_ConIdValido_RetornaMetodoRespuesta()
    {
        var metodoId = Guid.NewGuid();
        var metodo = new Metodo { Id = metodoId, Nombre = "Test" };

        _metodoServicioMock!
            .Setup(s => s.ObtenerMetodoPorId(metodoId))
            .Returns(metodo);

        var resultado = _controlador!.ObtenerMetodo(metodoId);

        Assert.AreEqual(metodoId, resultado.Value!.Id);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void ObtenerMetodo_ConGuidEmpty_LanzaEntidadNulaExcepcion()
    {
        _metodoServicioMock!
            .Setup(s => s.ObtenerMetodoPorId(Guid.Empty))
            .Throws(new EntidadNulaExcepcion("El id del método no puede estar vacío."));

        _controlador!.ObtenerMetodo(Guid.Empty);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void ObtenerMetodo_CuandoMetodoNoExiste_LanzaEntidadNulaExcepcion()
    {
        var metodoId = Guid.NewGuid();

        _metodoServicioMock
            .Setup(s => s.ObtenerMetodoPorId(metodoId))
            .Throws(new EntidadNulaExcepcion($"No se encontró un método con id: {metodoId}"));

        _controlador.ObtenerMetodo(metodoId);
    }

    [TestMethod]
    public void CrearFirmaMetodo_CuandoSolicitudEsValida_RetornaCrearMetodoRespuesta()
    {
        var solicitud = new CrearFirmaMetodoSolicitud
        {
            Nombre = "MiFirma",
            TipoRetornoId = _tipoDato!.Id
        };

        var metodoEsperado = new Metodo(solicitud.Nombre!, _tipoDato, new List<Parametro>());
        var metodoAgregado = new Metodo(solicitud.Nombre!, _tipoDato, new List<Parametro>()) { Id = metodoEsperado.Id };

        _tipoDatoServicioMock!
            .Setup(s => s.ObtenerTipoDatoPoId(solicitud.TipoRetornoId))
            .Returns(_tipoDato);

        _metodoServicioMock!
            .Setup(s => s.AgregarFirmaMetodo(It.IsAny<Metodo>()))
            .Returns(metodoAgregado);

        var respuesta = _controlador!.CrearFirmaMetodo(solicitud);

        Assert.AreEqual(metodoAgregado.Id, respuesta.Id);
    }

    [TestMethod]
    public void ObtenerTodasFirmaMetodos_CuandoHayFirmas_RetornaListaDeRespuestas()
    {
        var metodo1 = new Metodo("f1", _tipoDato!, new List<Parametro>());
        var metodo2 = new Metodo("f2", _tipoDato!, new List<Parametro>());
        var lista = new List<Metodo> { metodo1, metodo2 };

        _metodoServicioMock!
            .Setup(s => s.ObtenerTodasFirmaMetodos())
            .Returns(lista);

        var resultado = _controlador!.ObtenerTodasFirmaMetodos();

        Assert.AreEqual(2, resultado.Count);
        Assert.AreEqual("f1", resultado[0].Nombre);
    }

    [TestMethod]
    public void ObtenerTodasFirmaMetodos_CuandoNoHayFirmas_RetornaListaVacia()
    {
        _metodoServicioMock!
            .Setup(s => s.ObtenerTodasFirmaMetodos())
            .Returns(new List<Metodo>());

        var resultado = _controlador!.ObtenerTodasFirmaMetodos();

        Assert.IsNotNull(resultado);
        Assert.AreEqual(0, resultado.Count);
    }

    [TestMethod]
    public void ObtenerFirmaMetodo_CuandoExiste_RetornaRespuestaCorrecta()
    {
        var metodoId = Guid.NewGuid();
        var metodo = new Metodo("firma", _tipoDato!, new List<Parametro>()) { Id = metodoId };

        _metodoServicioMock!
            .Setup(s => s.ObtenerMetodoPorId(metodoId))
            .Returns(metodo);

        var resultado = _controlador!.ObtenerFirmaMetodo(metodoId);

        Assert.AreEqual(metodoId, resultado.Value!.Id);
        Assert.AreEqual("firma", resultado.Value.Nombre);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void ObtenerFirmaMetodo_CuandoNoExiste_LanzaExcepcion()
    {
        var id = Guid.NewGuid();

        _metodoServicioMock!
            .Setup(s => s.ObtenerMetodoPorId(id))
            .Throws(new EntidadNulaExcepcion("Método no encontrado"));

        _controlador!.ObtenerFirmaMetodo(id);
    }
}
