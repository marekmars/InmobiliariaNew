
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Inmobiliaria.Models;
[Table("contratos")]
public class Contrato
{
    [Required]
    public int Id { get; set; }
    [Required]
    public int IdInquilino { get; set; }
    [Required]
    public Inquilino? Inquilino { get; set; }
    [Required]
    public int IdInmueble { get; set; }
    [Required]
    public Inmueble? Inmueble { get; set; }
    [Required]
    public Double MontoMensual { get; set; }
    [Required]
    public DateTime FechaInicio { get; set; }
    [Required]
    public DateTime FechaFin { get; set; }
    [Required]
    public byte Estado { get; set; }

}
