using Microsoft.AspNetCore.Mvc.Testing;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using static InvestBoardAPI.Controllers.SimulacoesController;

namespace InvestBoardTest
{
    public class SimulacoesTest(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
    {
        [Fact]
        public async Task GivenARequest_WhenCallingGetSimulacoes_ThenTheAPIReturnsExpectedResponse()
        {
            // Arrange
            var client = factory.CreateClient();
            var expectedStatusCode = HttpStatusCode.OK;
            var stopwatch = Stopwatch.StartNew();

            // Act
            var response = await client.GetAsync($"/simulacoes");

            // Assert
            TestHelpers.AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var jsonResponse = await JsonSerializer.DeserializeAsync<IEnumerable<HistoricoSimulacao>>(await response.Content.ReadAsStreamAsync(), TestHelpers.JsonSerializerOptions);
            Assert.NotNull(jsonResponse);
        }

        [Fact]
        public async Task GivenARequest_WhenCallingGetSimulacoesPorProdutoDia_ThenTheAPIReturnsExpectedResponse()
        {
            // Arrange
            var client = factory.CreateClient();
            var expectedStatusCode = HttpStatusCode.OK;
            var stopwatch = Stopwatch.StartNew();

            // Act
            var response = await client.GetAsync($"/simulacoes/por-produto-dia");

            // Assert
            TestHelpers.AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var jsonResponse = await JsonSerializer.DeserializeAsync<IEnumerable<SimulacaoProdutoDia>>(await response.Content.ReadAsStreamAsync(), TestHelpers.JsonSerializerOptions);
            Assert.NotNull(jsonResponse);
        }
    }
}
