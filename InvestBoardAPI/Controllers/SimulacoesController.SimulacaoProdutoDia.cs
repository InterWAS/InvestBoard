using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InvestBoardAPI.Controllers
{
public partial class SimulacoesController
    {
        /// <summary>
        /// Representa os resultados da simulação para um produto específico em um determinado dia, incluindo o número de simulações realizadas e o valor médio final.
        /// </summary>
        /// <param name="Produto">O nome ou identificador do produto para o qual a simulação foi realizada. Não pode ser nulo.</param>
        /// <param name="Data">A data em que os resultados da simulação são válidos.</param>
        /// <param name="QuantidadeSimulacoes">O número total de simulações executadas para o produto e data especificados. Deve ser zero ou maior.</param>
        /// <param name="MediaValorFinal">O valor final médio calculado a partir de todas as simulações para o produto na data especificada.</param>
        public record SimulacaoProdutoDia(
            [property: Description("O nome ou identificador do produto para o qual a simulação foi realizada. Não pode ser nulo.")]
            string Produto,
            [property: Description("A data em que os resultados da simulação são válidos.")]
            DateOnly Data,
            [property: Description("O número total de simulações executadas para o produto e data especificados. Deve ser zero ou maior.")]
            int QuantidadeSimulacoes,
            [property: Description("O valor final médio calculado a partir de todas as simulações para o produto na data especificada.")]
            decimal MediaValorFinal);
    }
}
