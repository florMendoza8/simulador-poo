namespace Reflection.InterfacesReflection;

public interface ICargadorDePlugins<IInterfaz> where IInterfaz : class
{
    public List<string> ObtenerImplementaciones();
    public IInterfaz ObtenerImplementacion(int indice, params object[] argumentos);
}
