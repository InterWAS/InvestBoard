using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InvestBoardAPI.Controllers
{
public partial class SimularInvestimentosController
    {
        /// <summary>
        /// Representa as informações do tipo de produto utilizadas em uma solicitação de simulação para um cliente, 
        /// incluindo o identificador do cliente, o valor do produto, o prazo em meses e o tipo de produto.
        /// </summary>
        /// <param name="ClienteId">O identificador único do cliente para quem a simulação é realizada.</param>
        /// <param name="Valor">O valor monetário do produto que está sendo simulado.</param>
        /// <param name="PrazoMeses">A duração da simulação, em meses.</param>
        /// <param name="TipoProduto">O tipo ou categoria do produto que está sendo simulado. Não pode ser nulo.</param>
        public record PedidoSimulacaoTipoProduto(
            [property: Description("O identificador único do cliente para quem a simulação é realizada.")]
            int ClienteId,
            [property: Description("O valor monetário do produto que está sendo simulado.")]
            decimal Valor,
            [property: Description("A duração da simulação, em meses.")]
            int PrazoMeses,
            [property: Description("O tipo ou categoria do produto que está sendo simulado. Não pode ser nulo.")]
            string TipoProduto);
    }
}
