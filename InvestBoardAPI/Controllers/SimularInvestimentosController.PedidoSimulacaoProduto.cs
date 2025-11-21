using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InvestBoardAPI.Controllers
{
public partial class SimularInvestimentosController
    {
        /// <summary>
        /// Representa uma solicitação de simulação de produto para um cliente, 
        /// incluindo detalhes do produto, valor solicitado e condições de pagamento.
        /// </summary>
        /// <param name="ClienteId">O identificador único do cliente para quem a simulação do produto foi solicitada.</param>
        /// <param name="Valor">O valor monetário, na moeda relevante, a ser simulado para o produto.</param>
        /// <param name="PrazoMeses">O número de meses para o qual o pagamento ou a simulação deve ser calculado. Deve ser um número inteiro positivo.</param>
        /// <param name="ProdutoId">O identificador único do produto a ser simulado.</param>
        public record PedidoSimulacaoProduto(
            [property: Description("O identificador único do cliente para quem a simulação do produto foi solicitada.")]
            int ClienteId,
            [property: Description("O valor monetário, na moeda relevante, a ser simulado para o produto.")]
            decimal Valor,
            [property: Description("O número de meses para o qual o pagamento ou a simulação deve ser calculado. Deve ser um número inteiro positivo..")]
            int PrazoMeses,
            [property: Description("O identificador único do produto a ser simulado.")]
            int ProdutoId);
    }
}
