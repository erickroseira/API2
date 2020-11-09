using System.Threading.Tasks;

namespace API2.WebApi.Services
{
    /// <summary>
    /// Interface para WebClient.
    /// </summary>
    public interface IWebClientService
    {
        /// <summary>
        /// Realiza um GET HTTP request.
        /// </summary>
        /// <typeparam name="TResponse">O tipo da resposta.</typeparam>
        /// <param name="uri">O path do recurso.</param>
        /// <returns>A resposta.</returns>
        Task<TResponse> GetRequest<TResponse>(string uri);
    }
}
