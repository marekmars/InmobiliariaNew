using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria.Models;

public class Usuario : Persona {

    public string? Clave { get ; set ; }
    public string? Avatar { get ; set ; }
    public IFormFile? AvatarFile { get ; set ; }
    public EnumRol Rol { get ; set ; }
    
   
}
public enum EnumRol
{
    Administrador,
    Empleado,

}

