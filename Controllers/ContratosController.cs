using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;

namespace Inmobiliaria.Controllers;
[Authorize]
public class ContratosController : Controller
{
    private readonly ILogger<ContratosController> _logger;

    public ContratosController(ILogger<ContratosController> logger)
    {
        _logger = logger;
    }


    public IActionResult Index()
    {
        ContratosRepository repo = new();
    
        List<Contrato> contratos = repo.GetAllContratos(false);
        ViewBag.Disponibles = false;
        return View(contratos);
    }

 
    public IActionResult IndexDisponibles()
    {
        ContratosRepository repo = new();
        List<Contrato> contratos = repo.GetAllContratos(true);
        ViewBag.Disponibles = true;
        return View("Index", contratos);

    }
    public IActionResult FiltrarPorFecha(DateTime Desde, DateTime Hasta)
    {
        ContratosRepository repo = new();
        List<Contrato> contratos = repo.GetAllContratosFecha(Desde, Hasta);
        ViewBag.Disponibles = true;
        return View("Index", contratos);

    }
    public IActionResult FiltrarPorInmueble(int id)
    {
        ContratosRepository repo = new();
        InmueblesRepository repoInmuebles=new();
        List<Contrato> contratos = repo.GetAllContratosInmueble(id);
        ViewBag.idPropietario=repoInmuebles.GetInmuebleById(id).IdPropietario;
        
        return View("Index", contratos);

    }
    public IActionResult Create()
    {
        return View();
    }



    [HttpPost]
    public IActionResult Create(Contrato contrato)
    {
        try
        {
            ContratosRepository repo = new();
            var res = repo.CreateContrato(contrato);
            if (res > 0)
            {

                TempData["AlertMessage"] = "Contrato creado correctamente.";
                TempData["AlertType"] = "success";
                return RedirectToAction("Index");
            }
            else if (res == -1)
            {
                TempData["AlertMessage"] = "No se pudo crear el Contrato, debido a un error en los datos ingresados";
                TempData["AlertType"] = "error";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["AlertMessage"] = "No se pudo crear el Contrato, Ya existe uno para ese inmueble en la fecha seleccionada.";
                TempData["AlertType"] = "error";
                return RedirectToAction("Index");
            }

        }
        catch (System.Exception)
        {
            throw;
        }

    }

    [HttpPost]
    [Authorize(Policy = "Administrador")]
    public IActionResult Delete(int id, string resignacion)
    {
        try
        {
            ContratosRepository repo = new();
            InquilinosRepository repoInquilino = new();
            PagosRepository repoPago = new();
            string RefererUrl = Request.Headers["Referer"].ToString();

            if (string.IsNullOrWhiteSpace(resignacion))
            {
                var pagos = repoPago.GetPagoByContratoId(id);
                repo.DeleteContrato(id);
            }
            else
            {
                if (ContratoPagado(id))
                {
                    Contrato contrato = repo.GetContratoById(id);
                    if (contrato != null)
                    {

                        // Calcular la multa
                        ContratoPagado(contrato.Id);
                        DateTime fechaFin = contrato.FechaFin;
                        DateTime fechaHoy = DateTime.Now;
                        TimeSpan tiempoTranscurrido = fechaFin - fechaHoy;
                        double diasTranscurridos = tiempoTranscurrido.Days;
                        double mesesTranscurridos = Math.Round(diasTranscurridos / 30);
                        Console.WriteLine("MEses transcurridos: " + mesesTranscurridos);
                        double multa;

                        TempData["Inquilino"] = repoInquilino.GetInquilinoById(contrato.IdInquilino).ToString();
                        if (mesesTranscurridos >= 2)
                        {
                            multa = contrato.MontoMensual * 2;
                            TempData["Multa"] = "$" + multa;
                        }
                        else
                        {
                            multa = contrato.MontoMensual;
                            TempData["Multa"] = "$" + multa;
                        }

                        TempData["AlertMessage"] = $"Se resigno el contrato, el cliente debera abonar una multa de {TempData["Multa"]}";
                        TempData["AlertType"] = "warning";



                        contrato.FechaFin = fechaHoy;
                        repo.DeleteContrato(contrato.Id);
                        repo.UpdateContrato(contrato);
                    }
                    else
                    {
                        throw new Exception("Contrato no encontrado");
                    }
                }
                else
                {
                    TempData["AlertMessage"] = $"No se pudo cancelar el contrato, debido a que el contrato no esta pagado en su totalidad";
                    TempData["AlertType"] = "error";
                }

            }

            return Redirect(RefererUrl);
        }
        catch (Exception ex)
        {
            // Manejar la excepción de manera adecuada, como registrarla o mostrar un mensaje de error al usuario
            return RedirectToAction($"Error: {ex.Message}");
        }
    }

    public IActionResult Update(int id)
    {
        ContratosRepository repo = new();
        var contrato = repo.GetContratoById(id);
        TempData["RefererUrl"] = Request.Headers["Referer"].ToString();
        return View(contrato);
    }

    [HttpPost]
    public IActionResult Update(Contrato contrato)
    {
        ContratosRepository repo = new();
        string? returnUrl = TempData["RefererUrl"]?.ToString();
        int res = repo.UpdateContrato(contrato);
        if (res > 0)
        {

            TempData["AlertMessage"] = "Contrato modificado correctamente.";
            TempData["AlertType"] = "success";
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                // Maneja el caso en el que no haya URL de referencia
                return RedirectToAction("Index");
            }
        }
        else if (res == -1)
        {
            TempData["AlertMessage"] = "No se pudo modificar el Contrato, debido a un error en los datos ingresados";
            TempData["AlertType"] = "error";
            return RedirectToAction("Update");
        }
        else
        {
            TempData["AlertMessage"] = "No se pudo modificar el Contrato, Ya existe uno para ese inmueble en la fecha seleccionada.";
            TempData["AlertType"] = "error";
            return RedirectToAction("Update");
        }

    }

    public IActionResult Details(int id)
    {
        ContratosRepository repo = new();
        var contrato = repo.GetContratoById(id);
        return View(contrato);
    }

    [HttpGet("api/Contratos/GetContratos")]
    public IActionResult GetAllContratos()
    {
        // Aquí, realiza la lógica para obtener todos los contratos
        ContratosRepository repo = new();
        List<Contrato> contrato = repo.GetAllContratos(true); // Tu lógica para obtener todos los contratos

        if (contrato.Count > 0)
        {
            // Retorna la lista de contrats como JSON
            return Ok(contrato);
        }
        else
        {
            return NotFound();
        }
    }

    [HttpGet("Contratos/FiltrarContratos")]
    public IActionResult FiltrarContratos([FromQuery] string searchTerm)
    {

        IActionResult result = GetAllContratos(); // Obtener el resultado

        if (result is OkObjectResult okObjectResult)
        {
            var contratosResponse = okObjectResult.Value; // Obtener el contenido del resultado
            if (contratosResponse is List<Contrato> contratos)
            {
                if (string.IsNullOrEmpty(searchTerm))
                {
                    return Json(contratos);
                }


                var contratosFiltrados = contratos.Where(c =>
                    c.Inquilino.Nombre.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase) ||
                    c.Inquilino.Apellido.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase) ||
                    c.Inquilino.Dni.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase) ||
                    c.Inmueble.Direccion.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase) ||
                    c.MontoMensual.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase)


                ).ToList();

                return Json(contratosFiltrados);
            }
        }

        // Si algo salió mal o el tipo no coincide, regresa un error u otra respuesta
        return BadRequest("Error al obtener los contratos");
    }
    private bool ContratoPagado(int id)
    {
        ContratosRepository repo = new();
        PagosRepository repoPagos = new();
        var contrato = repo.GetContratoById(id);
        DateTime fechaInicio = contrato.FechaInicio;
        DateTime fechaHoy = DateTime.Now;
        TimeSpan tiempoTranscurrido = fechaHoy - fechaInicio;
        double diasTranscurridos = tiempoTranscurrido.Days + 1;
        double mesesTranscurridos = Math.Round(diasTranscurridos / 30);
        double valorParcialContrato = contrato.MontoMensual * mesesTranscurridos;
        double totalPAgados = 0;
        List<Pago> pagos = repoPagos.GetPagoByContratoId(id);
        pagos.ForEach(pago => totalPAgados += pago.Importe);
        double resto = valorParcialContrato - totalPAgados;
        Console.WriteLine("resto: " + resto);
        if (resto <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }


    }
}