using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InvestBoardAPI.Controllers
{
    public partial class TelemetriaController
    {
        /// <summary>
        /// Representa um conjunto de dados de telemetria para um período de tempo específico, 
        /// incluindo uma coleção de serviços de telemetria e as datas de início e término do intervalo.
        /// </summary>
        /// <param name="Servicos">Lista de serviços de telemetria incluídos no conjunto de dados. Não pode ser nulo.</param>
        /// <param name="Inicio">A data de início do intervalo de dados de telemetria.</param>
        /// <param name="Fim">A data final do intervalo de dados de telemetria.</param>
        public record DadosTelemetria(
            [property: Description("Lista de serviços de telemetria incluídos no conjunto de dados. Não pode ser nulo.")]
            List<ServicosTelemetria> Servicos,
            [property: Description("A data de início do intervalo de dados de telemetria.")]
            DateOnly Inicio,
            [property: Description("A data final do intervalo de dados de telemetria.")]
            DateOnly Fim);
    }
}
