
using Inmobiliaria.Controllers;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
  options =>
  {
    options.LoginPath = "/Usuarios/login";
    options.LogoutPath = "/Usuarios/logout";
    options.AccessDeniedPath = "/Home/Privacy";
  }  
);
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Empleado",policy => policy.RequireRole("Empleado","Administrador"));
    options.AddPolicy("Administrador",policy => policy.RequireRole("Administrador"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


// app.UseMiddleware<MiddlewareExpiracionContratos>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
 

app.Run();

