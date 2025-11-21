using System.ComponentModel.DataAnnotations;

namespace InvestBoardAPI.Data
{
    /// <summary>
    /// Representa um perfil de cliente com identificação, nome e nível máximo de risco permitido
    /// </summary>
    /// <remarks>
    /// Classe para definir e gerenciar informações de perfil, incluindo restrições de níveis de risco. O nível máximo de risco deve estar dentro do intervalo válido para evitar erros
    /// </remarks>
    public class Perfil
    {
        /// <summary>
        /// Campo de identificação do perfil
        /// </summary>
        [Key]
        [Display(Name = "ID")]
        public int PerfilId { get; set; }

        /// <summary>
        /// Nome do perfil
        /// </summary>
        [Display(Name = "Nome do Perfil")]
        [MaxLength(50)]
        [StringLength(50)]
        public required string Nome { get; set; }

        /// <summary>
        /// Descrição do perfil
        /// </summary>
        [Display(Name = "Descrição do Perfil")]
        [MaxLength(100)]
        [StringLength(100)]
        public required string Descricao { get; set; }

        /// <summary>
        /// Nível máximo de risco permitido
        /// </summary>
        /// <remarks>
        /// Valores válidos entre 0 to 5. Valores fora desse intervalo resultam em erro de validação
        /// </remarks>
        [Display(Name = "Risco Máximo")]
        [Range(0, 5)]
        public decimal RiscoMaximo { get; set; }

    }
}
