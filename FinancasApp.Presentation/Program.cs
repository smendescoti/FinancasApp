using FinancasApp.Presentation.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

/*
 * Definindo a pol�tica de autentica��o do projeto.
 * Neste projeto ser� feito por CookieAuthentication
 */
builder.Services.Configure<CookiePolicyOptions>
    (options => { options.MinimumSameSitePolicy = SameSiteMode.None; });
builder.Services.AddAuthentication
    (CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

//Habilitando o uso de sess�es no projeto
builder.Services.AddSession();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseMiddleware<CacheFilter>();

app.UseCookiePolicy(); //habilitando Cookies
app.UseAuthentication(); //habilitando Autentica��o
app.UseAuthorization(); //habilitando Autoriza��o

//Habilitando o uso de sess�es no projeto
app.UseSession();

/*
 * Definindo o padr�o de navega��o do projeto /Controller/View
 * e a p�gina inicial que ser� aberta ao acessar o sistema
 */
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
