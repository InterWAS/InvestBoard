using InvestBoardAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InvestBoardAPI.Controllers
{
    /// <summary>
    /// Fornece endpoints de API para recuperar dados de telemetria agregados, incluindo contagens de chamadas e tempos médios de resposta para todos os endpoints monitorados.
    /// </summary>
    /// <remarks>
    /// Este controlador expõe endpoints administrativos para acesso a estatísticas de telemetria agrupadas por endpoint. Os dados retornados incluem o número total de chamadas e os tempos médios de resposta em milissegundos. Caso não existam registros de telemetria, as respostas conterão listas vazias e valores de data padrão. O acesso a esses endpoints pode exigir autorização apropriada.
    /// </remarks>
    /// <param name="Context">O contexto do banco de dados usado para acessar os registros de telemetria. Não pode ser nulo.</param>
    [Route("telemetria")]
    [ApiController]
    public partial class TelemetriaController(InvestBoardDB Context) : ControllerBase
    {
        /// <summary>
        /// Recupera dados de telemetria agregados para todos os endpoints, incluindo contagem de chamadas e tempos médios de resposta.
        /// </summary>
        /// <remarks>
        /// Retorna estatísticas de telemetria agrupadas por endpoint, incluindo o número total de chamadas e o tempo médio de resposta em milissegundos. Se não houver dados de telemetria disponíveis, o objeto retornado conterá uma lista vazia e valores de data definidos como <see cref="DateOnly.MinValue"/> e <see cref="DateOnly.MaxValue"/>. Este endpoint destina-se ao uso administrativo e pode exigir autorização apropriada.
        /// </remarks>
        /// <returns>
        /// Um objeto "<see cref="ActionResult{DadosTelemetria}"/>" contendo os dados de telemetria para todos os endpoints. Se não houver registros de telemetria, retorna um objeto com dados vazios e valores de data padrão.
        /// </returns>
        // GET: /[controller]
        //[Authorize("Admin")]
        [HttpGet]
        [EndpointSummary("Recupera dados de telemetria agregados para todos os endpoints, incluindo contagem de chamadas e tempos médios de resposta.")]
        [EndpointDescription("Retorna estatísticas de telemetria agrupadas por endpoint, incluindo o número total de chamadas e o tempo médio de resposta em milissegundos. Se não houver dados de telemetria disponíveis, o objeto retornado conterá uma lista vazia e valores de data definidos como mínimo e máximo. Este endpoint destina-se ao uso administrativo e pode exigir autorização apropriada.")]
        [ProducesResponseType<DadosTelemetria>(StatusCodes.Status200OK, "application/json")]
        public async Task<ActionResult<DadosTelemetria>> GetTelemetria()
        {
            // Verifica se existe telemetria para retornar
            // Se não retorna dados vazios
            if (await Context.Telemetrias.AnyAsync())
            {
                return new DadosTelemetria(
                    await Context.Telemetrias.GroupBy(
                        evento => evento.Endpoint,
                        evento => evento.TempoResposta,
                        (endpoint, elapsed) => new ServicosTelemetria(
                            Nome: endpoint,
                            QuantidadeChamadas: elapsed.Count(),
                            MediaTempoRespostaMs: elapsed.Average())).ToListAsync(),
                    await Context.Telemetrias.MinAsync(evento => evento.Data),
                    await Context.Telemetrias.MaxAsync(evento => evento.Data));
            }
            else
            {
                return new DadosTelemetria([], DateOnly.MinValue, DateOnly.MaxValue);
            }
        }
    }
}
