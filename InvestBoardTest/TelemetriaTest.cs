using Microsoft.AspNetCore.Mvc.Testing;
using System.Diagnostics;
using System.Text.Json;
using static InvestBoardAPI.Controllers.TelemetriaController;

namespace InvestBoardTest
{
    /// <summary>
    /// Fornece testes de integração para os endpoints da API Telemetria usando uma fábrica de aplicativos web de teste.
    /// </summary>
    /// <remarks>
    /// Esta classe de teste verifica se os endpoints da Telemetria retornam respostas válidas e com o tipo de conteúdo esperado. 
    /// Ela utiliza o xUnit e o WebApplicationFactory do ASP.NET Core para simular requisições HTTP à API em um ambiente de teste controlado.
    /// </remarks>
    /// <param name="factory">A fábrica de aplicações web usada para criar um servidor de teste em memória para requisições de API.</param>
    public class TelemetriaTest(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
    {
        /// <summary>
        /// Verifica se uma solicitação GET para o endpoint especificado retorna um código de status bem-sucedido e o tipo de conteúdo JSON esperado.
        /// </summary>
        /// <remarks>
        /// Este teste garante que o endpoint responda com um código de status no intervalo de 200 a 299 e um tipo de conteúdo "application/json; charset=utf-8". 
        /// Teste para validar se os endpoints da API estão formatando corretamente as respostas.
        /// </remarks>
        /// <param name="url">A URL relativa do endpoint a ser testado. Deve ser uma rota válida que responda a solicitações GET.</param>
        /// <returns>Uma tarefa que representa a operação de teste assíncrona.</returns>
        [Theory]
        [InlineData("/Telemetria")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("application/json; charset=utf-8",
                response?.Content?.Headers?.ContentType?.ToString());
        }

        /// <summary>
        /// Verifica se uma solicitação GET para o endpoint '/Telemetria' retorna uma resposta bem-sucedida com o conteúdo JSON esperado.
        /// </summary>
        /// <remarks>
        /// Este teste garante que a API responda com HTTP 200 (OK), se o tipo de conteúdo da resposta é 'application/json' 
        /// e é um objeto JSON válido para o endpoint '/Telemetria'. 
        /// </remarks>
        /// <returns>Uma tarefa que representa a operação de teste assíncrona.</returns>
        [Fact]
        public async Task GivenARequest_WhenCallingGetTelemetria_ThenTheAPIReturnsExpectedResponse()
        {
            // Arrange
            var client = factory.CreateClient();

            var expectedStatusCode = System.Net.HttpStatusCode.OK;
            var stopwatch = Stopwatch.StartNew();

            // Act
            var response = await client.GetAsync("/Telemetria");

            // Assert
            TestHelpers.AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var jsonResponse = await JsonSerializer.DeserializeAsync<DadosTelemetria?>(await response.Content.ReadAsStreamAsync(), TestHelpers.JsonSerializerOptions);
            Assert.NotNull(jsonResponse);
        }
    }
}
