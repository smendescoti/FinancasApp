using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace FinancasApp.Presentation.Models
{
    /// <summary>
    /// Modelo de dados do formulário da página /Contas/Cadastro
    /// </summary>
    public class ContasCadastroViewModel
    {
        [MaxLength(150, ErrorMessage = "Por favor, informe no máximo {1} caracteres.")]
        [MinLength(8, ErrorMessage = "Por favor, informe no mínimo {1} caracteres.")]
        [Required(ErrorMessage = "Por favor, informe o nome da conta.")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "Por favor, informe a data da conta.")]
        public DateTime? Data { get; set; }

        [Required(ErrorMessage = "Por favor, informe o valor da conta.")]
        public decimal? Valor { get; set; }

        [MaxLength(250, ErrorMessage = "Por favor, informe no máximo {1} caracteres.")]
        [MinLength(8, ErrorMessage = "Por favor, informe no mínimo {1} caracteres.")]
        [Required(ErrorMessage = "Por favor, informe as observações da conta.")]
        public string? Observacoes { get; set; }

        [Required(ErrorMessage = "Por favor, selecione o tipo da conta.")]
        public int? Tipo { get; set; }

        [Required(ErrorMessage = "Por favor, selecione a categoria da conta.")]
        public Guid? CategoriaId { get; set; }

        /// <summary>
        /// Propriedade para popular o campo DropDownList de seleção de categorias
        /// </summary>
        public List<SelectListItem>? ListagemCategorias { get; set; }
    }
}
