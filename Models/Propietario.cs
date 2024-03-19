
using System.ComponentModel.DataAnnotations;


namespace Inmobiliaria.Models;
public class Propietario : Persona
{
   

    public override string ToString()
    {
        return Apellido + " " + Nombre;
    }
}
