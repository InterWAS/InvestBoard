namespace InvestBoardAPI.Utils
{
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
