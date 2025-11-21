using System.ComponentModel.DataAnnotations;

namespace InvestBoardAPI.Data
{
    /// <summary>
    /// Representa uma simulação financeira, incluindo detalhes do investimento, identificadores de clientes e produtos, e resultados calculados.
    /// </summary>
    /// <remarks>
    /// Classe para armazenar e recuperar informações sobre simulações de investimento individuais, 
    /// como o valor investido, a rentabilidade efetiva e o período de simulação. 
    /// Cada instância corresponde a uma única simulação realizada por um cliente para um produto específico.
    /// </remarks>
    public class Simulacao
    {
        /// <summary>
        /// Identificador da simulação
        /// </summary>
        [Key]
        [Display(Name = "ID Simulação")]
        public int SimulacaoId { get; set; }

        /// <summary>
        /// Identificador do cliente que realizou a simulação
        /// </summary>
        [Display(Name = "ID Cliente")]
        public int ClienteId { get; set; }

        /// <summary>
        /// Identificador do produto simulado
        /// </summary>
        [Display(Name = "ID Produto")]
        public int ProdutoId { get; set; }

        /// <summary>
        /// Produto associado à simulação
        /// </summary>
        public virtual Produto? Produto { get; set; }

        /// <summary>
        /// Valor inicial investido na simulação
        /// </summary>
        [Display(Name = "Valor inicial investido")]
        public decimal ValorInvestido { get; set; }

        /// <summary>
        /// Valor final após o período da simulação
        /// </summary>
        [Display(Name = "Valor final após período")]
        public decimal ValorFinal { get; set; }

        /// <summary>
        /// Rentabilidade efetiva obtida na simulação
        /// </summary>
        [Display(Name = "Rentabilidade efetiva")]
        public decimal RentabilidadeEfetiva { get; set; }

        /// <summary>
        /// Prazo total da simulação em meses
        /// </summary>
        [Display(Name = "Prazo total em meses")]
        public int PrazoMeses { get; set; }

        /// <summary>
        /// Data em que a simulação foi realizada
        /// </summary>
        [Display(Name = "Data da simulação")]
        public DateOnly DataSimulacao { get; set; }

        /// <summary>
        /// Hora em que a simulação foi realizada
        /// </summary>
        [Display(Name = "Hora da simulação")]
        public TimeOnly HoraSimulacao { get; set; }

    }
}
