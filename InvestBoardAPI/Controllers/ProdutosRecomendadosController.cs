using InvestBoardAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace InvestBoardAPI.Controllers
{
    /// <summary>
    /// Fornece endpoints de API para recuperar produtos recomendados com base no perfil de risco do usuário.
    /// </summary>
    /// <remarks>
    /// Este controlador expõe operações para obter recomendações de produtos personalizadas 
    /// de acordo com a tolerância ao risco de um perfil de usuário específico.
    /// Todos os endpoints exigem um identificador de perfil válido 
    /// e retornam produtos filtrados de acordo com o risco máximo permitido pelo perfil.
    /// </remarks>                                                                                                                                                                                                                                                                                                             
    /// <param name="Context">O contexto do banco de dados usado para acessar perfis de usuário e informações de produto. Não pode ser nulo.</param>
    [Route("produtos-recomendados")]
    [ApiController]
    public partial class ProdutosRecomendadosController(InvestBoardDB Context) : ControllerBase
    {

        /// <summary>
        /// Retorna uma lista de produtos recomendados para o perfil de usuário especificado, filtrados pela tolerância máxima ao risco do perfil.
        /// </summary>
        /// <remarks>
        /// Os produtos recomendados são selecionados com base no nível de risco permitido pelo perfil especificado. A classificação de risco e a rentabilidade em 12 meses estão incluídas no resultado para cada produto.
        /// </remarks>
        /// <param name="PerfilId">
        /// O identificador único do perfil de usuário para o qual os produtos recomendados são solicitados. Deve ser maior que zero.
        /// </param>
        /// <returns>
        /// Um ​​resultado de ação assíncrona contendo uma coleção de produtos recomendados que correspondem aos critérios de risco do perfil.
        /// Retorna um resultado de solicitação inválida se o ID do perfil for inválido ou um resultado de não encontrado se o perfil não existir.
        /// </returns>
        // GET: /[controller]/{Perfil}
        //[Authorize]
        [HttpGet("{PerfilId}")]
        [EndpointSummary("Retorna uma lista de produtos recomendados para o perfil de usuário especificado, filtrados pela tolerância máxima ao risco do perfil.")]
        [EndpointDescription("Os produtos recomendados são selecionados com base no nível de risco permitido pelo perfil especificado. A classificação de risco e a rentabilidade em 12 meses estão incluídas no resultado para cada produto.")]
        [ProducesResponseType<IEnumerable<ProdutoRecomendado>>(StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
        public async Task<ActionResult<IEnumerable<ProdutoRecomendado>>> GetProdutosRecomendados(
            [Description("O identificador único do perfil de usuário para o qual os produtos recomendados são solicitados. Deve ser maior que zero.")]
            int PerfilId)
        {
            if (PerfilId <= 0)
            {
                return BadRequest("ID Perfil inválido.");
            }
            var perfil = await Context.Perfis.FindAsync(PerfilId);
            if (perfil == null)
            {
                return NotFound("Perfil não encontrado.");
            }

            return await Context.Produtos
                .Where(produto => produto.Risco <= perfil.RiscoMaximo)
                .Select(produto => new ProdutoRecomendado(
                    ProdutoId: produto.ProdutoId,
                    Nome: produto.Nome,
                    Tipo: produto.Tipo.ToString(),
                    Rentabilidade12Meses: (produto.Rentabilidades != null ? produto.Rentabilidades.First().Valor : 0.0M),
                    Risco: produto.Risco <= 1.5M ? "Baixo" : produto.Risco <= 3.0M ? "Médio" : "Alto"
                ))
                .ToListAsync();
        }

    }
}
