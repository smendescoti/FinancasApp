using FinancasApp.Data.Entities;
using FinancasApp.Data.Helpers;
using FinancasApp.Data.Repositories;
using FinancasApp.Presentation.Models;
using Microsoft.AspNetCore.Mvc;

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
    }
}
