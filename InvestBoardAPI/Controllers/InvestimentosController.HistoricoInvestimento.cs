using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InvestBoardAPI.Controllers
{
    public partial class InvestimentosController
    {
        /// <summary>
        /// Representa um registro de uma transação de investimento, incluindo seu tipo, valor, rendimento e data.
        /// </summary>
        /// <param name="Id">O identificador único do registro de investimento.</param>
        /// <param name="Tipo">O tipo ou categoria do investimento, como 'Renda Fixa' ou 'Ações'.</param>
        /// <param name="Valor">O valor monetário da transação de investimento.</param>
        /// <param name="Rentabilidade">O rendimento ou retorno associado ao investimento, expresso em valor decimal.</param>
        /// <param name="Data">A data em que ocorreu a transação de investimento.</param>
        public record HistoricoInvestimento(
            [property: Description("O identificador único do registro de investimento.")]
            int Id, 
            [property: Description("O tipo ou categoria do investimento, como 'Renda Fixa' ou 'Ações'.")]
            string Tipo, 
            [property: Description("O valor monetário da transação de investimento.")]
            decimal Valor, 
            [property : Description("O rendimento ou retorno associado ao investimento, expresso em valor decimal")] 
            decimal Rentabilidade, 
            [property: Description("A data em que ocorreu a transação de investimento.")]
            DateOnly Data);
    }
}
