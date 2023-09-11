using FinancasApp.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace FinancasApp.Presentation.Models
{
    /// <summary>
    /// Modelo de dados do formulário da página /Contas/Consulta
    /// </summary>
    public class ContasConsultaViewModel
    {
        [Required(ErrorMessage = "Por favor, informe a data de início.")]
        public DateTime? DataInicio { get; set; }

        [Required(ErrorMessage = "Por favor, informe a data de término.")]
        public DateTime? DataFim { get; set; }

        /// <summary>
        /// Listagem de contas exibida na página
        /// após a realização da pesquisa
        /// </summary>
        public List<Conta>? ListagemContas { get; set; }
    }
}
