using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using API2.WebApi.Helpers;
using API2.WebApi.Models;
using API2.WebApi.Models.Response;
using API2.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace API2.WebApi.Controllers
{
    /// <summary>
    /// Recebe todos os requests relacionados a Juros.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class JurosController : ControllerBase
    {
        private readonly ILoggingService<JurosController> _loggingService;
        private readonly IJurosService _jurosService;

        /// <summary>
        /// Construtor do JurosController.
        /// </summary>
        /// <param name="loggingService">Instância do Serviço de Logging.</param>
        /// <param name="jurosService">Instancia do serviço de Juros.</param>
        public JurosController(ILoggingService<JurosController> loggingService, IJurosService jurosService)
        {
            _loggingService = loggingService;
            _jurosService = jurosService;
        }

        /// <summary>
        /// Calcula o juros para o valor inicial e tempo informados.
        /// </summary>
        /// <param name="valorInicial">Valor inicial a ser aplicado os juros.</param>
        /// <param name="meses">Quantidade de meses a aplicar os juros.</param>
        /// <returns>O valor total após aplicação dos juros.</returns>
        [HttpPost("calculajuros")]
        [ProducesResponseType(typeof(Response<Investimento>), 200)]
        [ProducesResponseType(typeof(ResponseStatus), 500)]
        public async Task<ActionResult<Response<Juros>>> CalculaJuros([FromQuery] decimal valorInicial, [FromQuery] int meses)
        {
            try
            {
                if (valorInicial == default && meses == default)
                {
                    var erros = new List<Error>
                    {
                        new Error { Mensagem = $"Valor inicial não pode ser zero. { new { valorInicial } }" },
                        new Error { Mensagem = $"Quantidade de meses não pode ser zero. { new { meses } }" }

                    };

                    _loggingService.LogInfo($"Parâmetros inválidos { new { valorInicial, meses } }.");
                    return BadRequest(ResponseHelper.CreateResponse("Parâmetros inválidos.", HttpStatusCode.BadRequest, erros));
                }

                _loggingService.LogInfo($"Calculando juros para { new { valorInicial, meses } }");

                var conteudo = new Investimento
                {
                    ValorInicial = valorInicial,
                    ValorFinal = await _jurosService.CalculaJurosAsync(valorInicial, meses)
                };

                var response = ResponseHelper.CreateResponse("Cálculo de Juros realizado com sucesso.", HttpStatusCode.OK, new List<Error>(), conteudo);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Exceção ocorreu. {ex.Message}", ex);

                var response = ResponseHelper.CreateResponse("Algo inesperado ocorreu. Por favor cheque a lista de erros.", HttpStatusCode.InternalServerError, new List<Error> { new Error { Mensagem = ex.Message } });

                return StatusCode(500, response);
            }
        }
    }
}