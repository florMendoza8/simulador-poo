namespace Dominio;

/// <summary>
/// Representa los niveles de accesibilidad posibles para un recurso.
/// </summary>
public enum Accesibilidad
{
    /// <summary>
    /// Accesible para todos.
    /// </summary>
    Public,

    /// <summary>
    /// Accesible solo dentro de la clase o clases derivadas.
    /// </summary>
    Protected,

    /// <summary>
    /// Accesible solo dentro de la clase.
    /// </summary>
    Private
}
