using FinancasApp.Data.Repositories;
using FinancasApp.Presentation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Globalization;

namespace FinancasApp.Presentation.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        /// <summary>
        /// Método para abrir a página /Home/Index
        /// </summary>
        public IActionResult Index()
        {
            var model = new DashboardViewModel();

            try
            {
                //capturando a data do primeiro e ultimo dia do mês atual
                var dataAtual = DateTime.Now;
                model.DataInicio = new DateTime(dataAtual.Year, dataAtual.Month, 1);
                model.DataFim = model.DataInicio.Value.AddMonths(1).AddDays(-1);

                //capturar o usuário autenticado na aplicação
                var usuario = JsonConvert.DeserializeObject<AuthenticationViewModel>(User.Identity.Name);

                //consultar as contas deste período
                var contaRepository = new ContaRepository();
                var contas = contaRepository.GetAll(model.DataInicio.Value, model.DataFim.Value, usuario.Id);

                //total de contas a receber e a pagar
                model.DonutChart = contas
                                    .GroupBy(group => group.Tipo)
                                    .Select(g => new DadosGrafico
                                    {
                                        Name = g.Key.ToString(),
                                        Data = g.Sum(conta => conta.Valor)
                                            .ToString("F2", CultureInfo.InvariantCulture)
                                    }).ToList();

                //total de contas por categoria
                model.ColumnChart = contas
                                    .GroupBy(group => group.Categoria.Nome)
                                    .Select(g => new DadosGrafico
                                    {
                                        Name = g.Key.ToString(),
                                        Data = g.Sum(conta => conta.Valor)
                                            .ToString("F2", CultureInfo.InvariantCulture)
                                    }).ToList();
            }
            catch(Exception e)
            {
                TempData["MensagemErro"] = e.Message;
            }

            return View(model);
        }
    }
}
