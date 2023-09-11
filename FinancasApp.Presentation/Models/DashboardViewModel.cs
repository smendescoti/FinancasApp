using System.ComponentModel.DataAnnotations;

namespace FinancasApp.Presentation.Models
{
    /// <summary>
    /// Modelo de dados da página de dashboard
    /// </summary>
    public class DashboardViewModel
    {
        [Required(ErrorMessage = "Por favor, informe a data de início.")]
        public DateTime? DataInicio { get; set; }

        [Required(ErrorMessage = "Por favor, informe a data de término.")]
        public DateTime? DataFim { get; set; }

        public List<DadosGrafico>? DonutChart { get; set; }
        public List<DadosGrafico>? ColumnChart { get; set; }
    }

    public class DadosGrafico
    {
        public string Name { get; set; }
        public string Data { get; set; }
    }
}
