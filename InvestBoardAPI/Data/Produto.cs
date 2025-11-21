using System.ComponentModel.DataAnnotations;

namespace InvestBoardAPI.Data
{

    /// <summary>
    /// Representa um produto financeiro com detalhes como nome, tipo, risco e rentabilidades associadas
    /// </summary>
    /// <remarks>
    /// Classe para definir produtos financeiros, incluindo suas características e associações com rentabilidades. 
    /// O risco do produto deve estar dentro do intervalo válido para evitar erros.
    /// </remarks>
    public class Produto
    {
        /// <summary>
        /// Identificador único do produto
        /// </summary>
        [Key]
        [Display(Name = "ID Produto")]
        public int ProdutoId { get; set; }

        /// <summary>
        /// Nome do produto
        /// </summary>
        [Display(Name = "Nome Produto")]
        [MaxLength(50)]
        [StringLength(50)]
        public required string Nome { get; set; }

        /// <summary>
        /// Tipos de produtos financeiros disponíveis
        /// </summary>
        public enum TipoProduto
        {
            CDB,
            RDB,
            LCI,
            LCA,
            Fundo
        }

        /// <summary>
        /// Tipo do produto financeiro
        /// </summary>
        [Display(Name = "Tipo de Produto")]
        public TipoProduto Tipo { get; set; }

        /// <summary>
        /// Risco associado ao produto, variando de 0.5 a 5
        /// </summary>
        [Display(Name = "Risco do Produto")]
        [Range(0.5, 5)]
        public decimal Risco { get; set; }

        /// <summary>
        /// Rentabilidades associadas ao produto
        /// </summary>
        public virtual ICollection<Rentabilidade>? Rentabilidades { get; set; }

    }
}
