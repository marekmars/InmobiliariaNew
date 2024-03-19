
using System.ComponentModel.DataAnnotations;


namespace Inmobiliaria.Models;
public abstract class Persona
{
    public int Id { get; set; } 
    [Required]
    public string Dni { get; set; } = "";
    [Required]
    public string Apellido { get; set; } = "";
    [Required]
    public string Nombre { get; set; } = "";
    [Required]
    public string Telefono { get; set; } = "";
    [Required]
    public string Correo { get; set; } = "";
    [Required]
    public byte Estado { get; set; } = 1;
}