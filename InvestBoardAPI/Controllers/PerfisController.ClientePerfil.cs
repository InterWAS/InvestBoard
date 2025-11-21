using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InvestBoardAPI.Controllers
{
public partial class PerfisController
    {
        /// <summary>
        /// Representa o perfil de um cliente, incluindo identificação, tipo de perfil, pontuação e descrição.
        /// </summary>
        /// <param name="ClienteId">O identificador único do cliente associado a este perfil.</param>
        /// <param name="Perfil">O tipo ou nome do perfil atribuído ao cliente. Pode ser nulo se não for especificado.</param>
        /// <param name="Pontuacao">A pontuação ou classificação associada ao perfil do cliente. Pode ser nula se não houver pontuação disponível.</param>
        /// <param name="Descricao">Uma descrição opcional que fornece detalhes adicionais sobre o perfil do cliente.</param>
        public record ClientePerfil(
            [property: Description("O identificador único do cliente associado a este perfil.")]
            int ClienteId,
            [property: Description("O tipo ou nome do perfil atribuído ao cliente. Pode ser nulo se não for especificado.")]
            string? Perfil, 
            [property: Range(0.0, 5.0)]
            [property: Description("A pontuação ou classificação associada ao perfil do cliente. Pode ser nula se não houver pontuação disponível.")]
            decimal? Pontuacao,
            [property: Description("Uma descrição opcional que fornece detalhes adicionais sobre o perfil do cliente.")]
            string? Descricao);
    }
}
