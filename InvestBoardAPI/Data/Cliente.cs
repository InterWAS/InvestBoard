using System.ComponentModel.DataAnnotations;

namespace InvestBoardAPI.Data
{
    /// <summary>
    /// Representa um cliente com identificação e informações de perfil associadas
    /// </summary>
    /// <remarks>
    /// Classe para definir e gerenciar informações de clientes, incluindo a associação a perfis específicos.
    /// </remarks>
    public class Cliente
    {
        /// <summary>
        /// Identificação do cliente
        /// </summary>
        [Display(Name = "ID Cliente")]
        public int ClienteId { get; set; }

        /// <summary>
        /// Identificação do perfil associado ao cliente
        /// </summary>
        [Display(Name = "ID Perfil")]
        public int PerfilId { get; set; }

        /// <summary>
        /// Perfil associado ao cliente
        /// </summary>
        public virtual Perfil? Perfil { get; set; }

    }
}
