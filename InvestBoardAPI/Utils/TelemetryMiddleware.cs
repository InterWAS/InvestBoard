using InvestBoardAPI.Data;
using System.Diagnostics;

namespace InvestBoardAPI.Utils
{
    /// <summary>
    /// Middleware que registra dados de telemetria para cada solicitação HTTP, incluindo o caminho do endpoint e o tempo de resposta, 
    /// para o banco de dados do aplicativo.
    /// </summary>
    /// <remarks>
    /// Middleware para registrar automaticamente as métricas de desempenho das requisições para monitoramento e análise. 
    /// Os dados de telemetria são armazenados após a conclusão de cada requisição, permitindo o rastreamento do uso do endpoint e
    /// tempos de resposta. Este middleware deve ser registrado no início do pipeline para garantir a medição precisa da duração das requisições.
    /// </remarks>
    /// <param name="next">
    /// O próximo delegate de middleware no pipeline de requisição HTTP. Ele é invocado para continuar o processamento da requisição.
    /// </param>
    /// <param name="dbContext">
    /// O contexto do banco de dados usado para persistir informações de telemetria para solicitações recebidas.
    /// </param>
    public class TelemetryMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        /// <summary>
        /// Invoca o próximo middleware no pipeline e registra os dados de telemetria da solicitação HTTP atual, incluindo o caminho do endpoint e o tempo de resposta.
        /// </summary>
        /// <param name="context">
        /// O contexto HTTP para a requisição atual. Fornece informações sobre a requisição e a resposta, e é passado para o próximo componente middleware.
        /// </param>
        /// <returns>
        /// Uma tarefa que representa a operação assíncrona de invocar o próximo middleware e registrar dados de telemetria.
        /// </returns>
        public async Task InvokeAsync(HttpContext context, InvestBoardDB dbContext)
        {
            var stopwatch = Stopwatch.StartNew();

            await _next(context); // Chama o próximo middleware

            stopwatch.Stop();
            
            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            var path = context.Request.Path.ToString();

            // Salva os dados de telemetria no banco de dados
            await dbContext.Telemetrias.AddAsync(
                new() 
                { 
                    Data = DateOnly.FromDateTime(DateTime.Now), 
                    Hora = TimeOnly.FromDateTime(DateTime.Now), 
                    Endpoint = path, 
                    TempoResposta = elapsedMilliseconds
                });
            await dbContext.SaveChangesAsync();

        }
    }
}
