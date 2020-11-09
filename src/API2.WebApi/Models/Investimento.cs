namespace API2.WebApi.Models
{
    /// <summary>
    /// Representa o Investimento.
    /// </summary>
    public class Investimento
    {
        /// <summary>
        /// Gets or sets valor inicial do investimento.
        /// </summary>
        public decimal ValorInicial { get; set; }

        /// <summary>
        /// Gets or sets o valor final do investimento após aplicar juros.
        /// </summary>
        public decimal ValorFinal { get; set; }
    }
}
