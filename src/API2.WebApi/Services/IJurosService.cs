using System.Threading.Tasks;

namespace API2.WebApi.Services
{
    /// <summary>
    /// Servico de Juros responsavel por processar tudo que é pertinente a juros. 
    /// </summary>
    public interface IJurosService
    {
        /// <summary>
        /// Calcula o total de juros dado o valor inicial do aporte e número de meses.
        /// </summary>
        /// <param name="valorInicial">Montante inicial a receber juros.</param>
        /// <param name="meses">Quantidade de meses a aplicar o juros.</param>
        /// <returns>O valor final após aplicacao de juros.</returns>
        Task<decimal> CalculaJurosAsync(decimal valorInicial, int meses);
    }
}
