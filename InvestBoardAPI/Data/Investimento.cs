using System.ComponentModel.DataAnnotations;

namespace InvestBoardAPI.Data
{
    /// <summary>
    /// Representa um investimento realizado por um cliente, incluindo detalhes sobre o cliente, o produto associado e os valores do investimento.
    /// </summary>
    /// <remarks>
    /// A classe Investimento engloba informações sobre uma única transação de investimento, vinculando o cliente e o produto envolvido, 
    /// bem como detalhes financeiros e de data. Esse tipo de dado é normalmente usado para rastrear e gerenciar registros de investimentos em aplicativos financeiros.
    /// </remarks>
    public class Investimento
    {

        [Display(Name = "ID Investimento")]
        public int InvestimentoId { get; set; }

        /// <summary>
        /// Identificador do cliente que realizou o investimento
        /// </summary>
        [Display(Name = "ID Cliente")]
        public int ClienteId { get; set; }

        /// <summary>
        /// Cliente que realizou o investimento
        /// </summary>
        public virtual Cliente? Cliente { get; set; }

        /// <summary>
        /// Identificador do produto investido
        /// </summary>
        [Display(Name = "ID Produto")]
        public int ProdutoId { get; set; }

        /// <summary>
        /// Produto associado ao investimento
        /// </summary>
        public virtual Produto? Produto { get; set; }

        /// <summary>
        /// Valor inicial investido
        /// </summary>
        [Display(Name = "Valor inicial investido")]
        public decimal Valor { get; set; }

        /// <summary>
        /// Rentabilidade obtida no investimento
        /// </summary>
        [Display(Name = "Rentabilidade")]
        public decimal Rentabilidade { get; set; }

        /// <summary>
        /// Data em que o investimento foi realizado
        /// </summary>
        [Display(Name = "Data do investimento")]
        public DateTime Data { get; set; }

    }
}
