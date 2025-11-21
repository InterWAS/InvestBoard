using Microsoft.AspNetCore.Mvc.Testing;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using static InvestBoardAPI.Controllers.ProdutosRecomendadosController;

namespace InvestBoardTest
{
    public class ProdutosRecomendadosTest(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
    {
        [Fact]
        public async Task GivenARequest_WhenCallingGetProdutosRecomendadosWithPerfilId_ThenTheAPIReturnsExpectedResponse()
        {
            // Arrange
            var client = factory.CreateClient();
            var perfilId = 1;
            var expectedStatusCode = HttpStatusCode.OK;
            var stopwatch = Stopwatch.StartNew();

            // Act
            var response = await client.GetAsync($"/produtos-recomendados/{perfilId}");

            // Assert
            TestHelpers.AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var jsonResponse = await JsonSerializer.DeserializeAsync<IEnumerable<ProdutoRecomendado>>(await response.Content.ReadAsStreamAsync(), TestHelpers.JsonSerializerOptions);
            Assert.NotNull(jsonResponse);
        }

    }
}
