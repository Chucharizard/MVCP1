using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<WAMVC.Data.ArtesaniasDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuración de autenticación con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(2);
        options.SlidingExpiration = true;
    });

// Configuración de autorización con políticas
builder.Services.AddAuthorization(options =>
{
    // Política para Administradores
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));

    // Política para Empleados
    options.AddPolicy("EmpleadoOnly", policy =>
        policy.RequireRole("Empleado"));

    // Política para Clientes
    options.AddPolicy("ClienteOnly", policy =>
        policy.RequireRole("Cliente"));

    // Política para Admin o Empleado
    options.AddPolicy("AdminOrEmpleado", policy =>
        policy.RequireRole("Admin", "Empleado"));

    // Política que requiere estar autenticado
    options.AddPolicy("Authenticated", policy =>
        policy.RequireAuthenticatedUser());
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

app.UseAuthentication();  // ← NO OLVIDES ESTO
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
