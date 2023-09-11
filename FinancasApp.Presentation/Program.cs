using FinancasApp.Presentation.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

/*
 * Definindo a política de autenticação do projeto.
 * Neste projeto será feito por CookieAuthentication
 */
builder.Services.Configure<CookiePolicyOptions>
    (options => { options.MinimumSameSitePolicy = SameSiteMode.None; });
builder.Services.AddAuthentication
    (CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

//Habilitando o uso de sessões no projeto
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
app.UseAuthentication(); //habilitando Autenticação
app.UseAuthorization(); //habilitando Autorização

//Habilitando o uso de sessões no projeto
app.UseSession();

/*
 * Definindo o padrão de navegação do projeto /Controller/View
 * e a página inicial que será aberta ao acessar o sistema
 */
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
