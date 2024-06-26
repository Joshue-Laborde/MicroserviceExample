using Microsoft.AspNetCore.Authentication.Cookies;
using System.Reflection;

ComonStartup.Assembly = Assembly.GetExecutingAssembly();
var app = ComonStartup.Create(builder =>
{
    // Configurar autenticación por cookies
    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/auth/login";
            options.LogoutPath = "/auth/logout";
            options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            options.SlidingExpiration = true;
        });

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("SessionPolicy", policy =>
            policy.RequireAuthenticatedUser()
                  .RequireAssertion(context =>
                      context.User.Identity.IsAuthenticated &&
                      context.User.Identity.Name != null)); // Verifica que el nombre de usuario esté presente en la sesión
    });

    // Configurar servicios de sesión
    builder.Services.AddDistributedMemoryCache();
    builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(30);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });

    // Configurar CORS
    //builder.Services.AddCors(options =>
    //{
    //    options.AddPolicy("AllowSpecificOrigin",
    //        builder =>
    //        {
    //            builder.WithOrigins("http://localhost:64927")
    //                   .AllowAnyMethod()
    //                   .AllowAnyHeader();
    //        });
    //});

    //builder.Services.AddOptions();
    builder.Services.ConfigureAppService();
    builder.Services.ConfigureDataService();
    builder.Services.ConfigureMappingProfile();
    builder.ConfigureDatabase();
});

//app.UseRouting();
//app.UseCors("AllowSpecificOrigin");
// Configurar middleware de autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// Configurar middleware de sesión
app.UseSession();
app.RunStart();
