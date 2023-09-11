using FinancasApp.Data.Entities;
using FinancasApp.Data.Helpers;
using FinancasApp.Data.Repositories;
using FinancasApp.Presentation.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Newtonsoft.Json;
using System.Security.Claims;

namespace FinancasApp.Presentation.Controllers
{
    /// <summary>
    /// Classe de controle do Asp.Net MVC
    /// </summary>
    public class AccountController : Controller
    {
        /// <summary>
        /// Método para abrir a página /Account/Login
        /// </summary>
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Método para receber o submit POST da página /Account/Login
        /// </summary>
        [HttpPost]
        public IActionResult Login(AccountLoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    //consultando no banco de dados o usuário através do email e da senha
                    var usuarioRepository = new UsuarioRepository();
                    var usuario = usuarioRepository.GetByEmailAndSenha(model.Email, SHA1Helper.Encrypt(model.Senha));

                    //verificando se o usuário foi encontrado
                    if(usuario != null)
                    {
                        //armazenar os dados do usuário autenticado
                        var authenticationViewModel = new AuthenticationViewModel
                        {
                            Id = usuario.Id,
                            Nome = usuario.Nome,
                            Email = usuario.Email,
                            DataHoraAcesso = DateTime.Now
                        };

                        //serializando os dados para JSON
                        var data = JsonConvert.SerializeObject(authenticationViewModel);

                        //criando a identificação do usuário no Asp.Net MVC
                        //preparando-o para ser gravado em um arquivo de Cookie
                        var claimsIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, data) },
                           CookieAuthenticationDefaults.AuthenticationScheme);

                        //gravando o Cookie de autenticação do Asp.Net
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                        //redirecionando para a página /Home/Index
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["MensagemAlerta"] = "Acesso negado. Usuário inválido.";
                    }
                }
                catch(Exception e)
                {
                    TempData["MensagemErro"] = e.Message;
                }
            }
            else
            {
                TempData["MensagemAlerta"] = "Ocorreram erros de validação no preenchimento do formulário, por favor verifique.";
            }

            return View();
        }

        /// <summary>
        /// Método para abrir a página /Account/Register
        /// </summary>
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Método para receber o submit POST da página /Account/Register
        /// </summary>
        [HttpPost]
        public IActionResult Register(AccountRegisterViewModel model)
        {
            //verificar se todos os campos enviados passaram
            //nas regras de validação do DataAnnotations
            if(ModelState.IsValid)
            {
                try
                {
                    var usuarioRepository = new UsuarioRepository();

                    //verificar se já existe um usuário cadastrado com o emaio informado
                    if(usuarioRepository.GetByEmail(model.Email) != null)
                    {
                        TempData["MensagemAlerta"] = "O email informado já está cadastrado para outro usuário.";
                    }
                    else
                    {
                        //capturando os dados do usuário
                        var usuario = new Usuario
                        {
                            Id = Guid.NewGuid(),
                            Nome = model.Nome,
                            Email = model.Email,
                            Senha = SHA1Helper.Encrypt(model.Senha),
                            DataHoraCriacao = DateTime.Now
                        };

                        //gravar o usuário no banco de dados                    
                        usuarioRepository.Create(usuario);

                        TempData["MensagemSucesso"] = $"Parabéns {usuario.Nome}, sua conta de usuário foi criada com sucesso.";
                        ModelState.Clear();
                    }                    
                }
                catch(Exception e)
                {
                    TempData["MensagemErro"] = e.Message;
                }
            }
            else
            {
                TempData["MensagemAlerta"] = "Ocorreram erros de validação no preenchimento do formulário, por favor verifique.";
            }

            return View();
        }

        /// <summary>
        /// Método para abrir a página /Account/ForgotPassword
        /// </summary>
        public IActionResult ForgotPassword()
        {
            return View();
        }

        /// <summary>
        /// Método para deslogar o usuário do sistema /Account/Logout
        /// </summary>
        public IActionResult Logout()
        {
            //apagar o cookie de autenticação gravado no navegador
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            //apagar todos os dados salvos em sessão
            HttpContext.Session.Clear();

            //redirecionar o usuário para a página de login
            return RedirectToAction("Login");
        }
    }
}
