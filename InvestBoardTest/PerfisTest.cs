using Microsoft.AspNetCore.Mvc.Testing;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.Json;
using static InvestBoardAPI.Controllers.PerfisController;

namespace InvestBoardTest
{
    public class PerfisTest(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
    {
        [Fact]
        public async Task GivenARequest_WhenCallingGetPerfis_ThenTheAPIReturnsExpectedResponse()
        {
            // Arrange
            var client = factory.CreateClient();

            var expectedStatusCode = HttpStatusCode.OK;
            var stopwatch = Stopwatch.StartNew();

            // Act
            var response = await client.GetAsync($"/perfil-risco");

            // Assert
            TestHelpers.AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var jsonResponse = await JsonSerializer.DeserializeAsync<IEnumerable<ClientePerfil>>(await response.Content.ReadAsStreamAsync(), TestHelpers.JsonSerializerOptions);
            Assert.NotNull(jsonResponse);
        }

        [Fact]
        public async Task GivenARequest_WhenCallingGetPerfilWithClienteId_ThenTheAPIReturnsExpectedResponse()
        {
            // Arrange
            var client = factory.CreateClient();
            var clienteId = 1;
            var expectedStatusCode = HttpStatusCode.OK;
            var stopwatch = Stopwatch.StartNew();

            // Act
            var response = await client.GetAsync($"/perfil-risco/{clienteId}");

            // Assert
            TestHelpers.AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var jsonResponse = await JsonSerializer.DeserializeAsync<ClientePerfil>(await response.Content.ReadAsStreamAsync(), TestHelpers.JsonSerializerOptions);
            Assert.NotNull(jsonResponse);
        }

        [Fact]
        public async Task GivenARequest_WhenCallingPostPerfil_ThenTheAPIReturnsExpectedResponse()
        {
            // Arrange
            var client = factory.CreateClient();
            var clienteId = 1;
            var perfilId = 1;
            var content = new StringContent("", Encoding.UTF8, "application/text");
            var expectedStatusCode = HttpStatusCode.Created;
            var stopwatch = Stopwatch.StartNew();

            // Act
            var responsePerfil = await client.GetAsync($"/perfil-risco/{clienteId}");
            if (responsePerfil.StatusCode == HttpStatusCode.OK)
            {
                return;
            }
            var response = await client.PostAsync(
                $"/perfil-risco?ClienteId={clienteId}&PerfilId={perfilId}",
                content);

            // Assert
            //Assert.Equal(expectedStatusCode, response.StatusCode);
            TestHelpers.AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var jsonResponse = await JsonSerializer.DeserializeAsync<ClientePerfil>(await response.Content.ReadAsStreamAsync(), TestHelpers.JsonSerializerOptions);
            Assert.NotNull(jsonResponse);
        }

        [Fact]
        public async Task GivenARequest_WhenCallingPutPerfil_ThenTheAPIReturnsExpectedResponse()
        {
            // Arrange
            var client = factory.CreateClient();
            var clienteId = 1;
            var perfilId = 1;
            var content = new StringContent("", Encoding.UTF8, "application/text");
            var expectedStatusCode = HttpStatusCode.OK;
            var stopwatch = Stopwatch.StartNew();

            // Act
            var responsePerfil = await client.GetAsync($"/perfil-risco/{clienteId}");
            if (responsePerfil.StatusCode != HttpStatusCode.OK)
            {
                return;
            }
            var response = await client.PutAsync(
                $"/perfil-risco?ClienteId={clienteId}&PerfilId={perfilId}",
                content);

            // Assert
            //Assert.Equal(expectedStatusCode, response.StatusCode);
            TestHelpers.AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var jsonResponse = await JsonSerializer.DeserializeAsync<ClientePerfil>(await response.Content.ReadAsStreamAsync(), TestHelpers.JsonSerializerOptions);
            Assert.NotNull(jsonResponse);
        }

    }
}
