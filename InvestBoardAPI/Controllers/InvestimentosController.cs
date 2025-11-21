using InvestBoardAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace InvestBoardAPI.Controllers
{
    [Route("investimentos")]
    [ApiController]
    public partial class InvestimentosController(InvestBoardDB Context) : ControllerBase
    {
        /// <summary>
        /// Recupera o histórico de investimentos de um cliente e investimento específicos.
        /// </summary>
        /// <remarks>
        /// Método usado para obter dados históricos de um investimento específico pertencente a um cliente. Os registros retornados incluem detalhes como tipo de investimento, valor, rentabilidade e data. 
        /// </remarks>
        /// <param name="ClienteId">O identificador único do cliente cujo histórico de investimentos deve ser recuperado.</param>
        /// <param name="InvestimentoId">O identificador único do investimento para o qual o histórico é solicitado.</param>
        /// <returns>
        /// Uma operação assíncrona que retorna um resultado de ação contendo uma coleção de registros de histórico de investimentos 
        /// para o cliente e investimento especificados. A coleção estará vazia se nenhum registro correspondente for encontrado.
        /// </returns>
        // GET: /[controller]/{ClienteId}/{InvestimentoId}
        //[Authorize]
        [HttpGet("{ClienteId}/{InvestimentoId}")]
        [EndpointSummary("Recupera o histórico de investimentos de um cliente e investimento específicos.")]
        [EndpointDescription("Método usado para obter dados históricos de um investimento específico pertencente a um cliente. Os registros retornados incluem detalhes como tipo de investimento, valor, rentabilidade e data. ")]
        [ProducesResponseType<IEnumerable<HistoricoInvestimento>>(StatusCodes.Status200OK, "application/json")]
        public async Task<ActionResult<IEnumerable<HistoricoInvestimento>>> GetInvestimento(
            [Description("O identificador único do cliente cujo histórico de investimentos deve ser recuperado.")] 
            int ClienteId, 
            [Description("O identificador único do investimento para o qual o histórico é solicitado.")] 
            int InvestimentoId)
        {
            return await Context.Investimentos
                .Where(investimento => investimento.ClienteId == ClienteId && investimento.InvestimentoId == InvestimentoId)
                .Select(investimento => new HistoricoInvestimento(
                    investimento.InvestimentoId,
                    investimento.Produto != null ? investimento.Produto.Tipo.ToString() : "",
                    investimento.Valor,
                    investimento.Rentabilidade,
                    DateOnly.FromDateTime(investimento.Data)
                    )).ToListAsync();
        }

        /// <summary>
        /// Recupera o histórico de investimentos do cliente especificado.
        /// </summary>
        /// <remarks>
        /// Este método requer um identificador de cliente válido. O histórico de investimentos retornado inclui detalhes como tipo de investimento, valor, rentabilidade e data. 
        /// </remarks>
        /// <param name="ClienteId">O identificador único do cliente cujo histórico de investimentos deve ser recuperado.</param>
        /// <returns>
        /// Uma operação assíncrona que retorna um <see cref="ActionResult"/> contendo uma coleção de objetos <see cref="HistoricoInvestimento"/> 
        /// para o cliente especificado. Retorna uma coleção vazia se o cliente não tiver investimentos.
        /// </returns>
        // GET: /[controller]/{ClienteId}
        //[Authorize]
        [HttpGet("{ClienteId}")]
        [EndpointSummary("Recupera o histórico de investimentos do cliente especificado.")]
        [EndpointDescription("Este método requer um identificador de cliente válido. O histórico de investimentos retornado inclui detalhes como tipo de investimento, valor, rentabilidade e data. ")]
        [ProducesResponseType<IEnumerable<HistoricoInvestimento>>(StatusCodes.Status200OK, "application/json")]
        public async Task<ActionResult<IEnumerable<HistoricoInvestimento>>> GetInvestimentos(
            [Description("O identificador único do cliente cujo histórico de investimentos deve ser recuperado.")]
            int ClienteId)
        {
            return await Context.Investimentos
                .Where(investimento => investimento.ClienteId == ClienteId)
                .Select(investimento => new HistoricoInvestimento(
                    investimento.InvestimentoId,
                    investimento.Produto != null ? investimento.Produto.Tipo.ToString() : "",
                    investimento.Valor,
                    investimento.Rentabilidade,
                    DateOnly.FromDateTime(investimento.Data)
                    )).ToListAsync();
        }

        /// <summary>
        /// Cria um novo registro de investimento para o cliente e produto especificados.
        /// </summary>
        /// <remarks>
        /// Este método requer identificadores válidos de cliente e produto. Se a operação for bem-sucedida, a resposta inclui o histórico de investimentos do investimento recém-criado. Respostas de erro incluem um ID de correlação para solução de problemas.
        /// </remarks>
        /// <param name="Investimento">
        /// Os detalhes do investimento a serem criados, incluindo identificadores de cliente e produto, valor do investimento e informações relacionadas. Não pode ser nulo. O cliente e o produto especificados devem existir.
        /// </param>
        /// <returns>
        /// Um ActionResult contendo o histórico de investimentos criado, caso a operação seja bem-sucedida. 
        /// Retorna 404 se o cliente ou produto não for encontrado, 409 se ocorrer um conflito de concorrência ou 500 para outros erros.
        /// </returns>
        // POST: /[controller]
        //[Authorize]
        [HttpPost]
        [EndpointSummary("Cria um novo registro de investimento para o cliente e produto especificados.")]
        [EndpointDescription("Este método requer identificadores válidos de cliente e produto. Se a operação for bem-sucedida, a resposta inclui o histórico de investimentos do investimento recém-criado. Respostas de erro incluem um ID de correlação para solução de problemas.")]
        [ProducesResponseType<HistoricoInvestimento>(StatusCodes.Status201Created, "application/json")]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict, "application/problem+json")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<HistoricoInvestimento>> PostInvestimento(
            [Description("Os detalhes do investimento a serem criados, incluindo identificadores de cliente e produto, valor do investimento e informações relacionadas. Não pode ser nulo. O cliente e o produto especificados devem existir.")]
            Investimento Investimento)
        {
            var cliente = await Context.Clientes.FindAsync(Investimento.ClienteId);
            if (cliente == null)
            {
                return NotFound("Cliente não encontrado.");
            }

            var produto = await Context.Produtos.FindAsync(Investimento.ProdutoId);
            if (produto == null)
            {
                return NotFound("Produto não encontrado.");
            }

            // Motor básico de recomendação de risco conforme movimentação do cliente
            // TODO: Melhorar motor de recomendação de risco 
            // TODO: Incluir na lógica quantidade de produtos diferentes que o cliente possui e seus volumes
            // TODO: Incluir na lógica flutuação do risco médio do cliente
            // TODO: Colocar em classe separada

            var investimentos = await Context.Investimentos
                .Where(i => i.ClienteId == Investimento.ClienteId)
                .GroupBy(i => i.Produto.Risco <= 1.5M ? "Baixo" : i.Produto.Risco <= 3.0M ? "Médio" : "Alto")
                .Select(g => new
                {
                    Risco = g.Key,
                    TotalInvestido = g.Sum(i => i.Valor),
                    Movimentacoes = g.Count()
                })
                .ToListAsync();

            var riscoBaixo = investimentos.FirstOrDefault(i => i.Risco == "Baixo");
            var riscoMedio = investimentos.FirstOrDefault(i => i.Risco == "Médio");
            var riscoAlto = investimentos.FirstOrDefault(i => i.Risco == "Alto");

            if (produto.Risco > cliente.RiscoMaximo)
            {
                if(cliente.RiscoMaximo <= 1.5M)
                {
                    var investido = ((riscoMedio?.TotalInvestido ?? 0.0M) + ((riscoAlto?.TotalInvestido ?? 0.0M)*2)) / (riscoBaixo?.TotalInvestido ?? 1M);
                    var movimento = ((riscoMedio?.Movimentacoes ?? 0) + ((riscoAlto?.Movimentacoes ?? 0))*2) / (riscoBaixo?.Movimentacoes ?? 1);
                    var acrescimo = (investido > 1 ? 0.05M : 0.0M) + (movimento > 1 ? 0.05M : 0.0M);
                    if (cliente.RiscoMaximo + acrescimo > 5.0M)
                    {
                        cliente.RiscoMaximo = 5.0M;
                    }
                    else
                    {
                        cliente.RiscoMaximo += acrescimo;
                    }
                }
            }
            else
            {
                if (produto.Risco < cliente.RiscoMaximo)
                {
                    if (cliente.RiscoMaximo > 3.0M)
                    {
                        var investido = ((riscoMedio?.TotalInvestido ?? 0.0M) + ((riscoBaixo?.TotalInvestido ?? 0.0M)*2)) / (riscoAlto?.TotalInvestido ?? 1M);
                        var movimento = ((riscoMedio?.Movimentacoes ?? 0) + ((riscoBaixo?.Movimentacoes ?? 0)*2)) / (riscoAlto?.Movimentacoes ?? 1);
                        var decrescimo = (investido > 1 ? 0.05M : 0.0M) + (movimento > 1 ? 0.05M : 0.0M);
                        if (cliente.RiscoMaximo - decrescimo < 1.5M)
                        {
                            cliente.RiscoMaximo = 1.5M;
                        }
                        else
                        {
                            cliente.RiscoMaximo -= decrescimo;
                        }
                    }
                }
            }

            var novoInvestimento = await Context.Investimentos
                    .AddAsync(Investimento);

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(409, "Conflito de concorrência ao criar o investimento.");
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
                nameof(GetInvestimentos),
                new {
                    novoInvestimento.Entity.ClienteId, 
                    novoInvestimento.Entity.InvestimentoId},
                new HistoricoInvestimento(
                    novoInvestimento.Entity.InvestimentoId,
                    novoInvestimento.Entity.Produto != null ? novoInvestimento.Entity.Produto.Tipo.ToString() : "",
                    novoInvestimento.Entity.Valor,
                    novoInvestimento.Entity.Rentabilidade,
                    DateOnly.FromDateTime(novoInvestimento.Entity.Data)));
        }


    }
}
