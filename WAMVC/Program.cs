using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<WAMVC.Data.ArtesaniasDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(2);
        options.SlidingExpiration = true;
    });

// Config de auth con pol
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

 
    options.AddPolicy("Authenticated", policy =>
        policy.RequireAuthenticatedUser());
});

var app = builder.Build();


//usuario por defecto juas juas
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<WAMVC.Data.ArtesaniasDBContext>();

    if (!context.Usuarios.Any())
    {
        context.Usuarios.Add(new WAMVC.Models.Usuario
        {
            Email = "admin@admin.com",
            Password = BCrypt.Net.BCrypt.HashPassword("admin123"),
            Rol = "Admin",
            Activo = true
        });

        context.Usuarios.Add(new WAMVC.Models.Usuario
        {
            Email = "usuario@test.com",
            Password = BCrypt.Net.BCrypt.HashPassword("123456"),
            Rol = "Usuario",
            Activo = true
        });

        context.SaveChanges();
    }
}



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();  
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
