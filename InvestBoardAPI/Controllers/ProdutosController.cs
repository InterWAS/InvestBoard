using InvestBoardAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InvestBoardAPI.Controllers
{
    /// <summary>
    /// Fornece endpoints de API HTTP para gerenciar e recuperar dados de produtos.
    /// </summary>
    /// <remarks>
    /// Este controlador expõe endpoints para interação com produtos armazenados no banco de dados. 
    /// Ele está configurado como um controlador de API e utiliza roteamento baseado no nome do controlador. 
    /// Todas as ações requerem um contexto de banco de dados válido para funcionar corretamente.
    /// </remarks>
    /// <param name="Context">O contexto do banco de dados usado para acessar as informações do produto.</param>
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController(InvestBoardDB Context) : ControllerBase
    {
        /// <summary>
        /// Recupera todos os produtos do banco de dados.
        /// </summary>
        /// <remarks>
        /// Este método realiza uma consulta assíncrona ao banco de dados para obter todos os produtos. 
        /// </remarks>
        /// <returns>
        /// Um <see cref="ActionResult{T}"/> contendo uma coleção de objetos <see cref="Produto"/>. 
        /// A coleção estará vazia se não existirem produtos.
        /// </returns>
        // GET: /[controller]
        //[Authorize]
        [HttpGet]
        [EndpointSummary("Recupera todos os produtos do banco de dados.")]
        [EndpointDescription("Este método realiza uma consulta assíncrona ao banco de dados para obter todos os produtos.")]
        [ProducesResponseType<IEnumerable<Produto>>(StatusCodes.Status200OK, "application/json")]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos()
        {
            return await Context.Produtos.ToListAsync();
        }

    }
}
