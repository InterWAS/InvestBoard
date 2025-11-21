namespace InvestBoardAPI.Utils
{
    /// <summary>
    /// Fornece métodos de extensão para simplificar o registro de middleware de telemetria no pipeline de requisições de uma aplicação ASP.NET Core.
    /// </summary>
    /// <remarks>
    /// Esta classe contém métodos de extensão para IApplicationBuilder para simplificar a adição de middleware de telemetria. 
    /// Esses métodos são normalmente chamados na configuração de inicialização para habilitar a coleta de telemetria de requisições para o aplicativo.
    /// </remarks>
    public static class TelemetryMiddlewareExtensions
    {
        public static IApplicationBuilder UseTelemetryMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TelemetryMiddleware>();
        }
    }
}
