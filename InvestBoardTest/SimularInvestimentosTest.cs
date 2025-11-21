using InvestBoardAPI.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.Json;
using static InvestBoardAPI.Controllers.SimularInvestimentosController;

namespace InvestBoardTest
{
    public class SimularInvestimentosTest(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
    {
        [Fact]
        public async Task GivenARequest_WhenCallingPostSimularInvestimento_ThenTheAPIReturnsExpectedResponse()
        {
            // Arrange
            var client = factory.CreateClient();
            var clienteId = 1;
            var content = new StringContent(
                JsonSerializer.Serialize(new PedidoSimulacaoTipoProduto(ClienteId: clienteId, Valor: 1000.00M, PrazoMeses: 12, TipoProduto: "CDB")),
                Encoding.UTF8,
                "application/json");
            var expectedStatusCode = HttpStatusCode.OK;
            var stopwatch = Stopwatch.StartNew();

            // Act
            var response = await client.PostAsync($"/simular-investimento", content);

            // Assert
            //Assert.Equal(expectedStatusCode, response.StatusCode);
            TestHelpers.AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var jsonResponse = await JsonSerializer.DeserializeAsync<RespostaSimulacao>(await response.Content.ReadAsStreamAsync(), TestHelpers.JsonSerializerOptions);
            Assert.NotNull(jsonResponse);
        }

        [Fact]
        public async Task GivenARequest_WhenCallingPostSimularInvestimentoProduto_ThenTheAPIReturnsExpectedResponse()
        {
            // Arrange
            var client = factory.CreateClient();
            var clienteId = 1;
            var produtoId = 1;
            var content = new StringContent(
                JsonSerializer.Serialize(new PedidoSimulacaoProduto(ClienteId: clienteId, Valor: 1000.00M, PrazoMeses: 12, ProdutoId: produtoId)),
                Encoding.UTF8,
                "application/json");
            var expectedStatusCode = HttpStatusCode.OK;
            var stopwatch = Stopwatch.StartNew();

            // Act
            var response = await client.PostAsync($"/simular-investimento/produto", content);

            // Assert
            //Assert.Equal(expectedStatusCode, response.StatusCode);
            TestHelpers.AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var jsonResponse = await JsonSerializer.DeserializeAsync<RespostaSimulacao>(await response.Content.ReadAsStreamAsync(), TestHelpers.JsonSerializerOptions);
            Assert.NotNull(jsonResponse);
        }

    }
}
