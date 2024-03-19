using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Inmobiliaria.Models;
public class Login 
{
    
     [Required]
    public string Correo { get; set; }="";
     [Required]
    public string Clave { get; set; }="";
    

}
