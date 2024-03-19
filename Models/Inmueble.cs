
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Inmobiliaria.Models;
[Table("inmuebles")]
public class Inmueble
{
    [Required]
    public int Id { get; set; }
    [Required]
    public int IdPropietario { get; set; }
    [ForeignKey(nameof(IdPropietario))]
    public Propietario? Propietario { get; set; }
    [Required]
    public string Direccion { get; set; } = "";
    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EnumUso Uso { get; set; }
    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EnumTipo Tipo { get; set; }
    [Required]
    public int CantAmbientes { get; set; }
    [Required]
    public double Latitud { get; set; }
    [Required]
    public double Longitud { get; set; }
    [Required]
    public double Precio { get; set; }
    [Required]
    public bool Estado { get; set; } = true;
    [Required]
    public bool Disponible { get; set; } = true;

    public override string ToString()
    {
        return "USO: " + Uso + " ----- TIPO: " + Tipo;
    }

}

public enum EnumUso
{
    Comercial,
    Personal,

}

public enum EnumTipo
{
    Local,
    Deposito,
    Casa,
    Departamento


}
