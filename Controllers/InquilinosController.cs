using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;
namespace Inmobiliaria.Controllers;
[Authorize]
public class InquilinosController : Controller
{
    private readonly ILogger<InquilinosController> _logger;

    public InquilinosController(ILogger<InquilinosController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        InquilinosRepository repo = new InquilinosRepository();
        List<Inquilino> inquilinos = repo.GetAllInquilinos();

        return View(inquilinos);
    }

    public IActionResult Create()
    {


        return View();
    }


    [HttpPost]
    public IActionResult Create(Inquilino inquilino)
    {

        try
        {
            InquilinosRepository repo = new();
            var res = repo.CreateInquilino(inquilino);
            InmueblesRepository repo2 = new();
            var enumTipo = repo2.getEnumTipos();
            var enumUso = repo2.getEnumUso();
            ViewBag.enumTipo = enumTipo;
            ViewBag.enumUso = enumUso;

            string returnUrl = Request.Headers["Referer"].ToString();
            Uri refererUri = new Uri(returnUrl);
            string relativePath = refererUri.GetComponents(UriComponents.PathAndQuery, UriFormat.SafeUnescaped);
            Console.WriteLine(relativePath);
            if (res > 0)
            {

                TempData["AlertMessage"] = "Inquilino creado correctamente.";
                TempData["AlertType"] = "success";
                Console.WriteLine("RELATIVE PATH: " + relativePath);


                if (relativePath == "/Inquilinos/Create")
                {

                    return Redirect("index");
                }



            }
            else if (res == -1)
            {
                TempData["AlertMessage"] = "No se pudo crear el inquilino, Error en la base de datos.";
                TempData["AlertType"] = "error";

            }
            else if (res == -2)
            {
                TempData["AlertMessage"] = "No se pudo crear el inquilino, Ya existe uno con ese DNI.";
                TempData["AlertType"] = "error";

            }
            else if (res == -3)
            {
                TempData["AlertMessage"] = "No se pudo crear el inquilino, Ingrese un email valido.";
                TempData["AlertType"] = "error";

            }
            else if (res == -4)
            {
                TempData["AlertMessage"] = "No se pudo crear el inquilino, Ingrese un DNI valido.";
                TempData["AlertType"] = "error";

            }
            else
            {
                TempData["AlertMessage"] = "No se pudo crear el inquilino, Ya existe un propietario con ese DNI.";
                TempData["AlertType"] = "error";

            }
            return Redirect(relativePath);
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

            InquilinosRepository repo = new();
            repo.DeleteInquilino(id);
            return RedirectToAction("Index");
        }
        catch (System.Exception)
        {
            throw;
        }
    }
    public IActionResult Update(int id)
    {
        InquilinosRepository repo = new();
        var inquilino = repo.GetInquilinoById(id);
        return View(inquilino);
    }


    [HttpPost]
    public IActionResult Update(Inquilino inquilino)
    {

        string returnUrl = Request.Headers["Referer"].ToString();
        Uri refererUri = new Uri(returnUrl);
        string relativePath = refererUri.GetComponents(UriComponents.PathAndQuery, UriFormat.SafeUnescaped);
        try
        {
            InquilinosRepository repo = new();
            int res = repo.UpdateInquilino(inquilino);

            if (res > 0)
            {

                TempData["AlertMessage"] = "Inquilino modificado correctamente.";
                TempData["AlertType"] = "success";
                return RedirectToAction("Index");
            }
            else if (res == -1)
            {
                TempData["AlertMessage"] = "No se pudo modificar el inquilino, Error en la base de datos.";
                TempData["AlertType"] = "error";
                return Redirect(relativePath);
            }
            else if (res == -2)
            {
                TempData["AlertMessage"] = "No se pudo modificar el inquilino, Ingrese un numero telefonico valido.";
                TempData["AlertType"] = "error";
                return Redirect(relativePath);
            }
            else if (res == -3)
            {
                TempData["AlertMessage"] = "No se pudo modificar el inquilino, Ingrese un email valido.";
                TempData["AlertType"] = "error";
                return Redirect(relativePath);
            }
            else if (res == -4)
            {
                TempData["AlertMessage"] = "No se pudo modificar el inquilino, Ingrese un DNI valido.";
                TempData["AlertType"] = "error";
                return Redirect(relativePath);
            }
            else
            {
                TempData["AlertMessage"] = "No se pudo modificar el inquilino, Ya existe un inquilino con ese DNI.";
                TempData["AlertType"] = "error";
                return Redirect(relativePath);
            }
        }
        catch (System.Exception)
        {

            throw;
        }

    }

    public IActionResult Details(int id)
    {
        InquilinosRepository repo = new();
        var inquilino = repo.GetInquilinoById(id);
        return View(inquilino);
    }

    //APIS

    [HttpGet("api/Inquilios/GetInquilino/{dni}")]
    public IActionResult GetInquilino(string dni)
    {
        // Aquí, realiza la lógica para buscar el propietario por DNI
        InquilinosRepository repo = new();
        Inquilino inquilinoEncontrado = repo.GetInquilinoByDni(dni); // Tu lógica de búsqueda

        if (inquilinoEncontrado != null)
        {
            // Retorna el objeto Propietario como JSON
            return Ok(inquilinoEncontrado);
        }
        else
        {
            return NotFound();
        }
    }

    [HttpGet("api/Inmuebles/GetInquilinos")]
    public IActionResult GetAllInquilinos()
    {

        InquilinosRepository repo = new();
        List<Inquilino> inquilinos = repo.GetAllInquilinos();

        if (inquilinos.Count > 0)
        {
            // Retorna la lista de propietarios como JSON
            return Ok(inquilinos);
        }
        else
        {
            return NotFound();
        }
    }

    [HttpGet("Inquilinos/FiltrarInquilinos")]
    public IActionResult FiltrarInquilinos([FromQuery] string searchTerm)
    {

        IActionResult result = GetAllInquilinos();

        if (result is OkObjectResult okObjectResult)
        {
            var inquilinosResponse = okObjectResult.Value;
            if (inquilinosResponse is List<Inquilino> inquilino)
            {
                if (string.IsNullOrEmpty(searchTerm))
                {
                    return Json(inquilino);
                }

                var inquilinosFiltrados = inquilino.Where(p =>
                    p.Nombre.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase) ||
                    p.Apellido.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase) ||
                    p.Dni.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase)
                ).ToList();

                return Json(inquilinosFiltrados);
            }
        }
        return BadRequest("Error al obtener los inquilinos");
    }

}