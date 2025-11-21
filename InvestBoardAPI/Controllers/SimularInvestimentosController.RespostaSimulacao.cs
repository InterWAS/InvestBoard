using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InvestBoardAPI.Controllers
{
public partial class SimularInvestimentosController
    {
        /// <summary>
        /// Representa o resultado de uma simulação, incluindo o produto validado, o resultado da simulação 
        /// e a data em que a simulação foi realizada.
        /// </summary>
        /// <param name="ProdutoValidado">
        /// O produto que foi validado antes da simulação. Contém informações sobre a elegibilidade e o status de validação do produto.
        /// </param>
        /// <param name="ResultadoSimulacao">O resultado da simulação, incluindo os valores calculados e quaisquer resultados relevantes.</param>
        /// <param name="DataSimulacao">A data e a hora em que a simulação foi executada.</param>
        public record RespostaSimulacao(
            [property: Description("O produto que foi validado antes da simulação. Contém informações sobre a elegibilidade e o status de validação do produto.")]
            ProdutoValidado ProdutoValidado,
            [property: Description("O resultado da simulação, incluindo os valores calculados e quaisquer resultados relevantes.")]
            ResultadoSimulacao ResultadoSimulacao,
            [property: Description("A data e a hora em que a simulação foi executada.")]
            DateTime DataSimulacao);
    }
}
