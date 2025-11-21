using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InvestBoardAPI.Controllers
{
public partial class SimularInvestimentosController
    {
        /// <summary>
        /// Representa o resultado de uma simulação financeira, incluindo o valor final, 
        /// a rentabilidade efetiva e o prazo de investimento em meses.
        /// </summary>
        /// <param name="ValorFinal">O valor final obtido na simulação, expresso em unidades monetárias.</param>
        /// <param name="RentabilidadeEfetiva">A taxa de rentabilidade efetiva alcançada durante a simulação, representada como um valor decimal.</param>
        /// <param name="PrazoMeses">A duração total do investimento ou da simulação, medida em meses.</param>
        public record ResultadoSimulacao(
            [property: Description("O valor final obtido na simulação, expresso em unidades monetárias.")]
            decimal ValorFinal,
            [property: Description("A taxa de rentabilidade efetiva alcançada durante a simulação, representada como um valor decimal.")]
            decimal RentabilidadeEfetiva,
            [property: Description("A duração total do investimento ou da simulação, medida em meses.")]
            int PrazoMeses);
    }
}
