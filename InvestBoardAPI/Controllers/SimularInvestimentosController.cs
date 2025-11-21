using InvestBoardAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using static InvestBoardAPI.Utils.Calculos;

namespace InvestBoardAPI.Controllers
{
    /// <summary>
    /// Controller responsável por processar solicitações de simulação de investimentos, validando dados do cliente e do produto, 
    /// e retornando os resultados calculados conforme os parâmetros informados.
    /// </summary>
    /// <remarks>
    /// Utilize este controller para realizar simulações de investimentos baseadas em diferentes
    /// tipos de produtos, valores e prazos. Todas as operações dependem da existência prévia do cliente e do produto no
    /// banco de dados. O controller retorna respostas detalhadas sobre o produto validado, o resultado da simulação e a
    /// data da operação. 
    /// </remarks>
    /// <param name="Context">
    /// O contexto do banco de dados utilizado para acessar informações de clientes, produtos e rentabilidades durante o
    /// processamento das simulações. Não pode ser nulo.
    /// </param>
    [Route("simular-investimento")]
    [ApiController]
    public partial class SimularInvestimentosController(InvestBoardDB Context) : ControllerBase
    {
        /// <summary>
        /// Realiza uma simulação de investimento com base no tipo de produto, valor e prazo especificados, retornando os resultados calculados e os detalhes do produto.
        /// </summary>
        /// <remarks>
        /// Este método valida os parâmetros de entrada e garante a existência do cliente e do produto antes da realização da simulação. O cálculo utiliza a taxa de rendimento do produto aplicável à quantidade e ao prazo especificados. A resposta inclui o valor final, o rendimento efetivo e a classificação de risco do produto.
        /// </remarks>
        /// <param name="Pedido">
        /// A solicitação de simulação de investimento contém o identificador do cliente, o valor do investimento, o prazo em meses e o tipo de produto. O valor e o prazo do investimento devem ser maiores que zero.
        /// </param>
        /// <returns>
        /// Um ActionResult contendo a resposta da simulação com informações do produto e resultados de investimento calculados. 
        /// Retorna um BadRequest se os valores de entrada forem inválidos ou NotFound se o cliente, 
        /// produto ou rendimento aplicável não for encontrado.
        /// </returns>
        // POST: /[controller]
        //[Authorize]
        [HttpPost]
        [EndpointSummary("Realiza uma simulação de investimento com base no tipo de produto, valor e prazo especificados, retornando os resultados calculados e os detalhes do produto.")]
        [EndpointDescription("Este método valida os parâmetros de entrada e garante a existência do cliente e do produto antes da realização da simulação. O cálculo utiliza a taxa de rendimento do produto aplicável à quantidade e ao prazo especificados. A resposta inclui o valor final, o rendimento efetivo e a classificação de risco do produto.")]
        [ProducesResponseType<RespostaSimulacao>(StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict, "application/problem+json")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RespostaSimulacao>> SimularInvestimento(
            [Description("A solicitação de simulação de investimento contém o identificador do cliente, o valor do investimento, o prazo em meses e o tipo de produto. O valor e o prazo do investimento devem ser maiores que zero.")]
            PedidoSimulacaoTipoProduto Pedido)
        {
            if (Pedido.Valor <= 0)
            {
                return BadRequest("O valor do investimento deve ser maior que zero.");
            }

            if (Pedido.PrazoMeses <= 0)
            {
                return BadRequest("O prazo em meses deve ser maior que zero.");
            }

            var cliente = await Context.Clientes.FindAsync(Pedido.ClienteId);
            if (cliente == null)
            {
                return NotFound("Cliente não encontrado.");
            }

            var produto = await Context.Produtos
                .FirstOrDefaultAsync(produto => produto.Tipo.ToString().ToLower() == Pedido.TipoProduto.ToLower());
            if (produto == null)
            {
                return NotFound("Produto não encontrado para o tipo de produto informado.");
            }

            var rentabilidade = produto.Rentabilidades?
                .FirstOrDefault(rentabilidade => (rentabilidade.FaixaInicial <= Pedido.Valor && rentabilidade.FaixaFinal >= Pedido.Valor));
            if (rentabilidade == null)
            {
                return NotFound("Rentabilidade não encontrada para o produto selecionado.");
            }

            // Lógica de simulação de investimento
            decimal taxaMensal = rentabilidade.Valor.TaxaAnualParaMensal().ArredondarDoisDecimais();
            decimal valorFinal = (Pedido.Valor * (decimal)Math.Pow((double)(1 + (taxaMensal / 100)), Pedido.PrazoMeses)).ArredondarDoisDecimais();
            decimal rentabilidadeEfetiva = (((valorFinal - Pedido.Valor) / Pedido.Valor) * 100).ArredondarDoisDecimais();

            // Salva simulação no banco de dados
            await Context.Simulacoes.AddAsync(new Simulacao
                {
                    ClienteId = Pedido.ClienteId,
                    ProdutoId = produto.ProdutoId,
                    ValorInvestido = Pedido.Valor,
                    ValorFinal =  valorFinal,
                    PrazoMeses = Pedido.PrazoMeses,
                    DataSimulacao = DateOnly.FromDateTime(DateTime.Now),
                    RentabilidadeEfetiva = rentabilidadeEfetiva
                });

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(409, "Conflito de concorrência ao salvar simulação.");
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

            return new ActionResult<RespostaSimulacao>( new RespostaSimulacao(
                new ProdutoValidado(
                    produto.ProdutoId,
                    produto.Nome,
                    produto.Tipo.ToString(),
                    rentabilidade.Valor.ArredondarDoisDecimais(),
                    produto.Risco <= 1.5M ? "Baixo" : produto.Risco <= 3.0M ? "Médio" : "Alto"
                ),
                new ResultadoSimulacao(
                    valorFinal,
                    rentabilidadeEfetiva,
                    Pedido.PrazoMeses
                ),
                DateTime.Now
            ));
        }

        /// <summary>
        /// Simula um investimento para um produto e cliente específicos, retornando os resultados projetados com base no valor e prazo do investimento fornecidos.
        /// </summary>
        /// <remarks>
        /// Este método valida os parâmetros de entrada e garante que o cliente e o produto existam antes de realizar a simulação. O cálculo utiliza o rendimento do produto aplicável ao valor de investimento especificado. A resposta inclui o valor projetado final, o rendimento efetivo e a classificação de risco do produto. Este endpoint destina-se ao uso em cenários de investimento financeiro onde os usuários desejam estimar os retornos de um produto específico.
        /// </remarks>
        /// <param name="Pedido">
        /// A solicitação de simulação de investimento contém o identificador do cliente, o identificador do produto, o valor do investimento e o prazo em meses. O valor do investimento e o prazo devem ser maiores que zero.
        /// </param>
        /// <returns>
        /// Um ActionResult contendo o resultado da simulação, incluindo informações validadas do produto e o resultado do investimento calculado. 
        /// Retorna um BadRequest se o valor ou prazo do investimento for inválido, ou NotFound se o cliente, produto ou rendimento aplicável não for encontrado.
        /// </returns>
        // POST: /[controller]/produto
        //[Authorize]
        [HttpPost("produto")]
        [EndpointSummary("Simula um investimento para um produto e cliente específicos, retornando os resultados projetados com base no valor e prazo do investimento fornecidos.")]
        [EndpointDescription("Este método valida os parâmetros de entrada e garante que o cliente e o produto existam antes de realizar a simulação. O cálculo utiliza o rendimento do produto aplicável ao valor de investimento especificado. A resposta inclui o valor projetado final, o rendimento efetivo e a classificação de risco do produto. Este endpoint destina-se ao uso em cenários de investimento financeiro onde os usuários desejam estimar os retornos de um produto específico.")]
        [ProducesResponseType<RespostaSimulacao>(StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict, "application/problem+json")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RespostaSimulacao>> SimularInvestimentoProduto(
            [Description("A solicitação de simulação de investimento contém o identificador do cliente, o identificador do produto, o valor do investimento e o prazo em meses. O valor do investimento e o prazo devem ser maiores que zero.")]
            PedidoSimulacaoProduto Pedido)
        {
            if (Pedido.Valor <= 0)
            {
                return BadRequest("O valor do investimento deve ser maior que zero.");
            }

            if (Pedido.PrazoMeses <= 0)
            {
                return BadRequest("O prazo em meses deve ser maior que zero.");
            }

            var cliente = await Context.Clientes.FindAsync(Pedido.ClienteId);
            if (cliente == null)
            {
                return NotFound("Cliente não encontrado.");
            }

            var produto = await Context.Produtos.FindAsync(Pedido.ProdutoId);
            if (produto == null)
            {
                return NotFound("Produto não encontrado.");
            }

            var rentabilidade = produto.Rentabilidades?
                .FirstOrDefault(rentabilidade => (rentabilidade.FaixaInicial <= Pedido.Valor && rentabilidade.FaixaFinal >= Pedido.Valor));
            if (rentabilidade == null)
            {
                return NotFound("Rentabilidade não encontrada para o produto selecionado.");
            }

            // Lógica de simulação de investimento
            decimal taxaMensal = rentabilidade.Valor.TaxaAnualParaMensal().ArredondarDoisDecimais();
            decimal valorFinal = (Pedido.Valor * (decimal)Math.Pow((double)(1 + (taxaMensal / 100)), Pedido.PrazoMeses)).ArredondarDoisDecimais();
            decimal rentabilidadeEfetiva = (((valorFinal - Pedido.Valor) / Pedido.Valor) * 100).ArredondarDoisDecimais();

            // Salva simulação no banco de dados
            await Context.Simulacoes.AddAsync(new Simulacao
            {
                ClienteId = Pedido.ClienteId,
                ProdutoId = produto.ProdutoId,
                ValorInvestido = Pedido.Valor,
                ValorFinal = valorFinal,
                PrazoMeses = Pedido.PrazoMeses,
                DataSimulacao = DateOnly.FromDateTime(DateTime.Now),
                RentabilidadeEfetiva = rentabilidadeEfetiva
            });

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(409, "Conflito de concorrência ao salvar simulação.");
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

            return new ActionResult<RespostaSimulacao>(new RespostaSimulacao(
                new ProdutoValidado(
                    produto.ProdutoId,
                    produto.Nome,
                    produto.Tipo.ToString(),
                    rentabilidade.Valor.ArredondarDoisDecimais(),
                    produto.Risco <= 1.5M ? "Baixo" : produto.Risco <= 3.0M ? "Médio" : "Alto"
                ),
                new ResultadoSimulacao(
                    valorFinal,
                    rentabilidadeEfetiva,
                    Pedido.PrazoMeses
                ),
                DateTime.Now
            ));
        }
    }
}
