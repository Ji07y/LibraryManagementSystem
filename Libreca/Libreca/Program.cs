using Microsoft.EntityFrameworkCore;
using Libreca.Data;
using NotificationService;
using Libreca.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor
builder.Services.AddControllersWithViews();

// Configurar el cliente HTTP para el microservicio de búsqueda de libros
builder.Services.AddHttpClient<BookSearchClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5299/");
});

// Configurar servicios adicionales
builder.Services.AddSingleton(sp => new EmailService("localhost", "emailQueue"));
builder.Services.AddSingleton(sp => new RabbitMQService("localhost"));

// Configurar la conexión a la base de datos
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar la autenticación con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
    });

var app = builder.Build();

// Iniciar el servicio de correo electrónico
var emailService = app.Services.GetRequiredService<EmailService>();
emailService.Start();

// Configurar el pipeline de HTTP
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
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
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
