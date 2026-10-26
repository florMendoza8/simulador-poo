using Dominio;

namespace Servicios.InterfacesServicios;

public interface IMetodoServicio
{
    public Metodo AgregarMetodo(Metodo metodo);

    public void EliminarMetodo(Metodo metodo);

    public Metodo ObtenerMetodoPorId(Guid metodoId);

    public List<Metodo> ObtenerTodosMetodos();

    public void AgregarParametro(Guid metodoId, Guid parametroId);

    public void AgregarVariableLocal(Guid metodoId, Guid variableLocalId);

    public void AgregarMetodoInvocado(Guid metodoOrigenId, Guid metodoInvocadoId);

    public void ActualizarMetodo(Metodo metodo);

    public Metodo AgregarFirmaMetodo(Metodo metodo);
    public List<Metodo> ObtenerTodasFirmaMetodos();
}
