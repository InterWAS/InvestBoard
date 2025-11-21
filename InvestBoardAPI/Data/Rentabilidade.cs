using System.ComponentModel.DataAnnotations;

namespace InvestBoardAPI.Data
{
    /// <summary>
    /// Representa as informações de rentabilidade de um produto, incluindo a faixa de investimento e o percentual de retorno nos últimos 12 meses
    /// </summary>
    /// <remarks>
    /// Classe usada para associar dados de rentabilidade a um produto específico e a uma faixa de investimento. 
    /// Ela pode ser usada todos os tipos de aplicações financeiras (CDB, LCI, Fundos,...) para exibir ou calcular retornos com base em valores investidos. 
    /// Todas as propriedades são necessárias para uma representação completa de um registro de rentabilidade.
    /// Deve existir ao menos uma rentabilidade associada a cada produto. Pode haver múltiplas rentabilidades para diferentes faixas de investimento dentro do mesmo produto.
    /// </remarks>
    public class Rentabilidade
    {
        /// <summary>
        /// Identificador da rentabilidade
        /// </summary>
        [Key]
        [Display(Name = "ID Rentabilidade")]
        public int RentabilidadeId { get; set; }

        /// <summary>
        /// Identificador do produto associado
        /// </summary>
        [Display(Name = "ID Produto")]
        public int ProdutoId { get; set; }

        /// <summary>
        /// Valor de rentabilidade dos últimos 12 meses, expresso em percentual
        /// </summary>
        [Display(Name = "Rentabilidade (%) 12 meses")]
        public decimal Valor { get; set; }

        /// <summary>
        /// Início da faixa de aplicação para a qual esta rentabilidade é aplicável. Valor mínimo de investimento
        /// </summary>
        [Display(Name = "Faixa Inicial")]
        public decimal FaixaInicial { get; set; }

        /// <summary>
        /// Final da faixa de aplicação para a qual esta rentabilidade é aplicável. Valor máximo de investimento
        /// </summary>
        [Display(Name = "Faixa Final")]
        public decimal FaixaFinal { get; set; }
    }
}
