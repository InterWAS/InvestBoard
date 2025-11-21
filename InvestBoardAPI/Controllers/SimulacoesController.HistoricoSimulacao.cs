using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InvestBoardAPI.Controllers
{
    public partial class SimulacoesController
    {
        /// <summary>
        /// Representa o registro histórico de uma simulação de investimento, incluindo detalhes sobre o cliente, produto,
        /// valor investido, valor final, período de simulação e data.
        /// </summary>
        /// <param name="SimulacaoId">O identificador único para o registro da simulação.</param>
        /// <param name="ClienteId">O identificador do cliente associado à simulação.</param>
        /// <param name="Produto">Nome do produto de investimento usado na simulação. Não pode ser nulo.</param>
        /// <param name="ValorInvestido">O montante de dinheiro inicialmente investido na simulação.</param>
        /// <param name="ValorFinal">O valor final do investimento ao término do período de simulação.</param>
        /// <param name="PrazoMeses">A duração da simulação, em meses. Deve ser maior que zero.</param>
        /// <param name="DataSimulacao">A data e a hora em que a simulação foi realizada.</param>
        public record HistoricoSimulacao(
            [property: Description("O identificador único para o registro da simulação.")]
            int SimulacaoId,
            [property: Description("O identificador do cliente associado à simulação.")]
            int ClienteId,
            [property: Description("Nome do produto de investimento usado na simulação. Não pode ser nulo.")]
            string Produto,
            [property: Description("O montante de dinheiro inicialmente investido na simulação.")]
            decimal ValorInvestido,
            [property: Description("O valor final do investimento ao término do período de simulação.")]
            decimal ValorFinal,
            [property: Description("A duração da simulação, em meses. Deve ser maior que zero.")]
            int PrazoMeses,
            [property: Description("A data e a hora em que a simulação foi realizada.")]
            DateOnly DataSimulacao);
    }
}
