using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inmobiliaria.Models;

[Table("pagos")]
public class Pago{

    [Display(Name="Codigo")]
    public int Id { get ; set ;}
    [Display(Name ="Numero de pago")]
    public int NroPago { get ; set;}
    public DateTime FechaPago { get ; set ; }
    public Double Importe { get ; set ; }
    [Display(Name ="Codigo de contrato")]
    public int IdContrato { get ; set ; }
    [ForeignKey(nameof(IdContrato))]
    public Contrato? Contrato { get ; set ; }

}