using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InvestBoardAPI.Controllers
{
    public partial class TelemetriaController
    {
        /// <summary>
        /// Representa dados de telemetria para um serviço, incluindo seu nome, 
        /// o número de chamadas realizadas e o tempo médio de resposta em milissegundos.
        /// </summary>
        /// <param name="Nome">O nome do serviço para o qual os dados de telemetria são coletados.</param>
        /// <param name="QuantidadeChamadas">O número total de chamadas feitas ao serviço durante o período de telemetria.</param>
        /// <param name="MediaTempoRespostaMs">O tempo médio de resposta do serviço, medido em milissegundos.</param>
        public record ServicosTelemetria(
            [property: Description("O nome do serviço para o qual os dados de telemetria são coletados.")]
            string Nome,
            [property: Description("O número total de chamadas feitas ao serviço durante o período de telemetria.")]
            int QuantidadeChamadas,
            [property: Description("O tempo médio de resposta do serviço, medido em milissegundos.")]
            double MediaTempoRespostaMs);
    }
}
