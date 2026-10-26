using AccesoADatos.InterfacesRepositorios;
using Dominio;
using Dominio.Excepciones;
using Servicios.InterfacesServicios;

namespace Servicios;

public class ClaseServicio : IClaseServicio
{
    private readonly IClaseRepositorio _repositorio;
    private readonly IAtributoServicio _atributoServicio;
    private readonly IMetodoServicio _metodoServicio;
    private readonly ITipoDatoServicio _tipoDatoServicio;

    public ClaseServicio(IClaseRepositorio repositorio, IAtributoServicio atributo, IMetodoServicio metodo,
        ITipoDatoServicio tipoDato)
    {
        _repositorio = repositorio;
        _atributoServicio = atributo;
        _metodoServicio = metodo;
        _tipoDatoServicio = tipoDato;
    }

    public Clase? CrearClase(Clase? clase)
    {
        ValidarClaseNoNula(clase);
        ValidarNombreClase(clase.Nombre);
        ValidarSiClaseExiste(clase);

        if (clase.ClaseBaseId.HasValue)
        {
            clase.ClaseBase = _repositorio.ObtenerClase(clase.ClaseBaseId.Value);
            if (clase.ClaseBase == null)
            {
                throw new EntidadNoEncontradaExcepcion("La clase base no fue encontrada.");
            }
        }

        ValidarClaseBaseNoSealed(clase);
        InicializarListasDeClase(clase);

        if (clase.ClaseBase != null)
        {
            ValidarNoHerenciaCircular(clase);
        }

        _repositorio.Agregar(clase);
        RegistrarTipoDatoParaClase(clase);
        return clase;
    }

    public void ModificarClase(Clase? clase)
    {
        ValidarClaseNoNula(clase);
        ValidarClaseExiste(clase?.Id);
        if (clase.ClaseBase != null)
        {
            ValidarClaseBase(clase.ClaseBase);
        }

        ValidarNoHerenciaCircular(clase);
        ValidarNombreClase(clase?.Nombre);

        ValidarClaseBaseNoSealed(clase);

        _repositorio.Actualizar(clase);
    }

    public void EliminarClase(Clase? clase)
    {
        ValidarClaseNoNula(clase);
        ValidarClaseExiste(clase?.Id);
        ValidarNoTieneClasesDerivadas(clase.Id);
        var tipoDato = _tipoDatoServicio.ObtenerTipoDatoPoId(clase.Id);
        _tipoDatoServicio.EliminarTipoDato(tipoDato);
        _repositorio.EliminarClase(clase);
    }

    public Clase? ObtenerClasePorId(Guid? id)
    {
        var clase = _repositorio.ObtenerClase(id);
        ValidarClaseExiste(id);
        return clase;
    }

    public List<Clase?> ObtenerTodasLasClases()
    {
        return _repositorio.ObtenerTodasLasClases();
    }

    public void AgregarAtributo(Guid idClase, Guid idAtributo)
    {
        ValidarClaseExiste(idClase);

        var clase = _repositorio.ObtenerClase(idClase);

        BuscarAtributo(idAtributo);
        var atributo = _atributoServicio.ObtenerAtributoPorId(idAtributo);
        AtributoEstaticoYaExiste(clase, atributo);
        AtributoYaExiste(clase, atributo);

        ValidoSiHeredaAtributo(clase, atributo);

        clase?.Atributos.Add(atributo);
        _repositorio.Actualizar(clase);
    }

    public void AgregarMetodo(Guid idClase, Guid idMetodo)
    {
        var clase = _repositorio.ObtenerClase(idClase);

        ValidarClaseExiste(idClase);

        var metodo = _metodoServicio.ObtenerMetodoPorId(idMetodo);
        BuscarMetodo(metodo);

        ValidarSiMetodoEstaticoYAbstracto(metodo);
        ValidarSiMetodoEstaticoExiste(clase, metodo);
        ValidarSiMetodoYaExiste(clase, metodo);

        ValidarOverrideVirtual(clase, metodo);
        MetodoEsOverride(clase, metodo);
        ValidarFuncionamientoModificadores(clase, metodo);
        MarcarClaseComoAbstractaSiCorresponde(clase, metodo);
        ValidarClaseSealedYMetodoHerencia(clase, metodo);
        if (clase?.ClaseBase != null && clase?.EsAbstract != true)
        {
            // ValidarImplementacionAbstractos(clase, metodo);
        }

        metodo.Clase = clase;
        clase?.MetodosInvocados.Add(metodo);
        _metodoServicio.ActualizarMetodo(metodo);
        _repositorio.Actualizar(clase);
    }

    public void QuitarAtributo(Guid idClase, Guid idAtributo)
    {
        ValidarClaseExiste(idClase);
        var clase = _repositorio.ObtenerClase(idClase);

        var atributo = clase?.Atributos.FirstOrDefault(a => a.Id == idAtributo);
        if(atributo == null)
        {
            throw new EntidadNoEncontradaExcepcion("El atributo no pertenece a la clase.");
        }

        clase.Atributos.Remove(atributo);
        _repositorio.Actualizar(clase);
    }

    public void QuitarMetodo(Guid idClase, Guid idMetodo)
    {
        ValidarClaseExiste(idClase);
        var clase = _repositorio.ObtenerClase(idClase);

        var metodo = clase?.MetodosInvocados.FirstOrDefault(m => m.Id == idMetodo);
        if(metodo == null)
        {
            throw new EntidadNoEncontradaExcepcion("El método no pertenece a la clase.");
        }

        clase.MetodosInvocados.Remove(metodo);
        _repositorio.Actualizar(clase);
    }

    private static void ValidarClaseNoNula(Clase? clase)
    {
        if(clase == null)
        {
            throw new EntidadNulaExcepcion("La clase no puede ser nula.");
        }
    }

    private static void ValidarNombreClase(string? nombre)
    {
        if(string.IsNullOrWhiteSpace(nombre))
        {
            throw new EntidadNulaExcepcion("El nombre no puede ser nulo o vacío.");
        }
    }

    private void ValidarSiClaseExiste(Clase? clase)
    {
        if(_repositorio.Existe(clase))
        {
            throw new EntidadYaExisteExcepcion("Ya existe una clase con esas características.");
        }
    }

    private void ValidarClaseExiste(Guid? id)
    {
        var existe = _repositorio.ObtenerClase(id);
        if(existe == null)
        {
            throw new EntidadNoEncontradaExcepcion("No se encuentra la clase solicitada");
        }
    }

    private void ValidarClaseBaseNoSealed(Clase? clase)
    {
        if (clase?.ClaseBase == null)
        {
            return;
        }

        var claseBase = ObtenerClasePorId(clase.ClaseBase.Id);
        if (claseBase.EsSealed)
        {
            throw new OperacionNoPermitidaExcepcion(
                $"La clase '{clase.Nombre}' no puede heredar de '{claseBase.Nombre}' porque es sealed.");
        }
    }

    private static void InicializarListasDeClase(Clase? clase)
    {
        if(clase?.Atributos == null)
        {
            clase.Atributos = new List<Atributo>();
        }

        if(clase?.MetodosInvocados == null)
        {
            clase.MetodosInvocados = new List<Metodo>();
        }
    }

    /*private void ValidarImplementacionAbstractos(Clase? clase, Metodo? metodoPendiente = null)
    {
        var metodosAbstractos = ObtenerMetodosJerarquia(ObtenerClasePorId(clase?.ClaseBase?.Id))
            .Where(m => m.EsAbstract);
        var propios = new List<Metodo>(clase.MetodosInvocados);

        if (metodoPendiente != null)
        {
            propios.Add(metodoPendiente);
        }

        if (clase?.EsAbstract != true)
        {
            var faltaImplementar = metodosAbstractos
                .Where(m =>
                    !propios.Any(mimpl => EsImplementacionDeAbstracto(mimpl, m)) &&
                    !(metodoPendiente != null && EsImplementacionDeAbstracto(metodoPendiente, m))
                );

            if (faltaImplementar.Any())
            {
                throw new OperacionNoPermitidaExcepcion("Falta implementar métodos abstractos");
            }
        }
    }*/

    private bool EsImplementacionDeAbstracto(Metodo overrideMetodo, Metodo abstractMetodo)
    {
        return overrideMetodo.EsOverride &&
               overrideMetodo.Nombre == abstractMetodo.Nombre &&
               overrideMetodo.Parametros.Count == abstractMetodo.Parametros.Count &&
               overrideMetodo.Parametros.Zip(abstractMetodo.Parametros)
                   .All(p => p.First.TipoDato.Id == p.Second.TipoDato.Id);
    }

    private void BuscarAtributo(Guid idAtributo)
    {
        var atributo = _atributoServicio.ObtenerAtributoPorId(idAtributo);
        if(atributo == null)
        {
            throw new EntidadNoEncontradaExcepcion("No se encontró el atributo");
        }
    }

    private static void AtributoEstaticoYaExiste(Clase? clase, Atributo atributo)
    {
        if(atributo.EsEstatico)
        {
            if(clase.Atributos.Where(a => a.EsEstatico).Any(a => a.Nombre == atributo.Nombre))
            {
                throw new EntidadYaExisteExcepcion($"Ya existe un atributo estático '{atributo.Nombre}'");
            }
        }
    }

    private static void AtributoYaExiste(Clase? clase, Atributo atributo)
    {
        if(clase.Atributos.Any(n => n.Nombre == atributo.Nombre))
        {
            throw new EntidadYaExisteExcepcion($"Ya existe un atributo con el nombre '{atributo.Nombre}' en la clase.");
        }
    }

    private static void BuscarMetodo(Metodo metodo)
    {
        if(metodo == null)
        {
            throw new EntidadNoEncontradaExcepcion("Este método no existe");
        }
    }

    private static bool MetodosSonIguales(Metodo primerMetodo, Metodo segundoMetodo)
    {
        if(primerMetodo.Nombre != segundoMetodo.Nombre)
        {
            return false;
        }

        if(primerMetodo.Parametros!.Count != segundoMetodo.Parametros!.Count)
        {
            return false;
        }

        return primerMetodo.Parametros.Zip(segundoMetodo.Parametros, (p1, p2) => p1.TipoDato!.Equals(p2.TipoDato))
            .All(x => x);
    }

    private static void ValidarSiMetodoEstaticoExiste(Clase? clase, Metodo metodo)
    {
        if(metodo.EsEstatico)
        {
            if(clase.MetodosInvocados.Where(m => m.EsEstatico).Any(m => MetodosSonIguales(m, metodo)))
            {
                throw new EntidadYaExisteExcepcion($"Ya existe un método estático'{metodo.Nombre}'");
            }
        }
    }

    private static void ValidarSiMetodoYaExiste(Clase? clase, Metodo metodo)
    {
        if(clase.MetodosInvocados.Any(m => MetodosSonIguales(m, metodo)))
        {
            throw new EntidadYaExisteExcepcion(
                $"Ya existe un método con el nombre '{metodo.Nombre}' y los mismos parametros");
        }
    }

    private static void ValidarSiMetodoEstaticoYAbstracto(Metodo metodo)
    {
        if(metodo.EsEstatico && metodo.EsAbstract)
        {
            throw new OperacionNoPermitidaExcepcion("Un método estático no puede ser abstracto");
        }
    }

    private void ValidarOverrideVirtual(Clase? clase, Metodo metodo)
    {
        if (!metodo.EsOverride)
        {
            return;
        }

        var aceptaOverride = clase.ClaseBase != null && ObtenerMetodosJerarquia(clase.ClaseBase)
            .Any(m => MetodosSonIguales(m, metodo) && (m.EsVirtual || m.EsAbstract));

        if (!aceptaOverride)
        {
            throw new OperacionNoPermitidaExcepcion(
                $"No podes sobreescribir '{metodo.Nombre}': no existe un metodo virtual o abstracto" +
                $" con esa firma en la jerarquia.");
        }
    }

    private void ValidarClaseBase(Clase? claseBase)
    {
        var claseExistente = _repositorio.ObtenerClase(claseBase!.Id);
        if(claseExistente == null)
        {
            throw new EntidadNoEncontradaExcepcion("La clase base indicada no existe");
        }
    }

    private void ValidarNoHerenciaCircular(Clase? clase)
    {
        if(clase?.ClaseBase == null)
        {
            return;
        }

        var actual = clase.ClaseBase;
        while(actual != null)
        {
            if(actual.Id == clase.Id)
            {
                throw new OperacionNoPermitidaExcepcion("No se puede establecer una herencia circular");
            }

            actual = actual.ClaseBase;
        }
    }

    private void ValidarNoTieneClasesDerivadas(Guid id)
    {
        var todas = _repositorio.ObtenerTodasLasClases();

        if (todas.Any(c => c.ClaseBase != null && c.ClaseBase.Id == id))
        {
            throw new OperacionNoPermitidaExcepcion("No se puede eliminar: existen clases que heredan de esta");
        }
    }

    private IEnumerable<Atributo> ObtenerAtributosJerarquia(Clase? clase)
    {
        foreach(var atributo in clase.Atributos)
        {
            yield return atributo;
        }

        if(clase.ClaseBase != null)
        {
            var padre = ObtenerClasePorId(clase.ClaseBase.Id);
            foreach(var atributo in ObtenerAtributosJerarquia(padre) )
            {
                yield return atributo;
            }
        }
    }

    private void ValidoSiHeredaAtributo(Clase? clase, Atributo atributo)
    {
        if(ObtenerAtributosJerarquia(clase).Any(a => a.Nombre == atributo.Nombre))
        {
            throw new EntidadYaExisteExcepcion("El atributo ya existe en la jerarquia");
        }
    }

    private IEnumerable<Metodo> ObtenerMetodosJerarquia(Clase? clase)
    {
        InicializarListasDeClase(clase);
        foreach(var metodo in clase.MetodosInvocados)
        {
            yield return metodo;
        }

        if(clase.ClaseBase != null)
        {
            var padre = ObtenerClasePorId(clase.ClaseBase.Id);
            foreach(var metodo in ObtenerMetodosJerarquia(padre) )
            {
                yield return metodo;
            }
        }
    }

    private void MetodoEsOverride(Clase? clase, Metodo metodo)
    {
        var enJerarquia = clase.ClaseBase != null &&
                          ObtenerMetodosJerarquia(clase).Any(m => MetodosSonIguales(m, metodo));
        if(!enJerarquia && clase.MetodosInvocados.Any(m => MetodosSonIguales(m, metodo)))
        {
            throw new EntidadYaExisteExcepcion("El método ya existe en la clase");
        }
    }

    public void VerificarImplementacionInterfaz(Clase clase)
    {
        foreach (var interfaz in clase.InterfacesImplementadas)
        {
            foreach (var metodoFirma in interfaz.FirmaMetodos)
            {
                var implementado = clase.MetodosInvocados.Any(m =>
                    m.Nombre == metodoFirma.Nombre &&
                    m.TipoRetorno == metodoFirma.TipoRetorno &&
                    CompararParametros(m.Parametros, metodoFirma.Parametros));

                if (!implementado)
                {
                    throw new OperacionNoPermitidaExcepcion($"La clase {clase.Nombre} no implementa el método {metodoFirma.Nombre} de la interfaz {interfaz.Nombre}.");
                }
            }
        }
    }

    public bool CompararParametros(List<Parametro>? a, List<Parametro>? b)
    {
        if(a == null || b == null)
        {
            return false;
        }

        if(a.Count != b.Count)
        {
            return false;
        }

        for (var i = 0; i < a.Count; i++)
        {
            if(a[i].TipoDato?.Nombre != b[i].TipoDato?.Nombre)
            {
                return false;
            }
        }

        return true;
    }

    private void MarcarClaseComoAbstractaSiCorresponde(Clase? clase, Metodo metodo)
    {
        if(!metodo.EsAbstract)
        {
            return;
        }

        if (clase.ClaseBase != null)
        {
            var claseBase = ObtenerClasePorId(clase.ClaseBase.Id);
            if (!claseBase.EsAbstract)
            {
                throw new OperacionNoPermitidaExcepcion(
                    $"La clase '{clase.Nombre}' se vuelve abstracta por contener un método abstracto, " +
                    $"pero su clase base '{claseBase.Nombre}' no es abstracta. Esto no está permitido.");
            }
        }

        clase.EsAbstract = true;
    }

    private void ValidarClaseSealedYMetodoHerencia(Clase? clase, Metodo metodo)
    {
        if (clase.EsSealed && (metodo.EsVirtual || metodo.EsAbstract))
        {
            throw new OperacionNoPermitidaExcepcion(
                $"No se puede agregar el metodo '{metodo.Nombre}' a la clase sealed '{clase.Nombre}' " +
                $"porque los metodos virtuales o abstractos no estan permitidos en clases sealed.");
        }
    }

    private void ValidarFuncionamientoModificadores(Clase? clase, Metodo metodo)
    {
        var metodosJerarquia = clase.ClaseBase != null
            ? ObtenerMetodosJerarquia(clase.ClaseBase)
            : Enumerable.Empty<Metodo>();

        ValidarOverrideTieneMetodoBase(metodo, metodosJerarquia);
        ValidarDuplicadoSinOverride(metodo, metodosJerarquia);
        ValidarVirtualSinRedefinicion(metodo, metodosJerarquia);
        ValidarRedefinicionSealed(metodo, metodosJerarquia);
        ValidarRedefinicionAbstract(metodo, metodosJerarquia);
    }

    private void ValidarOverrideTieneMetodoBase(Metodo metodo, IEnumerable<Metodo> metodosJerarquia)
    {
        if(!metodo.EsOverride)
        {
            return;
        }

        var metodoBase = metodosJerarquia.FirstOrDefault(m =>
            MetodosSonIguales(m, metodo) && (m.EsVirtual || m.EsAbstract));

        if (metodoBase == null)
        {
            throw new OperacionNoPermitidaExcepcion(
                $"El metodo '{metodo.Nombre}' no puede hacer override porque no hay ningun metodo virtual" +
                $" o abstracto con esa firma en la jerarquia.");
        }
    }

    private void ValidarDuplicadoSinOverride(Metodo metodo, IEnumerable<Metodo> metodosJerarquia)
    {
        var yaExiste = metodosJerarquia.Any(m => MetodosSonIguales(m, metodo));
        if (yaExiste && !metodo.EsOverride)
        {
            throw new OperacionNoPermitidaExcepcion(
                $"Ya existe un metodo con la firma de '{metodo.Nombre}' en la jerarquia. " +
                $"Debe declararse como override.");
        }
    }

    private void ValidarVirtualSinRedefinicion(Metodo metodo, IEnumerable<Metodo> metodosJerarquia)
    {
        if (!metodo.EsVirtual)
        {
            return;
        }

        var yaExiste = metodosJerarquia.Any(m => MetodosSonIguales(m, metodo));
        if (yaExiste)
        {
            throw new OperacionNoPermitidaExcepcion(
                $"No se puede declarar el metodo '{metodo.Nombre}' como virtual porque ya existe uno con " +
                $"esa firma en la jerarquia.");
        }
    }

    private void ValidarRedefinicionSealed(Metodo metodo, IEnumerable<Metodo> metodosJerarquia)
    {
        var metodoBase = metodosJerarquia.FirstOrDefault(m => MetodosSonIguales(m, metodo));
        if(metodoBase == null || metodo.EsOverride)
        {
            return;
        }

        if (metodoBase.EsSealed == true)
        {
            throw new OperacionNoPermitidaExcepcion(
                $"No se puede redefinir el metodo '{metodo.Nombre}' porque esta marcado como sealed" +
                $" en la clase base.");
        }
    }

    private void ValidarRedefinicionAbstract(Metodo metodo, IEnumerable<Metodo> metodosJerarquia)
    {
        var metodoBase = metodosJerarquia.FirstOrDefault(m => MetodosSonIguales(m, metodo));
        if(metodoBase != null && !metodo.EsOverride)
        {
            if (metodoBase.EsAbstract)
            {
                throw new OperacionNoPermitidaExcepcion(
                    $"Debe usar override para implementar el metodo abstracto '{metodo.Nombre}'.");
            }
        }
    }

    private void RegistrarTipoDatoParaClase(Clase? clase)
    {
        TipoDato? tipoPadre = null;
        if(clase.ClaseBase != null)
        {
            tipoPadre = _tipoDatoServicio.ObtenerTipoDatoPoId(clase.ClaseBaseId.Value);
            if(tipoPadre == null)
            {
                throw new EntidadNoEncontradaExcepcion(
                    $"No existe TipoDato para la clase base '{clase.ClaseBase.Nombre}'");
            }
        }

        var nuevoTipo = new TipoDato(clase.Nombre, tipoPadre!);
        nuevoTipo.Id = clase.Id;
        _tipoDatoServicio.CrearTipoDato(nuevoTipo);
    }
}
