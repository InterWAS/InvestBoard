using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InvestBoardAPI.Controllers
{
public partial class SimularInvestimentosController
    {
        /// <summary>
        /// Representa um produto validado com suas informações de identificação, classificação, rentabilidade e risco.
        /// </summary>
        /// <param name="ProdutoId">O identificador único do produto.</param>
        /// <param name="Nome">O nome do produto. Não pode ser nulo.</param>
        /// <param name="Tipo">O tipo ou categoria do produto. Não pode ser nulo.</param>
        /// <param name="Rentabilidade">O valor de rentabilidade do produto, expresso em decimal.</param>
        /// <param name="Risco">Classificação de risco do produto. Não pode ser nula.</param>
        public record ProdutoValidado(
            [property: Description("O identificador único do produto.")]
            int ProdutoId,
            [property: Description("O nome do produto. Não pode ser nulo.")]
            string Nome,
            [property: Description("O tipo ou categoria do produto. Não pode ser nulo.")]
            string Tipo,
            [property: Description("O valor de rentabilidade do produto, expresso em decimal.")]
            decimal Rentabilidade,
            [property: Description("Classificação de risco do produto. Não pode ser nula.")]
            string Risco);
    }
}
