namespace InvestBoardAPI.Utils
{
    /// <summary>
    /// Fornece métodos de extensão para realizar cálculos financeiros comuns com valores decimais, como conversões percentuais, 
    /// arredondamento e cálculos de rentabilidade efetiva.
    /// </summary>
    /// <remarks>
    /// Esta classe estática contém métodos de extensão que simplificam operações financeiras envolvendo números decimais. 
    /// Os métodos incluem a conversão de valores para e de porcentagens, arredondamento para duas casas decimais, 
    /// cálculo de taxas mensais a partir de taxas anuais e determinação da rentabilidade efetiva. 
    /// Todos os métodos são projetados para serem usados ​​como extensões do tipo decimal, 
    /// permitindo um código fluente e legível em contextos financeiros.
    /// </remarks>
    public static class Calculos
    {

        // Extension block
        extension(decimal source) // extension members for 
        {
            public decimal ParaPorcentagem() => source * 100;

            public decimal DePorcentagem() => source / 100;

            public decimal ArredondarDoisDecimais() => Math.Round(source, 2);

            public decimal TaxaAnualParaMensal() => (decimal)((Math.Pow((double)(1 + (source / 100)), 1.0 / 12.0) - 1) * 100);

            public decimal CalcularRentabilidadeEfetiva(decimal valorFinal)
            {
                if (source <= 0)
                {
                    throw new ArgumentException("O valor investido deve ser maior que zero.", nameof(source));
                }
                return (valorFinal - source) / source;
            }
        }
    }
}
