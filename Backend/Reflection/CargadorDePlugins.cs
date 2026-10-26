using System.Reflection;
using Dominio.Excepciones;
using Reflection.InterfacesReflection;

namespace Reflection;

public sealed class CargadorDePlugins<IInterfaz> : ICargadorDePlugins<IInterfaz>
    where IInterfaz : class
{
    private readonly DirectoryInfo directorio = EncontrarCarpetaPlugins();
    private static DirectoryInfo EncontrarCarpetaPlugins()
    {
        var directorioActual = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

        while (directorioActual != null)
        {
            var posiblePlugins = Path.Combine(directorioActual.FullName, "Backend", "Plugins");
            if (Directory.Exists(posiblePlugins))
            {
                return new DirectoryInfo(posiblePlugins);
            }

            directorioActual = directorioActual.Parent;
        }

        throw new OperacionNoPermitidaExcepcion("No se encontró la carpeta 'Backend/Plugins'.");
    }

    private List<Type> implementaciones = [];

    public List<string> ObtenerImplementaciones()
    {
        var archivos = directorio
            .GetFiles("*.dll")
            .ToList();
        implementaciones = [];

        foreach (var archivo in archivos)
        {
            var ensamblado = Assembly.LoadFile(archivo.FullName);

            var tiposCargados = ensamblado
                .GetTypes()
                .Where(tipo => tipo.IsClass && typeof(IInterfaz)
                    .IsAssignableFrom(tipo))
                .ToList();

            if(tiposCargados.Count == 0)
            {
                Console.WriteLine($"Nadie implementa la interfaz: {typeof(IInterfaz).Name} en el assembly: " +
                                  $"{archivo.FullName}");
            }

            implementaciones = implementaciones
                .Union(tiposCargados)
                .ToList();
        }

        return implementaciones.ConvertAll(t => t.Name);
    }

    public IInterfaz ObtenerImplementacion(int indice, params object[] argumentos)
    {
        try
        {
            var tipo = implementaciones.ElementAt(indice);
            var instancia = Activator.CreateInstance(tipo, argumentos) as IInterfaz;

            if (instancia is null)
            {
                throw new OperacionNoPermitidaExcepcion($"No se pudo instanciar el tipo {tipo.FullName} " +
                                                        $"como {typeof(IInterfaz).Name}");
            }

            return instancia;
        }
        catch (ArgumentOutOfRangeException)
        {
            throw new OperacionNoPermitidaExcepcion("Indice fuera de rango al " +
                                                    "intentar cargar el plugin");
        }
    }
}
