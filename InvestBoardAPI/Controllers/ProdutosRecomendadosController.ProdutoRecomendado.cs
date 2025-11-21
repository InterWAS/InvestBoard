using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InvestBoardAPI.Controllers
{
    public partial class ProdutosRecomendadosController
    {
        /// <summary>
        /// Representa um produto financeiro recomendado, incluindo seu identificador, nome, tipo, rentabilidade em 12 meses e classificação de risco.
        /// </summary>
        /// <param name="ProdutoId">O identificador único do produto recomendado.</param>
        /// <param name="Nome">O nome do produto recomendado.</param>
        /// <param name="Tipo">O tipo ou categoria do produto recomendado.</param>
        /// <param name="Rentabilidade12Meses">A rentabilidade do produto nos últimos 12 meses, expressa em valor decimal.</param>
        /// <param name="Risco">Classificação de risco associada ao produto recomendado.</param>
        public record ProdutoRecomendado(
            [property: Description("O identificador único do produto recomendado.")]
            int ProdutoId,
            [property: Description("O nome do produto recomendado.")]
            string Nome,
            [property: Description("O tipo ou categoria do produto recomendado.")]
            string Tipo,
            [property: Description("A rentabilidade do produto nos últimos 12 meses, expressa em valor decimal.")]
            decimal Rentabilidade12Meses,
            [property: Description("Classificação de risco associada ao produto recomendado.")]
            string Risco);

    }
}
