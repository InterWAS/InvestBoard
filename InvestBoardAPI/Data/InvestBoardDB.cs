using Microsoft.EntityFrameworkCore;

namespace InvestBoardAPI.Data
{
    /// <summary>
    /// Representa o contexto de banco de dados do Entity Framework Core para o aplicativo InvestBoard, 
    /// fornecendo acesso a entidades relacionadas a investimentos, como clientes, perfis, produtos e rentabilidade de produtos.
    /// </summary>
    /// <remarks>Este contexto gerencia o modelo de dados de investimento do aplicativo e é usado para consultar e persistir entidades relacionadas a operações de investimento. 
    /// O contexto preenche automaticamente os dados iniciais para perfis, produtos e devoluções de produtos quando o modelo é criado. 
    /// A segurança de threads não é garantida.
    /// </remarks>
    /// <param name="options">
    /// As opções a serem usadas pelo contexto do banco de dados, incluindo configurações como o provedor de banco de dados e a string de conexão. 
    /// Não pode ser nulo.
    /// </param>
    public class InvestBoardDB : DbContext
    {

        public InvestBoardDB(DbContextOptions<InvestBoardDB> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        /// <summary>
        /// Configura o modelo para o contexto aplicando configurações de entidade e preenchendo as tabelas do sistema.
        /// </summary>
        /// <remarks>
        /// Substitui a implementação base para incluir configuração de modelo e dados adicionais de inicialização necessária pelo aplicativo.
        /// </remarks>
        /// <param name="modelBuilder">
        /// Construtor de modelo para o contexto. Fornece configuração para tipos de entidade, relacionamentos e esquema de banco de dados.
        /// </param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Preenche as tabelas do sistema
            SeedTables(modelBuilder);
        }

        /// <summary>
        /// Coleção de Cliente no contexto
        /// </summary>
        /// <remarks>
        /// Para consultar, adicionar, atualizar ou remover registros de Clientes
        /// </remarks>
        public DbSet<Cliente> Clientes { get; set; }
        /// <summary>
        /// Coleção de Perfis no contexto
        /// </summary>
        /// <remarks>
        /// Para consultar, adicionar, atualizar ou remover registros de Perfis
        /// </remarks>
        public DbSet<Perfil> Perfis { get; set; }
        /// <summary>
        /// Coleção de Produtos de Investimento no contexto
        /// </summary>
        /// <remarks>
        /// Para consultar, adicionar, atualizar ou remover registros de Produtos
        /// </remarks>
        public DbSet<Produto> Produtos { get; set; }
        /// <summary>
        /// Coleção de Rentabilidade do Produto no contexto
        /// </summary>
        /// <remarks>
        /// Para consultar, adicionar, atualizar ou remover registros de Rentabilidade do Produto
        /// </remarks>
        public DbSet<Rentabilidade> Rentabilidades { get; set; }

        /// <summary>
        /// Coleção de Telemetria no contexto
        /// </summary>
        /// <remarks>
        /// Para consultar, adicionar, atualizar ou remover registros de Telemetria
        /// </remarks>
        public DbSet<Telemetria> Telemetrias { get; set; }

        /// <summary>
        /// Coleção de entidades de simulação para consulta e salvamento.
        /// </summary>
        /// <remarks>
        /// Para consultar, adicionar, atualizar ou remover registros de simulações de investimento.
        /// </remarks>
        public DbSet<Simulacao> Simulacoes { get; set; }

        /// <summary>
        /// Coleção de entidades de investimento para consulta e armazenamento.
        /// </summary>
        /// <remarks>
        /// Para consultar, adicionar, atualizar ou remover registros de investimento.
        /// </remarks>
        public DbSet<Investimento> Investimentos { get; set; }

        /// <summary>
        /// Preenche as tabelas do sistema
        /// </summary>
        /// <param name="modelBuilder">Construtor de modelo para o contexto</param>
        private void SeedTables(ModelBuilder modelBuilder)
        {
            // Perfil
            SeedPerfil(modelBuilder);

            // Produtos
            SeedProdutos(modelBuilder);
        }

        /// <summary>
        /// Preenche a tabela de perfis com dados iniciais usando <see cref="ModelBuilder.HasData"/>
        /// </summary>
        /// <param name="modelBuilder">
        /// A instância de <see cref="ModelBuilder"/> usada para configurar entidades e inserir dados de inicialização.
        /// Não pode ser nula.
        /// </param>
        /// <remarks>
        /// Este método adiciona dados estáticos (seed) para a entidade Perfil.
        /// - Cada chamada a HasData usa IDs explícitos (<c>PerfilId</c>) para garantir previsibilidade nas migrações.
        /// - Campo <c>RiscoMaximo</c> é decimal e representa score entre 0 e 5.
        /// - O método não realiza validação dos dados. Os valores atendem às restrições das entidades (por exemplo, intervalos de risco).
        /// - Caso seja necessário alterar ou expandir os dados de seed, mantenha os IDs únicos e atualize eventuais migrações relacionadas.
        /// </remarks>
        private void SeedPerfil(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Perfil>().HasData(new Perfil() { 
                PerfilId = 1, 
                Nome = "Conservador", 
                Descricao = "Perfil que busca segurança, foco em liquidez.", 
                RiscoMaximo = 1.5M });
            modelBuilder.Entity<Perfil>().HasData(new Perfil() { 
                PerfilId = 2, 
                Nome = "Moderado", 
                Descricao = "Perfil equilibrado entre segurança e rentabilidade.", 
                RiscoMaximo = 3M });
            modelBuilder.Entity<Perfil>().HasData(new Perfil() { 
                PerfilId = 3, 
                Nome = "Agressivo", 
                Descricao = "Perfil com foco em rentabilidade, aceita maior risco.", 
                RiscoMaximo = 5M });
        }

        /// <summary>
        /// Preenche a tabela de produtos e suas rentabilidades com dados iniciais usando <see cref="ModelBuilder.HasData"/>
        /// </summary>
        /// <param name="modelBuilder">
        /// A instância de <see cref="ModelBuilder"/> usada para configurar entidades e inserir dados de inicialização.
        /// Não pode ser nula.
        /// </param>
        /// <remarks>
        /// Este método adiciona dados estáticos (seed) para as entidades Produto e Rentabilidade.
        /// - Cada chamada a HasData usa IDs explícitos (por exemplo, <c>ProdutoId</c> e <c>RentabilidadeId</c>) para garantir previsibilidade nas migrações.
        /// - Produtos são definidos primeiro e cada rentabilidade referencia o produto via <c>ProdutoId</c>.
        /// - Campos como <c>Valor</c>, <c>FaixaInicial</c> e <c>FaixaFinal</c> são decimais e representam percentuais e limites monetários.
        /// - O método não realiza validação dos dados. Os valores atendem às restrições das entidades (por exemplo, limites de faixa e intervalos de risco).
        /// - Caso seja necessário alterar ou expandir os dados de seed, mantenha os IDs únicos e atualize eventuais migrações relacionadas.
        /// </remarks>
        private void SeedProdutos(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 1, Nome = "LCI CDI", Tipo = Produto.TipoProduto.LCI, Risco = 1M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 1, ProdutoId = 1, Valor = 12.37445M, FaixaInicial = 1000M, FaixaFinal = 99999.99M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 2, ProdutoId = 1, Valor = 12.52345M, FaixaInicial = 100000M, FaixaFinal = 249999.99M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 3, ProdutoId = 1, Valor = 12.67245M, FaixaInicial = 250000M, FaixaFinal = 499999.99M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 4, ProdutoId = 1, Valor = 12.82145M, FaixaInicial = 500000M, FaixaFinal = 999999.99M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 5, ProdutoId = 1, Valor = 12.82145M, FaixaInicial = 1000000M, FaixaFinal = 999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 2, Nome = "LCA CDI", Tipo = Produto.TipoProduto.LCA, Risco = 1M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 6, ProdutoId = 2, Valor = 12.37445M, FaixaInicial = 1000M, FaixaFinal = 99999.99M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 7, ProdutoId = 2, Valor = 12.52345M, FaixaInicial = 100000M, FaixaFinal = 249999.99M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 8, ProdutoId = 2, Valor = 12.67245M, FaixaInicial = 250000M, FaixaFinal = 499999.99M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 9, ProdutoId = 2, Valor = 12.82145M, FaixaInicial = 500000M, FaixaFinal = 999999.99M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 10, ProdutoId = 2, Valor = 12.82145M, FaixaInicial = 1000000M, FaixaFinal = 999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 3, Nome = "CDB CDI", Tipo = Produto.TipoProduto.CDB, Risco = 0.75M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 11, ProdutoId = 3, Valor = 14.155M, FaixaInicial = 200M, FaixaFinal = 2999999.99M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 12, ProdutoId = 3, Valor = 14.453M, FaixaInicial = 3000000M, FaixaFinal = 4999999.99M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 13, ProdutoId = 3, Valor = 14.602M, FaixaInicial = 5000000M, FaixaFinal = 9999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 4, Nome = "RDB CDI", Tipo = Produto.TipoProduto.RDB, Risco = 0.75M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 14, ProdutoId = 4, Valor = 14.155M, FaixaInicial = 200M, FaixaFinal = 2999999.99M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 15, ProdutoId = 4, Valor = 14.453M, FaixaInicial = 3000000M, FaixaFinal = 4999999.99M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 16, ProdutoId = 4, Valor = 14.602M, FaixaInicial = 5000000M, FaixaFinal = 9999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 5, Nome = "CDB FLEX", Tipo = Produto.TipoProduto.CDB, Risco = 1.25M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 17, ProdutoId = 5, Valor = 13.857M, FaixaInicial = 1000M, FaixaFinal = 49999.99M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 18, ProdutoId = 5, Valor = 14.155M, FaixaInicial = 50000M, FaixaFinal = 249999.99M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 19, ProdutoId = 5, Valor = 14.26675M, FaixaInicial = 250000.00M, FaixaFinal = 499999.99M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 20, ProdutoId = 5, Valor = 14.3785M, FaixaInicial = 500000.00M, FaixaFinal = 999999.99M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 21, ProdutoId = 5, Valor = 14.453M, FaixaInicial = 1000000.00M, FaixaFinal = 4999999.99M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 22, ProdutoId = 5, Valor = 14.602M, FaixaInicial = 5000000.00M, FaixaFinal = 9999999.99M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 23, ProdutoId = 5, Valor = 14.751M, FaixaInicial = 10000000.00M, FaixaFinal = 19999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 6, Nome = "RDB FLEX", Tipo = Produto.TipoProduto.RDB, Risco = 1.25M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 24, ProdutoId = 6, Valor = 13.857M, FaixaInicial = 1000M, FaixaFinal = 49999.99M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 25, ProdutoId = 6, Valor = 14.155M, FaixaInicial = 50000M, FaixaFinal = 249999.99M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 26, ProdutoId = 6, Valor = 14.26675M, FaixaInicial = 250000.00M, FaixaFinal = 499999.99M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 27, ProdutoId = 6, Valor = 14.3785M, FaixaInicial = 500000.00M, FaixaFinal = 999999.99M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 28, ProdutoId = 6, Valor = 14.453M, FaixaInicial = 1000000.00M, FaixaFinal = 4999999.99M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 29, ProdutoId = 6, Valor = 14.602M, FaixaInicial = 5000000.00M, FaixaFinal = 9999999.99M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 30, ProdutoId = 6, Valor = 14.751M, FaixaInicial = 10000000.00M, FaixaFinal = 19999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 7, Nome = "CDB PRÉ-FIXADO", Tipo = Produto.TipoProduto.CDB, Risco = 1M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 31, ProdutoId = 7, Valor = 9.938M, FaixaInicial = 1000M, FaixaFinal = 49999.99M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 32, ProdutoId = 7, Valor = 10.016M, FaixaInicial = 50000M, FaixaFinal = 199999.99M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 33, ProdutoId = 7, Valor = 10.055M, FaixaInicial = 200000.00M, FaixaFinal = 499999.99M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 34, ProdutoId = 7, Valor = 10.1136M, FaixaInicial = 500000.00M, FaixaFinal = 999999.99M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 35, ProdutoId = 7, Valor = 10.1527M, FaixaInicial = 1000000.00M, FaixaFinal = 4999999.99M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 36, ProdutoId = 7, Valor = 10.2113M, FaixaInicial = 5000000.00M, FaixaFinal = 9999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 8, Nome = "RDB PRÉ-FIXADO", Tipo = Produto.TipoProduto.RDB, Risco = 1M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 37, ProdutoId = 8, Valor = 9.938M, FaixaInicial = 1000M, FaixaFinal = 49999.99M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 38, ProdutoId = 8, Valor = 10.016M, FaixaInicial = 50000M, FaixaFinal = 199999.99M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 39, ProdutoId = 8, Valor = 10.055M, FaixaInicial = 200000.00M, FaixaFinal = 499999.99M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 40, ProdutoId = 8, Valor = 10.1136M, FaixaInicial = 500000.00M, FaixaFinal = 999999.99M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 41, ProdutoId = 8, Valor = 10.1527M, FaixaInicial = 1000000.00M, FaixaFinal = 4999999.99M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 42, ProdutoId = 8, Valor = 10.2113M, FaixaInicial = 5000000.00M, FaixaFinal = 9999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 9, Nome = "FIC FIF ACOES E FUNDO IBOVESPA", Tipo = Produto.TipoProduto.Fundo, Risco = 3.5M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 43, ProdutoId = 9, Valor = 9.52M, FaixaInicial = 0.01M, FaixaFinal = 9999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 10, Nome = "FIF ACOES DIVIDENDOS", Tipo = Produto.TipoProduto.Fundo, Risco = 3.5M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 44, ProdutoId = 10, Valor = 12.31M, FaixaInicial = 0.01M, FaixaFinal = 9999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 11, Nome = "FIF ACOES CONSUMO", Tipo = Produto.TipoProduto.Fundo, Risco = 3.5M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 45, ProdutoId = 11, Valor = 4.72M, FaixaInicial = 0.01M, FaixaFinal = 9999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 12, Nome = "FIF ACOES VALE DO RIO DOCE", Tipo = Produto.TipoProduto.Fundo, Risco = 4M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 46, ProdutoId = 12, Valor = -8.15M, FaixaInicial = 0.01M, FaixaFinal = 9999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 13, Nome = "FIF ACOES BDR NIVEL I", Tipo = Produto.TipoProduto.Fundo, Risco = 3.5M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 47, ProdutoId = 13, Valor = 9.47M, FaixaInicial = 0.01M, FaixaFinal = 9999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 14, Nome = "FIF ACOES SMALL CAPS ATIVO", Tipo = Produto.TipoProduto.Fundo, Risco = 3.5M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 48, ProdutoId = 14, Valor = 3.7M, FaixaInicial = 0.01M, FaixaFinal = 9999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 15, Nome = "EXPERT CLARITAS VALOR FIC FIF AÇÕES", Tipo = Produto.TipoProduto.Fundo, Risco = 3.5M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 49, ProdutoId = 15, Valor = 7.42M, FaixaInicial = 10000.00M, FaixaFinal = 9999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 16, Nome = "FIF E-SIMPLES RENDA FIXA LP", Tipo = Produto.TipoProduto.Fundo, Risco = 0.5M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 50, ProdutoId = 16, Valor = 10.34M, FaixaInicial = 0.01M, FaixaFinal = 9999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 17, Nome = "FIC FIF FACIL RF SIMPLES", Tipo = Produto.TipoProduto.Fundo, Risco = 0.5M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 51, ProdutoId = 17, Valor = 10.06M, FaixaInicial = 0.01M, FaixaFinal = 9999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 18, Nome = "FIC FIF BETA REF DI LONGO PRAZO", Tipo = Produto.TipoProduto.Fundo, Risco = 1M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 52, ProdutoId = 18, Valor = 10.59M, FaixaInicial = 0.01M, FaixaFinal = 9999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 19, Nome = "FIC FIF SIGMA RF REF DI LONGO PRAZO", Tipo = Produto.TipoProduto.Fundo, Risco = 1M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 53, ProdutoId = 19, Valor = 11.87M, FaixaInicial = 50000.00M, FaixaFinal = 9999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 20, Nome = "FIC FIF OMEGA PRIVATE RF REF DI LP", Tipo = Produto.TipoProduto.Fundo, Risco = 1M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 54, ProdutoId = 20, Valor = 12.02M, FaixaInicial = 100000.00M, FaixaFinal = 9999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 21, Nome = "FIC FIF ABSOLUTO PRE RF LONGO PRAZO", Tipo = Produto.TipoProduto.Fundo, Risco = 1.75M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 55, ProdutoId = 21, Valor = 8.48M, FaixaInicial = 0.01M, FaixaFinal = 9999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 22, Nome = "FIC FIF OBJETIVO PRE RF LP", Tipo = Produto.TipoProduto.Fundo, Risco = 1.75M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 56, ProdutoId = 22, Valor = 9.15M, FaixaInicial = 1000.00M, FaixaFinal = 9999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 23, Nome = "FIC FIF CAPITAL INDICE DE PRECOS RF LP", Tipo = Produto.TipoProduto.Fundo, Risco = 1.75M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 57, ProdutoId = 23, Valor = 6.94M, FaixaInicial = 0.01M, FaixaFinal = 9999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 24, Nome = "FIC FIF PERFORMANCE IMA B RF LP", Tipo = Produto.TipoProduto.Fundo, Risco = 2M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 58, ProdutoId = 24, Valor = 5.55M, FaixaInicial = 1000.00M, FaixaFinal = 9999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 25, Nome = "FIF MEGA RF REF DI LONGO PRAZO", Tipo = Produto.TipoProduto.Fundo, Risco = 1M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 59, ProdutoId = 25, Valor = 12.29M, FaixaInicial = 100000.00M, FaixaFinal = 9999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 26, Nome = "FIC FIF PLENO REF DI LP", Tipo = Produto.TipoProduto.Fundo, Risco = 1M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 60, ProdutoId = 26, Valor = 11.77M, FaixaInicial = 10000.00M, FaixaFinal = 9999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 27, Nome = "FIC FIF GIRO IMEDIATO REF DI LP", Tipo = Produto.TipoProduto.Fundo, Risco = 1M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 61, ProdutoId = 27, Valor = 10.45M, FaixaInicial = 1000.00M, FaixaFinal = 9999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 28, Nome = "FIC FIF EXPERTISE RF CRED PRIV LP", Tipo = Produto.TipoProduto.Fundo, Risco = 2M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 62, ProdutoId = 28, Valor = 12.24M, FaixaInicial = 10000.00M, FaixaFinal = 9999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 29, Nome = "FIC FIF MAXI RENDA FIXA CRED PRIV LP", Tipo = Produto.TipoProduto.Fundo, Risco = 2M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 63, ProdutoId = 29, Valor = 12.47M, FaixaInicial = 200000.00M, FaixaFinal = 9999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 30, Nome = "FIF QUALIFICADO RF CRED PRIV LP", Tipo = Produto.TipoProduto.Fundo, Risco = 1.75M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 64, ProdutoId = 30, Valor = 12.39M, FaixaInicial = 10000.00M, FaixaFinal = 9999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 31, Nome = "FIC FIF ATIVA RENDA FIXA LP", Tipo = Produto.TipoProduto.Fundo, Risco = 1.75M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 65, ProdutoId = 31, Valor = 11.81M, FaixaInicial = 50000.00M, FaixaFinal = 9999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 32, Nome = "FIC FIF IDEAL RF LONGO PRAZO", Tipo = Produto.TipoProduto.Fundo, Risco = 1.75M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 66, ProdutoId = 32, Valor = 10.91M, FaixaInicial = 0.01M, FaixaFinal = 9999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 33, Nome = "FIC FIF INVESTIDOR RF LONGO PRAZO", Tipo = Produto.TipoProduto.Fundo, Risco = 1.75M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 67, ProdutoId = 33, Valor = 11.7M, FaixaInicial = 5000.00M, FaixaFinal = 9999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 34, Nome = "FIC FIF ESPECIAL RF LONGO PRAZO", Tipo = Produto.TipoProduto.Fundo, Risco = 1.75M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 68, ProdutoId = 34, Valor = 12.2M, FaixaInicial = 100000.00M, FaixaFinal = 9999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 35, Nome = "FIC FIF RENDA FIXA CURTO PRAZO", Tipo = Produto.TipoProduto.Fundo, Risco = 1M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 69, ProdutoId = 35, Valor = 11.57M, FaixaInicial = 10000.00M, FaixaFinal = 9999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 36, Nome = "FIF INFLACAO PRIVATE 2027 RF LONGO PRAZO - RL", Tipo = Produto.TipoProduto.Fundo, Risco = 1M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 70, ProdutoId = 36, Valor = 10.27M, FaixaInicial = 10000.00M, FaixaFinal = 9999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 37, Nome = "FIC FIF JUROS E MOEDAS MUL PLUS LP", Tipo = Produto.TipoProduto.Fundo, Risco = 2.25M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 71, ProdutoId = 37, Valor = 11.05M, FaixaInicial = 0.01M, FaixaFinal = 9999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 38, Nome = "FIF RV 30 MM LONGO PRAZO", Tipo = Produto.TipoProduto.Fundo, Risco = 2.5M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 72, ProdutoId = 38, Valor = 7.95M, FaixaInicial = 0.01M, FaixaFinal = 9999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 39, Nome = "FIC FOF SMART MULTIESTRATEGIA MM", Tipo = Produto.TipoProduto.Fundo, Risco = 2.5M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 73, ProdutoId = 39, Valor = 11.7M, FaixaInicial = 0.01M, FaixaFinal = 9999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 40, Nome = "FIF INDEXA OURO MULTIMERCADO LP", Tipo = Produto.TipoProduto.Fundo, Risco = 2.5M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 74, ProdutoId = 40, Valor = 46.59M, FaixaInicial = 0.01M, FaixaFinal = 9999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 41, Nome = "FIC FIF INDEXA DOLAR CAMBIAL", Tipo = Produto.TipoProduto.Fundo, Risco = 3M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 75, ProdutoId = 41, Valor = 5.24M, FaixaInicial = 0.01M, FaixaFinal = 9999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 42, Nome = "FIC FIF ALOCACAO MACRO MM LP", Tipo = Produto.TipoProduto.Fundo, Risco = 2.5M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 76, ProdutoId = 42, Valor = 8.84M, FaixaInicial = 10000.00M, FaixaFinal = 9999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 43, Nome = "FIC FIF ESTRAT. LIVRE MULTIMERCADO LP", Tipo = Produto.TipoProduto.Fundo, Risco = 3.5M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 77, ProdutoId = 43, Valor = 4.6M, FaixaInicial = 0.01M, FaixaFinal = 9999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 44, Nome = "FIC FIF PLUS QUALIFICADO MM CRED PRIV LP", Tipo = Produto.TipoProduto.Fundo, Risco = 3.75M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 78, ProdutoId = 44, Valor = 12.46M, FaixaInicial = 10000.00M, FaixaFinal = 9999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 45, Nome = "FIF INDEXA BOLSA AMERICANA MULTIMERCADO LP", Tipo = Produto.TipoProduto.Fundo, Risco = 3.5M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 79, ProdutoId = 45, Valor = 17.69M, FaixaInicial = 0.01M, FaixaFinal = 9999999999.99M });
            modelBuilder.Entity<Produto>().HasData(new Produto() { ProdutoId = 46, Nome = "FIC FIF FOF SMART LONG SHORT LP", Tipo = Produto.TipoProduto.Fundo, Risco = 3.5M });
            modelBuilder.Entity<Rentabilidade>().HasData(new Rentabilidade() { RentabilidadeId = 80, ProdutoId = 46, Valor = 8.67M, FaixaInicial = 0.01M, FaixaFinal = 9999999999.99M });
        }

    }
}
