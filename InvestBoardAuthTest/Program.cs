using NETCore.Keycloak.Client.HttpClients.Implementation;
using NETCore.Keycloak.Client.Models;
using NETCore.Keycloak.Client.Models.Auth;
using System.Net.Http.Headers;

Console.WriteLine("InvestBoard Auth Test");

var client = new HttpClient();

// Create Keycloak client
var keycloakClient = new KeycloakClient("http://localhost:8080");
// Authenticate
var token = await keycloakClient.Auth.GetClientCredentialsTokenAsync(
    "InvestBoardRealm",
    new KcClientCredentials
    {
        ClientId = "InvestBoardFrontClient", 
        Secret = "QjxrZ1IoewJJGBibDlBbMJSKBxJY1EcB"
    });

if (token.IsError) throw new Exception(token.ErrorMessage);

Console.WriteLine($"Access Token:\n{token.Response.AccessToken}\n");

// Chama endpoint protegido
try
{
    client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", token.Response.AccessToken);
    var apiResponse = await client.GetAsync("https://localhost:32769/Telemetria");
    if (apiResponse.Content.Headers.ContentType?.MediaType == "application/json")
    {
        var content = await apiResponse.Content.ReadAsStringAsync();
        Console.WriteLine($"Resposta da API: {content}");
    }
    else
    {         
        Console.WriteLine($"Status da API: {apiResponse.StatusCode} - {apiResponse.ReasonPhrase}");
    }
}
catch (Exception e)
{
    Console.WriteLine($"Erro ao chamar API: {e.Message}");
    throw;
}

Console.WriteLine("Fim do teste.");
Console.Read();
