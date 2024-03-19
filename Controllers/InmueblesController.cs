using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualBasic;
namespace Inmobiliaria.Controllers;

[Authorize]
public class InmueblesController : Controller
{
    private readonly ILogger<InmueblesController> _logger;
    private readonly InmueblesRepository _repo=new();

    public InmueblesController(ILogger<InmueblesController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index(int id)
    {
        if (id == 0)
        {
            Console.WriteLine($"id: {id}");
            InmueblesRepository repo = new();
            List<Inmueble> inmueble = _repo.GetAllInmuebles();
            ViewBag.Disponibles = 0;
            return View(inmueble);
        }
        else
        {

            return IndexInmueblesPropietario(id);
        }

    }
    public IActionResult IndexDisponibles(int id)
    {
        InmueblesRepository repo = new();
        List<Inmueble> inmueble = repo.GetAllInmueblesDisponibles();
        ViewBag.Disponibles = 1;
        return View("Index", inmueble);

    }
    public IActionResult IndexInmueblesPropietario(int id)
    {
        InmueblesRepository repo = new();
        PropietariosRepository repoPropietarios = new();
        ViewBag.NombrePropietario=repoPropietarios.GetPropietarioById(id).ToString();
       
        List<Inmueble> inmuebles = repo.GetInmueblesPropietario(id);
          

        return View("Index", inmuebles);
    }
    public IActionResult Create()
    {
        InmueblesRepository repo = new();
        var enumTipo = repo.getEnumTipos();
        var enumUso = repo.getEnumUso();
        ViewBag.enumTipo = enumTipo;
        ViewBag.enumUso = enumUso;
        return View();
    }

    [HttpPost]
    public IActionResult Create(Inmueble inmueble)
    {

        try
        {
            InmueblesRepository repo = new();

            var res = repo.CreateInmueble(inmueble);
            if (res > 0)
            {

                TempData["AlertMessage"] = "Inmueble creado correctamente.";
                TempData["AlertType"] = "success";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["AlertMessage"] = "No se pudo crear el inmueble.";
                TempData["AlertType"] = "error";
                return RedirectToAction("Create");
            }

        }
        catch (System.Exception)
        {
            throw;
        }

    }
    [HttpPost]
    public IActionResult PausarPlay(int id)
    {
        try
        {

            InmueblesRepository repo = new();
            Inmueble inmueble = repo.GetInmuebleById(id);
            string returnUrl = Request.Headers["Referer"].ToString();
            Uri refererUri = new Uri(returnUrl);
            string relativePath = refererUri.GetComponents(UriComponents.PathAndQuery, UriFormat.SafeUnescaped);

            if (inmueble.Disponible)
            {
                inmueble.Disponible = false;
                TempData["AlertMessage"] = "El Inmueble se pauso correctamente.";
                TempData["AlertType"] = "success";
            }
            else
            {
                inmueble.Disponible = true;
                TempData["AlertMessage"] = "El Inmueble se encuentra disponible nuevamente.";
                TempData["AlertType"] = "success";
            }
            repo.UpdateInmuebleDisponible(inmueble);

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
        string returnUrl = Request.Headers["Referer"].ToString();
        Uri refererUri = new Uri(returnUrl);
        string relativePath = refererUri.GetComponents(UriComponents.PathAndQuery, UriFormat.SafeUnescaped);
        try
        {
            InmueblesRepository repo = new();
            repo.DeleteInmueble(id);
            return Redirect(relativePath);
        }
        catch (System.Exception)
        {
            throw;
        }
    }
    public IActionResult Update(int id)
    {
        InmueblesRepository repo = new();
        var inmueble = repo.GetInmuebleById(id);
        var enumTipo = repo.getEnumTipos();
        var enumUso = repo.getEnumUso();
        TempData["RefererUrl"] = Request.Headers["Referer"].ToString();
        ViewBag.enumTipo = enumTipo;
        ViewBag.enumUso = enumUso;
        return View(inmueble);
    }

    [HttpPost]
    public IActionResult Update(Inmueble inmueble)
    {
        InmueblesRepository repo = new();
        Console.WriteLine(inmueble.ToString());
        string? returnUrl = TempData["RefererUrl"]?.ToString();
        int res = repo.UpdateInmueble(inmueble);
        if (res > 0)
        {

            TempData["AlertMessage"] = "Inmueble modificado correctamente.";
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
        else
        {
            TempData["AlertMessage"] = "No se pudo modificar el inmueble.";
            TempData["AlertType"] = "error";
            return RedirectToAction("Update");
        }

    }

    public IActionResult Details(int id)
    {
        InmueblesRepository repo = new();
        var propietario = repo.GetInmuebleById(id);
        return View(propietario);
    }

    [HttpGet("api/Inmuebles/GetInmueble/{id}")]
    public IActionResult GetInmueble(int id)
    {
        // Aquí, realiza la lógica para buscar el propietario por DNI
        InmueblesRepository repo = new();
        Inmueble inmuebleEncontrado = repo.GetInmuebleById(id); // Tu lógica de búsqueda

        if (inmuebleEncontrado != null)
        {
            // Retorna el objeto Propietario como JSON
            return Ok(inmuebleEncontrado);
        }
        else
        {
            return NotFound();
        }
    }

    // [HttpGet("api/Inmuebles/GetInmuebles")]
    // public IActionResult GetAllInmuebles()
    // {

    //     InmueblesRepository repo = new();
    //     List<Inmueble> inmuebles = repo.GetAllInmuebles();
    //     Console.WriteLine(inmuebles[1].ToString());

    //     if (inmuebles.Count > 0)
    //     {

    //         return Ok(inmuebles);
    //     }
    //     else
    //     {
    //         return NotFound();
    //     }
    // }

    [HttpGet("Inmuebles/FiltrarInmuebles")]
    public IActionResult FiltrarInmuebles([FromQuery] string searchTerm,[FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
    {
        Console.WriteLine("ENTRO");
        IActionResult result = Ok(_repo.GetAllInmueblesFecha(fechaInicio, fechaFin));
        // Obtener el resultado

        if (result is OkObjectResult okObjectResult)
        {
            var inmueblesResponse = okObjectResult.Value; // Obtener el contenido del resultado
            if (inmueblesResponse is List<Inmueble> inmuebles)
            {
                if (string.IsNullOrEmpty(searchTerm))
                {
                    var inmueblesActivos = inmuebles.Where(i => i.Estado  &&
                    i.Disponible ).ToList();
                    return Json(inmueblesActivos); // Retorna los inmuebles activos si no hay término de búsqueda
                }

                // Filtra los inmuebles activos por el término de búsqueda (puedes personalizar la lógica según tus necesidades)
                var inmueblesFiltrados = inmuebles.Where(i =>
                    i.Estado == true &&
                    i.Disponible == true &&
                    (i.Propietario.Nombre.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                     i.Propietario.Apellido.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                     i.Propietario.Dni.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                     i.Direccion.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                     || i.Tipo.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase)

                    )
                ).ToList();

                // var propiedades = typeof(Inmueble).GetProperties();

                // foreach (var propiedad in propiedades)
                // {
                //     var nombrePropiedad = propiedad.Name;
                //     var valorPropiedad = propiedad.GetValue(inmueblesFiltrados);

                //     Console.WriteLine($"{nombrePropiedad}: {valorPropiedad}");
                // }
                return Json(inmueblesFiltrados);
            }
        }

        // Si algo salió mal o el tipo no coincide, regresa un error u otra respuesta
        return BadRequest("Error al obtener los inmuebles");
    }



}