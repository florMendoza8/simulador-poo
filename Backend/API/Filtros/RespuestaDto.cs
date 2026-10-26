namespace API.Filtros;

public class RespuestaDto
{
    public object? Contenido { get; set; }
    public bool EjecucionExitosa { get; set; }
    public string? Mensaje { get; set; }
    public string? Detalles { get; set; }
}
