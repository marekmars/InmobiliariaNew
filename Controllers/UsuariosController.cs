using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;
namespace Inmobiliaria.Controllers;

[Authorize]

public class UsuariosController : Controller
{
    private readonly ILogger<UsuariosController> _logger;
    private UsuariosRepository _repo;
    private readonly IConfiguration configuration;
    private readonly IWebHostEnvironment environment;

    public UsuariosController(ILogger<UsuariosController> logger, IConfiguration configuration, IWebHostEnvironment enviroment)
    {
        _repo = new UsuariosRepository();
        this.configuration = configuration;
        this.environment = enviroment;
        _logger = logger;
    }
    [Authorize(Policy = "Administrador")]
    public IActionResult Index()
    {
        List<Usuario> users = _repo.GetAllUsuarios();
        return View(users);
    }

    [AllowAnonymous]
    public IActionResult Login()
    {

        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(Login login)

    {
        Console.WriteLine("ENTRO LOGIN");
        try
        {
            Console.WriteLine("ENTRO LOGIN");
            if (ModelState.IsValid)
            {

                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: login.Clave,
                    salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8
                ));
                var r = _repo.GetUserByMail(login.Correo);
                Console.WriteLine(r);
                if (r == null || r.Clave != hashed)
                {
                    TempData["AlertMessage"] = "Usuario o contraseña incorrectos";
                    TempData["AlertType"] = "error";
                    return View("Login");
                }
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, r.Correo),
                        new Claim(ClaimTypes.Role, r.Rol.ToString())
                    };
                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme
                );
                await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));
                return Redirect("/Home");
            }
            return View();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();
        }
    }


    [AllowAnonymous]
    public async Task<ActionResult> Logout()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme
        );
        return RedirectToAction("Index", "Home");
    }

    [Authorize(Policy = "Administrador")]
    public IActionResult Create()
    {
        UsuariosRepository repo = new();
        var roles = repo.getEnumRol();
        ViewBag.roles = roles;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Administrador")]
    public ActionResult Create(Usuario usuario, string ClaveNuevamente)
    {
        try
        {
            string returnUrl = Request.Headers["Referer"].ToString();
            Uri refererUri = new Uri(returnUrl);
            string relativePath = refererUri.GetComponents(UriComponents.PathAndQuery, UriFormat.SafeUnescaped);
            if (ClaveNuevamente != usuario.Clave)
            {

                TempData["AlertMessage"] = "Las contraseñas no coinciden";
                TempData["AlertType"] = "error";
                return RedirectToAction("Create");
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: usuario.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8
                    ));
            usuario.Clave = hashed;

            var res = _repo.CreateUsuario(usuario);
            if (res == -2)
            {

                TempData["AlertMessage"] = "Ya existe una cuenta registrada con ese email.";
                TempData["AlertType"] = "error";
                return RedirectToAction("Create");

            }
            if (res == -3)
            {
                TempData["AlertMessage"] = "Ya existe una cuenta registrada con ese Dni.";
                TempData["AlertType"] = "error";
                return RedirectToAction("Create");
            }
            if (usuario.AvatarFile != null && usuario.Id > 0)
            {
                string wwwPath = environment.WebRootPath;
                string path = Path.Combine(wwwPath, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string fileName = "avatar_" + usuario.Id + Path.GetExtension(usuario.AvatarFile.FileName);
                string pathCompleto = Path.Combine(path, fileName);
                usuario.Avatar = Path.Combine("/Uploads", fileName);
                using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                {
                    usuario.AvatarFile.CopyTo(stream);
                }
                _repo.UpdateAvatar(usuario);

            }

            TempData["AlertMessage"] = "Usuario creado correctamente";
            TempData["AlertType"] = "success";
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            throw;
        }
    }

    public ActionResult Perfil()
    {
        UsuariosRepository repo = new();
        var roles = repo.getEnumRol();
        ViewBag.roles = roles;
        ViewBag.Titulo = "Mi Perfil";
        var u = _repo.GetUserByMail(User.Identity.Name);
        return View("Update", u);
    }

    [Authorize(Policy = "Administrador")]
    public ActionResult Update(int id)
    {
        UsuariosRepository repo = new();
        var roles = repo.getEnumRol();
        ViewBag.roles = roles;
        ViewBag.Titulo = "Modificar Usuario";
        var usuario = _repo.GetUserById(id);
        return View(usuario);
    }

    // [HttpPost]

    // public ActionResult Update(Usuario usuario, string ClaveActual, string ClaveNueva)
    // {
    //     var usuarioAnt = _repo.GetUserById(usuario.Id);

    //     var claveActual = Convert.ToBase64String(KeyDerivation.Pbkdf2(
    //                     password: ClaveActual,
    //                     salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
    //                     prf: KeyDerivationPrf.HMACSHA1,
    //                     iterationCount: 1000,
    //                     numBytesRequested: 256 / 8
    //                 ));
    //     Console.WriteLine("ClaveActual: " + claveActual);
    //     Console.WriteLine("ClaveAnterior: " + usuarioAnt.Clave);


    //     try
    //     {
    //         if (ClaveNueva == null || ClaveNueva == "")
    //         {
    //             usuario.Clave = usuarioAnt.Clave;
    //         }
    //         else
    //         {

    //             {
    //                 if (claveActual == usuarioAnt.Clave)
    //                 {
    //                     Console.WriteLine("ENTROOOOO");
    //                     string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
    //                    password: ClaveNueva,
    //                    salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
    //                    prf: KeyDerivationPrf.HMACSHA1,
    //                    iterationCount: 1000,
    //                    numBytesRequested: 256 / 8
    //                ));
    //                     usuario.Clave = hashed;
    //                 }
    //                 else
    //                 {
    //                     usuario.Clave = usuarioAnt.Clave;
    //                     TempData["AlertMessage"] = "La contraseña actual es incorrecta";
    //                     TempData["AlertType"] = "error";
    //                 }


    //             }


    //         }

    //         if (usuario.AvatarFile != null)
    //         {
    //             string wwwPath = environment.WebRootPath;
    //             string path = Path.Combine(wwwPath, "Uploads");
    //             if (!Directory.Exists(path))
    //             {
    //                 Directory.CreateDirectory(path);
    //             }
    //             string fileName = "avatar_" + usuario.Id + Path.GetExtension(usuario.AvatarFile.FileName);
    //             string pathCompleto = Path.Combine(path, fileName);
    //             usuario.Avatar = Path.Combine("/Uploads", fileName);
    //             using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
    //             {
    //                 usuario.AvatarFile.CopyTo(stream);
    //             }
    //         }
    //         else
    //         {
    //             usuario.Avatar = usuarioAnt.Avatar;
    //         }

    //         if (!User.IsInRole("Administrador"))
    //         {
    //             //vista = nameof(Perfil);
    //             var usuarioActual = _repo.GetUserByMail(User.Identity.Name);
    //             if (usuarioActual.Id != usuario.Id)
    //             {
    //                 return RedirectToAction(nameof(Index));
    //             }
    //         }
    //         var res = _repo.UpdateUsuario(usuario);
    //         ViewBag.Roles = _repo.GetEnumsTipes();
    //         return RedirectToAction(nameof(Index));
    //     }
    //     catch
    //     {
    //         throw;
    //     }
    // }
    [HttpPost]
    public ActionResult UpdateAvatar(Usuario usuario)
    {
        string returnUrl = Request.Headers["Referer"].ToString();
        Uri refererUri = new Uri(returnUrl);
        string relativePath = refererUri.GetComponents(UriComponents.PathAndQuery, UriFormat.SafeUnescaped);

        if (usuario.AvatarFile != null)
        {
            string wwwPath = environment.WebRootPath;
            string path = Path.Combine(wwwPath, "Uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fileName = "avatar_" + usuario.Id + "_" + Guid.NewGuid() + Path.GetExtension(usuario.AvatarFile.FileName);
            string pathCompleto = Path.Combine(path, fileName);
            usuario.Avatar = Path.Combine("/Uploads", fileName);

            using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
            {
                usuario.AvatarFile.CopyTo(stream);
            }

            _repo.UpdateAvatar(usuario);

            // Generar un identificador único para el parámetro de cache busting
            string cacheBuster = Guid.NewGuid().ToString("N");

            // Modificar la URL de la imagen para forzar la recarga
            string avatarUrlWithCacheBuster = usuario.Avatar + "?v=" + cacheBuster;

            usuario.Avatar = avatarUrlWithCacheBuster;

            TempData["AlertMessage"] = "Avatar modificado Correctamente";
            TempData["AlertType"] = "success";
        }

        return Redirect(relativePath);
    }


    [HttpPost]
    public ActionResult UpdateDatosLogueo(Usuario usuario, string ClaveActual, string ClaveNueva, string ClaveNuevaAgain)
    {
        var usuarioAnt = _repo.GetUserById(usuario.Id);
        string returnUrl = Request.Headers["Referer"].ToString();
        Uri refererUri = new Uri(returnUrl);
        string relativePath = refererUri.GetComponents(UriComponents.PathAndQuery, UriFormat.SafeUnescaped);
        if (ClaveNueva != ClaveNuevaAgain)
        {
            TempData["AlertMessage"] = "Las contraseñas no coinciden";
            TempData["AlertType"] = "error";
            return Redirect(relativePath);
        }

        var claveActual = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                               password: ClaveActual,
                               salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                               prf: KeyDerivationPrf.HMACSHA1,
                               iterationCount: 1000,
                               numBytesRequested: 256 / 8
                           ));
        if (ClaveNueva == null || ClaveNueva == "")
        {
            usuario.Clave = usuarioAnt.Clave;
        }
        else
        {

            if (claveActual == usuarioAnt.Clave)
            {
                Console.WriteLine("ENTROOOOO");
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
               password: ClaveNueva,
               salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
               prf: KeyDerivationPrf.HMACSHA1,
               iterationCount: 1000,
               numBytesRequested: 256 / 8
           ));
                usuario.Clave = hashed;
                TempData["AlertMessage"] = "La contraseña se cambio correctamente";
                TempData["AlertType"] = "success";
                _repo.UpdateUsuarioDatosLogueo(usuario);
            }
            else
            {
                TempData["AlertMessage"] = "La contraseña actual es incorrecta";
                TempData["AlertType"] = "error";
                return Redirect(relativePath);

            }

        }


        return Redirect(relativePath);



    }

    [HttpPost]
    public ActionResult UpdateDatosPersonales(Usuario usuario)
    {
        string returnUrl = Request.Headers["Referer"].ToString();
        Uri refererUri = new Uri(returnUrl);
        string relativePath = refererUri.GetComponents(UriComponents.PathAndQuery, UriFormat.SafeUnescaped);
        var usuarioAnt = _repo.GetUserById(usuario.Id);
        try
        {

            if (!User.IsInRole("Administrador"))
            {
                usuario.Rol = usuarioAnt.Rol;
                usuario.Estado = usuarioAnt.Estado;
            }

            var res = _repo.UpdateUsuarioDatosPersonales(usuario);
            ViewBag.Roles = _repo.getEnumRol();
            if (res < 0)
            {
                TempData["AlertMessage"] = "Error al modificar el usuario";
                TempData["AlertType"] = "error";
            }
            else
            {
                TempData["AlertMessage"] = "Usuario modificado correctamente.";
                TempData["AlertType"] = "success";
            }

            return Redirect(relativePath);
        }
        catch
        {
            throw;
        }
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Administrador")]
    public ActionResult Delete(int id)
    {
        try
        {
            Console.WriteLine("Id: " + id);
            var res = _repo.DeleteUsuario(id);
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }
    // public ActionResult Update(Usuario usuario)
    // {
    //     var usuarioAnt = _repo.GetUserById(usuario.Id);

    //     try
    //     {
    //         if (usuario.Clave == null || usuario.Clave == "")
    //         {
    //             usuario.Clave = usuarioAnt.Clave;
    //         }
    //         else
    //         {
    //             string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
    //                     password: usuario.Clave,
    //                     salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
    //                     prf: KeyDerivationPrf.HMACSHA1,
    //                     iterationCount: 1000,
    //                     numBytesRequested: 256 / 8
    //                 ));
    //             usuario.Clave = hashed;
    //         }

    //         if (usuario.AvatarFile != null)
    //         {
    //             string wwwPath = environment.WebRootPath;
    //             string path = Path.Combine(wwwPath, "Uploads");
    //             if (!Directory.Exists(path))
    //             {
    //                 Directory.CreateDirectory(path);
    //             }
    //             string fileName = "avatar_" + usuario.Id + Path.GetExtension(usuario.AvatarFile.FileName);
    //             string pathCompleto = Path.Combine(path, fileName);
    //             usuario.Avatar = Path.Combine("/Uploads", fileName);
    //             using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
    //             {
    //                 usuario.AvatarFile.CopyTo(stream);
    //             }
    //         }
    //         else
    //         {
    //             usuario.Avatar = usuarioAnt.Avatar;
    //         }

    //         if (!User.IsInRole("Administrador"))
    //         {
    //             //vista = nameof(Perfil);
    //             var usuarioActual = _repo.GetUserByMail(User.Identity.Name);
    //             if (usuarioActual.Id != usuario.Id)
    //             {
    //                 return RedirectToAction(nameof(Index));
    //             }
    //         }
    //         var res = _repo.UpdateUsuario(usuario);
    //         ViewBag.Roles = _repo.GetEnumsTipes();
    //         return RedirectToAction(nameof(Index));
    //     }
    //     catch
    //     {
    //         throw;
    //     }
    // }

public IActionResult Details(int id)
    {
        UsuariosRepository repo = new();
        var usuario = repo.GetUserById(id);
        return View(usuario);
    }
}