using FinancasApp.Data.Entities;
using FinancasApp.Data.Enums;
using FinancasApp.Data.Repositories;
using FinancasApp.Presentation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace FinancasApp.Presentation.Controllers
{
    [Authorize]
    public class ContasController : Controller
    {
        //Contas/Cadastro
        public IActionResult Cadastro()
        {
            //ao abrir a página de cadastro, precisamos enviar um objeto viewmodel
            //contendo a lista de categorias que será exibida pelo campo DropDownList
            var model = new ContasCadastroViewModel();
            model.ListagemCategorias = ObterCategorias();

            //enviando o objeto viewmodel para a página
            return View(model);
        }

        //Contas/Cadastro
        [HttpPost]
        public IActionResult Cadastro(ContasCadastroViewModel model)
        {
            //verificando se todos os campos passaram nas validações
            if(ModelState.IsValid)
            {
                try
                {
                    //capturar o usuário autenticado na aplicação
                    var usuario = JsonConvert.DeserializeObject<AuthenticationViewModel>(User.Identity.Name);

                    //criando um objeto Conta
                    var conta = new Conta
                    {
                        Id = Guid.NewGuid(),
                        Nome = model.Nome,
                        Data = model.Data.Value,
                        Valor = model.Valor.Value,
                        Observacoes = model.Observacoes,
                        Tipo = (TipoConta) model.Tipo,
                        CategoriaId = model.CategoriaId.Value, //foreign key
                        UsuarioId = usuario.Id //foreign key
                    };

                    //gravar a conta no banco de dados
                    var contaRepository = new ContaRepository();
                    contaRepository.Create(conta);

                    TempData["MensagemSucesso"] = "Conta cadastrada com sucesso.";
                    ModelState.Clear(); //limpar o formulário
                    model = new ContasCadastroViewModel();
                }
                catch(Exception e)
                {
                    TempData["MensagemErro"] = e.Message;
                }
            }
            else
            {
                TempData["MensagemAlerta"] = "Ocorreram erros de validação no preenchimento do formulário.";
            }

            //ao retornar para a página de cadastro, precisamos enviar um objeto viewmodel
            //contendo a lista de categorias que será exibida pelo campo DropDownList
            model.ListagemCategorias = ObterCategorias();
            //enviando o objeto viewmodel para a página
            return View(model);
        }

        //Contas/Consulta
        public IActionResult Consulta()
        {
            try
            {
                //verificando se há datas selecionadas em sessão
                var dataInicio = HttpContext.Session.GetString("DataInicio");
                var dataFim = HttpContext.Session.GetString("DataFim");

                if(dataInicio != null && dataFim != null)
                {
                    var model = new ContasConsultaViewModel();
                    model.DataInicio = DateTime.Parse(dataInicio);
                    model.DataFim = DateTime.Parse(dataFim);

                    //capturar o usuário autenticado na aplicação
                    var usuario = JsonConvert.DeserializeObject<AuthenticationViewModel>(User.Identity.Name);

                    var contaRepository = new ContaRepository();
                    model.ListagemContas = contaRepository.GetAll
                        (model.DataInicio.Value, model.DataFim.Value, usuario.Id);

                    return View(model);
                }
            }
            catch(Exception e)
            {
                TempData["MensagemErro"] = e.Message;
            }

            return View();
        }

        //Contas/Consulta
        [HttpPost]
        public IActionResult Consulta(ContasConsultaViewModel model)
        {
            //verificando se os campos do formulário passaram nas validações
            if(ModelState.IsValid)
            {
                try
                {
                    //capturar o usuário autenticado na aplicação
                    var usuario = JsonConvert.DeserializeObject<AuthenticationViewModel>(User.Identity.Name);

                    //executar a consulta no banco de dados
                    var contaRepository = new ContaRepository();
                    model.ListagemContas = contaRepository.GetAll
                        (model.DataInicio.Value, model.DataFim.Value, usuario.Id);

                    //armazenar as datas selecionadas em uma sessão
                    HttpContext.Session.SetString("DataInicio", model.DataInicio.ToString());
                    HttpContext.Session.SetString("DataFim", model.DataFim.ToString());
                }
                catch(Exception e)
                {
                    TempData["MensagemErro"] = e.Message;
                }
            }
            else
            {
                TempData["MensagemAlerta"] = "Ocorreram erros no preenchimento do formulário.";
            }

            //retornando a model para a página
            return View(model);
        }

        //Contas/Edicao
        public IActionResult Edicao(Guid id)
        {
            var model = new ContasEdicaoViewModel();
            model.ListagemCategorias = ObterCategorias();

            try
            {
                //procurar a conta no banco de dados através do ID
                var contaRepository = new ContaRepository();
                var conta = contaRepository.GetById(id);

                //preencher a classe viewmodel com os dados da conta
                model.Id = conta.Id;
                model.Nome = conta.Nome;
                model.Data = conta.Data;
                model.Valor = conta.Valor;
                model.Observacoes = conta.Observacoes;
                model.Tipo = (int) conta.Tipo;
                model.CategoriaId = conta.CategoriaId;
            }
            catch(Exception e)
            {
                TempData["MensagemErro"] = e.Message;
            }

            //enviando a viewmodel para a página
            return View(model);
        }

        [HttpPost] //método para receber o SUBMIT POST do formulário
        public IActionResult Edicao(ContasEdicaoViewModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var contaRepository = new ContaRepository();
                    var conta = contaRepository.GetById(model.Id);

                    conta.Nome = model.Nome;
                    conta.Data = model.Data.Value;
                    conta.Valor = model.Valor.Value;
                    conta.Observacoes = model.Observacoes;
                    conta.Tipo = (TipoConta) model.Tipo;
                    conta.CategoriaId = model.CategoriaId.Value;

                    contaRepository.Update(conta);

                    TempData["MensagemSucesso"] = "Conta atualizada com sucesso.";
                }
                catch(Exception e)
                {
                    TempData["MensagemErro"] = e.Message;
                }
            }
            else
            {
                TempData["MensagemAlerta"] = "Ocorreram erros de validação no preenchimento do formulário.";
            }

            model.ListagemCategorias = ObterCategorias();
            return View(model);
        }

        //método para capturar a exclusão da conta
        public IActionResult Exclusao(Guid id)
        {
            try
            {
                var contaRepository = new ContaRepository();
                var conta = contaRepository.GetById(id);

                //excluindo a conta
                contaRepository.Delete(conta);

                TempData["MensagemSucesso"] = "Conta excluída com sucesso.";
            }
            catch(Exception e)
            {
                TempData["MensagemErro"] = e.Message;
            }

            //redirecionar para a página de consulta
            return RedirectToAction("Consulta");
        }

        //método para gerar a lista de seleção de categorias
        private List<SelectListItem> ObterCategorias()
        {
            var lista = new List<SelectListItem>();

            //consultando todas as categorias do banco de dados
            var categoriaRepository = new CategoriaRepository();
            foreach (var item in categoriaRepository.GetAll())
            {
                lista.Add(new SelectListItem 
                { 
                    Value = item.Id.ToString(), //valor do campo
                    Text = item.Nome //texto exibido no campo
                });
            }

            return lista;
        }
    }
}
