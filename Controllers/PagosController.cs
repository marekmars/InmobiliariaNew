using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;
namespace Inmobiliaria.Controllers;
[Authorize]
public class PagosController : Controller
{
    private readonly ILogger<PagosController> _logger;

    public PagosController(ILogger<PagosController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index(int id)
    {    
        PagosRepository repo = new();
        ContratosRepository repoContrato = new();
        Console.WriteLine("IdContrato: "+id);
        ViewBag.Contrato = repoContrato.GetContratoById(id); 
        List<Pago> pagos = repo.GetPagoByContratoId(id);    
        TempData["IdContrato"] = id;
        if (pagos.Count > 0)
        {
            return View(pagos);
        }
        else
        {
            return View();
        }

    }
    // public IActionResult Index2(int id)
    // {
    //     PagosRepository repo = new();
    //     ContratosRepository repoContrato = new();
    //     ViewBag.Contrato = repoContrato.GetContratoById(id);
    //     List<Pago> pagos = repo.GetPagoByContratoId(id);
    //     ViewBag.LinkBack="/Contratos/FiltrarPorInmueble/"+pagos[0].Contrato.IdInmueble;
    //     TempData["IdContrato"] = id;
    //     if (pagos.Count > 0)
    //     {
    //         return View("Index",pagos);
    //     }
    //     else
    //     {
    //         return View("Index");
    //     }

    // }
    public IActionResult Create(int IdContrato)
    {
        ContratosRepository repo = new();
        Contrato contrato = repo.GetContratoById(IdContrato);
        return View(contrato);
    }



    [HttpPost]
    public IActionResult Create(Pago pago)
    {
        try
        {
            PagosRepository repo = new();
            var res = repo.CreatePago(pago);
            ContratosRepository repoContrato = new();
            ViewBag.Contrato = repoContrato.GetContratoById(pago.IdContrato);

            string returnUrl = $"/Pagos/Index/{pago.IdContrato}";

            if (res > 0)
            {
                TempData["AlertMessage"] = "Contrato creado correctamente.";
                TempData["AlertType"] = "success";
            }
            else
            {
                TempData["AlertMessage"] = "No se pudo generar el pago, debido a un error en el sistema.";
                TempData["AlertType"] = "error";
            }

            // Redireccionar a la página de Index de Pagos con el parámetro IdContrato
            return Redirect(returnUrl);
        }
        catch (System.Exception)
        {
            throw;
        }
    }


    [HttpPost]
    [Authorize(Policy = "Administrador")]
    public IActionResult Delete(int id)
    {
        try
        {
            PagosRepository repo = new();
            repo.DeletePago(id);
            string returnUrl = Request.Headers["Referer"].ToString();
            Uri refererUri = new Uri(returnUrl);
            string relativePath = refererUri.GetComponents(UriComponents.PathAndQuery, UriFormat.SafeUnescaped);
             TempData["AlertMessage"] = "Pago eliminado correctamente.";
                TempData["AlertType"] = "success";
            return Redirect(relativePath);
        }
        catch (System.Exception)
        {
            throw;
        }
    }
    public IActionResult Update(int id)
    {
        PagosRepository repo = new();
        ContratosRepository repoContrato = new();
        var pago = repo.GetPagoById(id);
        Contrato contrato = repoContrato.GetContratoById(pago.IdContrato);
        pago.Contrato = contrato;


        return View(pago);
    }

    [HttpPost]
    public IActionResult Update(Pago pago)
    {
        try
        {
            PagosRepository repo = new();
            var res = repo.UpdatePago(pago);
            ContratosRepository repoContrato = new();
            ViewBag.Contrato = repoContrato.GetContratoById(pago.IdContrato);

            string returnUrl = $"/Pagos/Index/{pago.IdContrato}";

            if (res > 0)
            {
                TempData["AlertMessage"] = "Contrato modificado correctamente.";
                TempData["AlertType"] = "success";
            }
            else
            {
                TempData["AlertMessage"] = "No se pudo modificar el pago, debido a un error en el sistema.";
                TempData["AlertType"] = "error";
            }

            // Redireccionar a la página de Index de Pagos con el parámetro IdContrato
            return Redirect(returnUrl);
        }
        catch (System.Exception)
        {
            throw;
        }

    }

    public IActionResult Details(int id)
    {
        PagosRepository repo = new();
        ContratosRepository repoContrato = new();
        var contrato = repo.GetPagoById(id);
        contrato.Contrato = repoContrato.GetContratoById(contrato.IdContrato);
        return View(contrato);
    }


}