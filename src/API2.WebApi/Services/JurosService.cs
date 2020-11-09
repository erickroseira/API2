using System;
using System.Threading.Tasks;
using API2.WebApi.Models;
using API2.WebApi.Models.Response;
using Microsoft.Extensions.Configuration;

namespace API2.WebApi.Services
{
    /// <summary>
    /// Implementação para <see cref="IJurosService"/>.
    /// </summary>
    public class JurosService : IJurosService
    {
        private readonly ILoggingService<JurosService> _loggingService;
        private readonly IWebClientService _webClientService;
        private readonly IConfiguration _config;

        /// <summary>
        /// Construtor do serviço de Juros.
        /// </summary>
        /// <param name="loggingService">Instância do Serviço de Logging.</param>
        /// <param name="config">Instance of IConfiuration que armazena a configuração do appsettings.json</param>
        /// <param name="webClientService">Cliente Web para realização de requisições HTTP.</param>
        public JurosService(ILoggingService<JurosService> loggingService, IConfiguration config, IWebClientService webClientService)
        {
            _loggingService = loggingService;
            _config = config;
            _webClientService = webClientService;
        }

        /// <see cref="IJurosService.CalculaJurosAsync"/>
        public async Task<decimal> CalculaJurosAsync(decimal valorInicial, int meses)
        {
            try
            {
                _loggingService.LogInfo($"Calculando Juros para os seguintes parâmetros { new { valorInicial, meses} }.");

                var taxaJurosResponse = await _webClientService.GetRequest<Response<Juros>>($"{_config.GetValue<string>("API1Uri")}{Endpoints.TaxaJuros}");
                var juros = (double) taxaJurosResponse.Conteudo.Taxa;

                var valorFinal = valorInicial * (decimal) Math.Pow(1 + juros, meses);
                var valorFinalTruncado = Math.Truncate(valorFinal * 100) / 100;

                return valorFinalTruncado;
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Exceção ocorreu. {ex.Message}", ex);
                
                throw;
            }
        }
    }
}
