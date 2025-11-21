using InvestBoardAPI.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.Json;
using static InvestBoardAPI.Controllers.InvestimentosController;

namespace InvestBoardTest
{
    public class InvestimentosTest(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
    {
        [Fact]
        public async Task GivenARequest_WhenCallingGetInvestimentosWithClienteIdInvestimentoId_ThenTheAPIReturnsExpectedResponse()
        {
            // Arrange
            var client = factory.CreateClient();

            var expectedStatusCode = HttpStatusCode.OK;
            var cliente = 1;
            var investimento = 1;
            var stopwatch = Stopwatch.StartNew();

            // Act
            var response = await client.GetAsync($"/investimentos/{cliente}/{investimento}");

            // Assert
            TestHelpers.AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var jsonResponse = await JsonSerializer.DeserializeAsync<IEnumerable<HistoricoInvestimento>>(await response.Content.ReadAsStreamAsync(), TestHelpers.JsonSerializerOptions);
            Assert.NotNull(jsonResponse);
        }

        [Fact]
        public async Task GivenARequest_WhenCallingGetInvestimentosWithClienteId_ThenTheAPIReturnsExpectedResponse()
        {
            // Arrange
            var client = factory.CreateClient();

            var expectedStatusCode = HttpStatusCode.OK;
            var cliente = 1;
            var stopwatch = Stopwatch.StartNew();

            // Act
            var response = await client.GetAsync($"/investimentos/{cliente}");

            // Assert
            TestHelpers.AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var jsonResponse = await JsonSerializer.DeserializeAsync<IEnumerable<HistoricoInvestimento>>(await response.Content.ReadAsStreamAsync(), TestHelpers.JsonSerializerOptions);
            Assert.NotNull(jsonResponse);
        }

        [Fact]
        public async Task GivenARequest_WhenCallingPostInvestimento_ThenTheAPIReturnsExpectedResponse()
        {
            // Arrange
            var client = factory.CreateClient();
            var clienteId = 1;
            var perfilId = 1;
            var produtoId = 1;
            var content = new StringContent(
                JsonSerializer.Serialize(new Investimento (){ClienteId = clienteId, ProdutoId = produtoId, Valor = 1000, Rentabilidade = 12.5M, Data = DateTime.Now}),
                Encoding.UTF8,
                "application/json");
            var expectedStatusCode = HttpStatusCode.Created;
            var stopwatch = Stopwatch.StartNew();

            // Act
            var responsePerfil = await client.GetAsync($"/perfil-risco/{clienteId}");
            if (responsePerfil.StatusCode != HttpStatusCode.OK)
            {
                responsePerfil = await client.PostAsync(
                    $"/perfil-risco?ClienteId={clienteId}&PerfilId={perfilId}", 
                    new StringContent("", Encoding.UTF8, "application/text"));
                //TestHelpers.AssertCommonResponseParts(stopwatch, responsePerfil, expectedStatusCode);
            }
            var response = await client.PostAsync($"/investimentos", content);

            // Assert
            Assert.Equal(expectedStatusCode, response.StatusCode);
            //TestHelpers.AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var jsonResponse = await JsonSerializer.DeserializeAsync<HistoricoInvestimento>(await response.Content.ReadAsStreamAsync(), TestHelpers.JsonSerializerOptions);
            Assert.NotNull(jsonResponse);
        }
    }
}
