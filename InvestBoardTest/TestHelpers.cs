using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace InvestBoardTest
{
    /// <summary>
    /// Fornece métodos auxiliares e configuração para testar respostas HTTP e serialização JSON em testes unitários.
    /// </summary>
    /// <remarks>
    /// Esta classe inclui utilitários para verificar o conteúdo da resposta HTTP, códigos de status e tempo decorrido,
    /// bem como para criar conteúdo HTTP formatado em JSON. Todos os membros são destinados ao uso em cenários de teste
    /// e não foram projetados para uso em produção.
    /// </remarks>
    internal class TestHelpers
    {
        private const string _jsonMediaType = "application/json";
        private const string _plainMediaType = "text/plain";
        private const int _expectedMaxElapsedMilliseconds = 1000;
        public static readonly JsonSerializerOptions JsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

        /// <summary>
        /// Verifica se a resposta HTTP possui o código de status e o conteúdo JSON esperados e se o tipo de conteúdo da resposta é application/json.
        /// </summary>
        /// <remarks>
        /// Este método verifica se o tipo de conteúdo da resposta é JSON e se o conteúdo desserializado corresponde ao valor esperado. 
        /// A verificação falhará se o código de status, o tipo de conteúdo ou o conteúdo não corresponderem aos valores esperados.
        /// </remarks>
        /// <typeparam name="T">O tipo de conteúdo de resposta esperado para comparar com o conteúdo JSON desserializado.</typeparam>
        /// <param name="stopwatch">Uma instância de cronômetro usada para medir a duração da requisição HTTP. 
        /// Deve estar em execução e associada à requisição que está sendo validada.</param>
        /// <param name="response">A mensagem de resposta HTTP a ser validada. Não pode ser nula.</param>
        /// <param name="expectedStatusCode">O código de status HTTP esperado que a resposta deve ter.</param>
        /// <param name="expectedContent">O conteúdo esperado para comparação com o conteúdo JSON desserializado do corpo da resposta.</param>
        /// <returns>Uma tarefa que representa a operação de asserção assíncrona.</returns>
        public static async Task AssertResponseWithContentAsync<T>(
            Stopwatch stopwatch,
            HttpResponseMessage response,
            System.Net.HttpStatusCode expectedStatusCode,
            T expectedContent)
        {
            AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
            Assert.Equal(_jsonMediaType, response.Content.Headers.ContentType?.MediaType);
            Assert.Equal(expectedContent, await JsonSerializer.DeserializeAsync<T?>(
                await response.Content.ReadAsStreamAsync(), 
                JsonSerializerOptions));
        }

        /// <summary>
        /// Verifica se a resposta HTTP corresponde ao código de status esperado e contém o texto simples especificado.
        /// content.
        /// </summary>
        /// <remarks>
        /// Este método verifica se a resposta utiliza o tipo de mídia texto simples e se seu conteúdo corresponde ao valor esperado. 
        /// Destina-se ao uso em cenários de teste para garantir a correção da resposta.
        /// </remarks>
        /// <param name="stopwatch">Uma instância de cronômetro usada para medir a duração da solicitação HTTP para fins de validação.</param>
        /// <param name="response">A mensagem de resposta HTTP a ser validada em relação ao código de status e conteúdo esperados.</param>
        /// <param name="expectedStatusCode">O código de status HTTP que se espera que a resposta tenha.</param>
        /// <param name="expectedContent">O conteúdo esperado em texto simples do corpo da resposta.</param>
        /// <returns>Uma tarefa que representa a operação de asserção assíncrona.</returns>
        public static async Task AssertResponseWithPlainAsync(
            Stopwatch stopwatch,
            HttpResponseMessage response,
            System.Net.HttpStatusCode expectedStatusCode,
            string expectedContent)
        {
            AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
            Assert.Equal(_plainMediaType, response.Content.Headers.ContentType?.MediaType);
            Assert.Equal(expectedContent, await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Verifica se a resposta HTTP possui o código de status esperado e que o tempo decorrido para a operação está dentro do limite permitido.
        /// </summary>
        /// <remarks>
        /// Este método destina-se ao uso em cenários de teste para verificar tanto a execução correta quanto o desempenho das operações HTTP. 
        /// Se as asserções falharem, uma exceção será lançada pela estrutura de teste subjacente.
        /// </remarks>
        /// <param name="stopwatch">A instância <see cref="System.Diagnostics.Stopwatch"/> utilizada para medir o tempo decorrido da operação. 
        /// Deve ser iniciada antes do início da operação.</param>
        /// <param name="response">A instância <see cref="System.Net.Http.HttpResponseMessage"/> que será validada. Representa a resposta HTTP recebida da operação.</param>
        /// <param name="expectedStatusCode">O valor <see cref="System.Net.HttpStatusCode"/> esperado que a resposta deve ter.</param>
        public static void AssertCommonResponseParts(Stopwatch stopwatch,
            HttpResponseMessage response, System.Net.HttpStatusCode expectedStatusCode)
        {
            Assert.Equal(expectedStatusCode, response.StatusCode);
            Assert.True(stopwatch.ElapsedMilliseconds < _expectedMaxElapsedMilliseconds);
        }

        /// <summary>
        /// Cria uma nova instância de StringContent contendo a representação JSON do modelo especificado, usando codificação UTF-8 e o tipo de mídia application/json.
        /// </summary>
        /// <remarks>Este método utiliza System.Text.Json para serialização. 
        /// O StringContent resultante pode ser usado como corpo de uma requisição HTTP ao enviar dados JSON.</remarks>
        /// <typeparam name="T">O tipo de modelo a ser serializado para JSON.</typeparam>
        /// <param name="model">O objeto a ser serializado para JSON. Não pode ser nulo.</param>
        /// <returns>Um objeto StringContent contendo a representação JSON serializada do modelo.</returns>
        public static StringContent GetJsonStringContent<T>(T model)
            => new(JsonSerializer.Serialize(model), Encoding.UTF8, _jsonMediaType);
    }
}
