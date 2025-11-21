using InvestBoardAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace InvestBoardAPI.Controllers
{
    /// <summary>
    /// Fornece endpoints de API para gerenciar e recuperar informações de perfil de risco associadas a clientes. 
    /// Suporta operações para obter, criar e atualizar o perfil de risco de um cliente.
    /// </summary>
    /// <remarks>
    /// Este controlador expõe endpoints para recuperar, criar e atualizar perfis de risco de clientes. 
    /// Todas as operações exigem identificadores válidos de cliente e perfil. 
    /// As respostas indicam sucesso ou fornecem informações de erro apropriadas 
    /// caso os recursos solicitados não sejam encontrados ou se a validação de entrada falhar.
    /// </remarks>
    /// <param name="Context">O contexto do banco de dados usado para acessar os dados do cliente e do perfil. Não pode ser nulo.</param>
    [Route("perfil-risco")]
    [ApiController]
    public partial class PerfisController(InvestBoardDB Context) : ControllerBase
    {

        /// <summary>
        /// Recupera uma lista de perfis de clientes, incluindo nome do perfil, pontuação de risco e descrição, para todos os clientes no sistema.
        /// </summary>
        /// <remarks>
        /// As informações de perfil de cada cliente são incluídas somente se disponíveis; caso contrário, os campos de perfil estarão vazios. A autorização pode ser necessária dependendo da configuração do aplicativo.
        /// </remarks>
        /// <returns>
        /// Uma operação assíncrona que, ao ser concluída, retorna um <see cref="ActionResult{T}"/> 
        /// contendo uma lista de objetos <see cref="ClientePerfil"/> 
        /// representando os perfis de todos os clientes. Se não houver clientes, a lista estará vazia.
        /// </returns>
        // GET: /[controller]
        //[Authorize("Admin")]
        [HttpGet]
        [EndpointSummary("Recupera uma lista de perfis de clientes, incluindo nome do perfil, pontuação de risco e descrição, para todos os clientes no sistema.")]
        [EndpointDescription("As informações de perfil de cada cliente são incluídas somente se disponíveis; caso contrário, os campos de perfil estarão vazios. A autorização pode ser necessária dependendo da configuração do aplicativo.")]
        [ProducesResponseType<IEnumerable<ClientePerfil>>(StatusCodes.Status200OK, "application/json")]
        public async Task<ActionResult<IEnumerable<ClientePerfil>>> GetPerfil()
        {
            return await Context.Clientes
                .Select(
                    cliente => new ClientePerfil(
                        ClienteId: cliente.ClienteId,
                        Perfil: (cliente.Perfil == null ? null : cliente.Perfil.Nome),
                        Pontuacao: (cliente.Perfil == null ? null : cliente.Perfil.RiscoMaximo),
                        Descricao: (cliente.Perfil == null ? null : cliente.Perfil.Descricao)))
                .ToListAsync();
        }

        /// <summary>
        /// Recupera as informações de perfil de um cliente específico.
        /// </summary>
        /// <param name="ClienteId">O identificador único do cliente cujo perfil deve ser recuperado. Deve ser maior que zero.</param>
        /// <returns>
        /// Um <see cref="ActionResult{ClientePerfil}"/> contendo as informações do perfil do cliente, se encontradas; caso contrário, 
        /// um resultado indicando que o cliente não foi encontrado ou que o identificador é inválido.
        /// </returns>
        // GET: /[controller]/{ClienteId}
        //[Authorize]
        [HttpGet("{ClienteId}")]
        [EndpointSummary("Recupera as informações de perfil de um cliente específico.")]
        [EndpointDescription("As informações de perfil do cliente são retornadas somente se disponíveis; caso contrário, retorna lista vazia.")]
        [ProducesResponseType<IEnumerable<ClientePerfil>>(StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
        public async Task<ActionResult<ClientePerfil?>> GetPerfil(
            [Description("O identificador único do cliente cujo perfil deve ser recuperado. Deve ser maior que zero.")]
            int ClienteId)
        {
            if (ClienteId <= 0)
            {
                return BadRequest("ID Cliente inválido.");
            }
            var cliente = await Context.Clientes.FindAsync(ClienteId);
            if (cliente == null)
            {
                return NotFound("Cliente não encontrado.");
            }
            return new ClientePerfil(
                ClienteId: cliente.ClienteId,
                Perfil: (cliente.Perfil?.Nome),
                Pontuacao: (cliente.Perfil?.RiscoMaximo),
                Descricao: (cliente.Perfil?.Descricao));
        }

        /// <summary>
        /// Cria um novo perfil de risco para um cliente usando os identificadores de cliente e de perfil especificados.
        /// </summary>
        /// <remarks>
        /// Retorna BadRequest se o identificador do cliente ou o identificador do perfil for inválido, ou se o cliente já possuir um perfil de risco. Retorna NotFound se o perfil especificado não existir.
        /// </remarks>
        /// <param name="ClienteId">O identificador único do cliente para o qual o perfil de risco será criado. Deve ser um número inteiro positivo.</param>
        /// <param name="PerfilId">O identificador único do perfil de risco a ser associado ao cliente. Deve ser um número inteiro positivo.</param>
        /// <returns>
        /// Se a operação for bem-sucedida, será retornado um ActionResult contendo o ClientePerfil criado; 
        /// caso contrário, um resultado BadRequest ou NotFound se a entrada for inválida ou o perfil não existir.
        /// </returns>
        // POST: /[controller]
        //[Authorize]
        [HttpPost]
        [EndpointSummary("Cria um novo perfil de risco para um cliente usando os identificadores de cliente e de perfil especificados.")]
        [EndpointDescription("Retorna BadRequest se o identificador do cliente ou o identificador do perfil for inválido, ou se o cliente já possuir um perfil de risco. Retorna NotFound se o perfil especificado não existir.")]
        [ProducesResponseType<ClientePerfil>(StatusCodes.Status201Created, "application/json")]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict, "application/problem+json")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ClientePerfil>> PostPerfil(
            [Description("O identificador único do cliente para o qual o perfil de risco será criado. Deve ser um número inteiro positivo.")]
            int ClienteId,
            [Description("O identificador único do perfil de risco a ser associado ao cliente. Deve ser um número inteiro positivo.")]
            int PerfilId)
        {
            if (ClienteId <= 0)
            {
                return BadRequest("ID Cliente inválido.");
            }
            if (PerfilId <= 0)
            {
                return BadRequest("ID Perfil inválido.");
            }

            var perfil = await Context.Perfis.FindAsync(PerfilId);
            if (perfil == null)
            {
                return NotFound("Perfil não encontrado.");
            }

            var cliente = await Context.Clientes.FindAsync(ClienteId);
            if (cliente != null)
            {
                return BadRequest("Perfil de risco do cliente já cadastrado.");
            }

            var novoCliente = await Context.Clientes
                .AddAsync(
                    new Cliente() {
                        ClienteId = ClienteId,
                        PerfilId = PerfilId });

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(409, "Conflito de concorrência ao criar o perfil do cliente.");
            }
            catch (DbUpdateException)
            {
                var correlationId = Guid.NewGuid().ToString();
                return Problem(detail: $"Erro ao salvar alterações. CorrelationId: {correlationId}", statusCode: 500);
            }
            catch (Exception)
            {
                var correlationId = Guid.NewGuid().ToString();
                return Problem(detail: $"Erro interno. CorrelationId: {correlationId}", statusCode: 500);
            }

            return CreatedAtAction(
                nameof(GetPerfil),
                new { ClienteId },
                new ClientePerfil(
                    ClienteId: ClienteId,
                    Perfil: perfil.Nome,
                    Pontuacao: perfil.RiscoMaximo,
                    Descricao: perfil.Descricao));
        }

        /// <summary>
        /// Atualiza o perfil associado ao cliente especificado e retorna as informações atualizadas do perfil do cliente.
        /// </summary>
        /// <remarks>
        /// Este método atribui um novo perfil ao cliente especificado. Retorna BadRequest se o identificador do cliente ou o identificador do perfil for inválido. A operação é assíncrona e requer que tanto o cliente quanto o perfil existam. Se algum deles não for encontrado, uma resposta NotFound será retornada.
        /// </remarks>
        /// <param name="ClienteId">O identificador único do cliente cujo perfil será atualizado. Deve corresponder a um cliente já existente.</param>
        /// <param name="PerfilId">O identificador único do perfil a ser associado ao cliente. Deve corresponder a um perfil existente.</param>
        /// <returns>
        /// Um ActionResult contendo as informações atualizadas do perfil do cliente. Retorna um resultado NotFound se o cliente ou o perfil não existir.
        /// </returns>
        // PUT: /[controller]
        //[Authorize]
        [HttpPut]
        [EndpointSummary("Atualiza o perfil associado ao cliente especificado e retorna as informações atualizadas do perfil do cliente.")]
        [EndpointDescription("Este método atribui um novo perfil ao cliente especificado. Retorna BadRequest se o identificador do cliente ou o identificador do perfil for inválido. A operação é assíncrona e requer que tanto o cliente quanto o perfil existam. Se algum deles não for encontrado, uma resposta NotFound será retornada.")]
        [ProducesResponseType<ClientePerfil>(StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict, "application/problem+json")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ClientePerfil>> PutPerfil(
            [Description("O identificador único do cliente cujo perfil será atualizado. Deve corresponder a um cliente já existente.")] 
            int ClienteId, 
            [Description("O identificador único do perfil a ser associado ao cliente. Deve corresponder a um perfil existente.")] 
            int PerfilId)
        {
            if (ClienteId <= 0)
            {
                return BadRequest("ID Cliente inválido.");
            }
            if (PerfilId <= 0)
            {
                return BadRequest("ID Perfil inválido.");
            }

            var cliente = await Context.Clientes.FindAsync(ClienteId);
            if (cliente == null)
            {
                return NotFound("Cliente não encontrado.");
            }
            var perfil = await Context.Perfis.FindAsync(PerfilId);
            if (perfil == null)
            {
                return NotFound("Perfil não encontrado.");
            }

            if (cliente.PerfilId == PerfilId)
            {
                return new ClientePerfil(
                    ClienteId: cliente.ClienteId,
                    Perfil: perfil.Nome,
                    Pontuacao: perfil.RiscoMaximo,
                    Descricao: perfil.Descricao);
            }

            cliente.PerfilId = PerfilId;
            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(409, "Conflito de concorrência ao atualizar o cliente.");
            }
            catch (DbUpdateException)
            {
                var correlationId = Guid.NewGuid().ToString();
                return Problem(detail: $"Erro ao salvar alterações. CorrelationId: {correlationId}", statusCode: 500);
            }
            catch (Exception)
            {
                var correlationId = Guid.NewGuid().ToString();
                return Problem(detail: $"Erro interno. CorrelationId: {correlationId}", statusCode: 500);
            }

            return new ClientePerfil(
                ClienteId: cliente.ClienteId,
                Perfil: perfil.Nome,
                Pontuacao: perfil.RiscoMaximo,
                Descricao: perfil.Descricao);
        }
    }
}
