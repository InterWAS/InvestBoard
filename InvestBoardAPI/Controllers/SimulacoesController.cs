using InvestBoardAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InvestBoardAPI.Controllers
{
    /// <summary>
    /// Provides API endpoints for managing and retrieving simulation history records.
    /// </summary>
    /// <remarks>
    /// Este controlador expõe endpoints para acessar o histórico de simulação, para
    /// recuperar detalhes de simulações passadas, como informações do cliente, 
    /// produto, valor investido, valor final, prazo, e data da simulação. 
    /// O acesso a esses endpoints pode ser restrito com base nas políticas de 
    /// autorização configuradas para o aplicativo.
    /// </remarks>
    /// <param name="Context">O contexto do banco de dados usado para acessar os dados da simulação e as entidades relacionadas.</param>
    [Route("simulacoes")]
    [ApiController]
    public partial class SimulacoesController(InvestBoardDB Context) : ControllerBase
    {
        /// <summary>
        /// Recupera uma coleção de registros do histórico de simulações para todas as simulações.
        /// </summary>
        /// <remarks>
        /// Endpoint usado para obter uma lista de todas as simulações realizadas, incluindo detalhes como cliente, produto, valor investido, valor final, prazo e data da simulação. O acesso pode ser restrito a usuários autorizados, dependendo da configuração do controlador.
        /// </remarks>
        /// <returns>
        /// Uma operação assíncrona que retorna um <see cref="ActionResult{T}"/> 
        /// contendo uma coleção enumerável de objetos <see cref="HistoricoSimulacao"/> 
        /// representando o histórico de cada simulação.
        /// </returns>
        // GET: /[controller]
        //[Authorize("Admin")]
        [HttpGet]
        [EndpointSummary("Recupera uma coleção de registros do histórico de simulações para todas as simulações.")]
        [EndpointDescription("Endpoint usado para obter uma lista de todas as simulações realizadas, incluindo detalhes como cliente, produto, valor investido, valor final, prazo e data da simulação. O acesso pode ser restrito a usuários autorizados, dependendo da configuração do controlador.")]
        [ProducesResponseType<IEnumerable<HistoricoSimulacao>>(StatusCodes.Status200OK, "application/json")]
        public async Task<ActionResult<IEnumerable<HistoricoSimulacao>>> GetSimulacoes()
        {
            return await Context.Simulacoes
                .Select(simulacao => new HistoricoSimulacao(
                    simulacao.SimulacaoId,
                    simulacao.ClienteId,
                    simulacao.Produto != null ? simulacao.Produto.Nome : "",
                    simulacao.ValorInvestido,
                    simulacao.ValorFinal,
                    simulacao.PrazoMeses,
                    simulacao.DataSimulacao
                    )).ToListAsync();
        }

        /// <summary>
        /// Recupera uma coleção de registros históricos de simulação agrupados por produto e data da simulação.
        /// </summary>
        /// <remarks>
        /// Cada item na coleção retornada inclui o nome do produto, a data da simulação, o número total de simulações e o valor médio final para esse grupo. O acesso a este endpoint pode ser restrito a usuários autorizados, dependendo da configuração do controlador.
        /// </remarks>
        /// <returns>
        /// Um objeto "<see cref="ActionResult{T}"/>" contendo uma lista de objetos "<see cref="SimulacaoProdutoDia"/>". Cada objeto representa dados de simulação agregados para um produto e data específicos. Retorna uma lista vazia se nenhuma simulação for encontrada.
        /// </returns>
        // GET: /[controller]/por-produto-dia
        //[Authorize("Admin")]
        [HttpGet("por-produto-dia")]
        [EndpointSummary("Recupera uma coleção de registros históricos de simulação agrupados por produto e data da simulação.")]
        [EndpointDescription("Cada item na coleção retornada inclui o nome do produto, a data da simulação, o número total de simulações e o valor médio final para esse grupo. O acesso a este endpoint pode ser restrito a usuários autorizados, dependendo da configuração do controlador.")]
        [ProducesResponseType<IEnumerable<SimulacaoProdutoDia>>(StatusCodes.Status200OK, "application/json")]
        public async Task<ActionResult<IEnumerable<SimulacaoProdutoDia>>> GetSimulacoesPorProdutoDia()
        {
            return await Context.Simulacoes
                .Include(simulacao => simulacao.Produto)
                .GroupBy(
                    simulacao => new { simulacao.DataSimulacao, simulacao.Produto.Nome})
                .Select(
                    simulacao => new SimulacaoProdutoDia(
                        Produto: simulacao.Key.Nome, 
                        Data: simulacao.Key.DataSimulacao, 
                        QuantidadeSimulacoes: simulacao.Count(), 
                        MediaValorFinal: simulacao.Average(s => s.ValorFinal)))
                .ToListAsync();
        }
    }
}
