using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using API2.WebApi.Models;
using API2.WebApi.Models.Response;
using API2.WebApi.Services;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;

namespace API2.WebApi.UnitTests.Services
{
    public class JurosServiceUnitTests
    {
        private Mock<ILoggingService<JurosService>> _loggingServiceMock;
        private Mock<IWebClientService> _webClientServiceMock;
        private Mock<IConfiguration> _configurationMock;
        private IJurosService _jurosService;

        [SetUp]
        public void Setup()
        {
            _loggingServiceMock = new Mock<ILoggingService<JurosService>>();
            _webClientServiceMock = new Mock<IWebClientService>();
            _configurationMock = new Mock<IConfiguration>();
            _jurosService = new JurosService(_loggingServiceMock.Object, _configurationMock.Object, _webClientServiceMock.Object);
        }

        [Test]
        public async Task CalculaJurosAsync_Deve_CalcularCorretamente()
        {
            //Arrange
            var taxaJuros = 0.01M;
            var juros = new Juros { Taxa = taxaJuros };
            var response = new Response<Juros>
            {
                Status = new Status
                {
                    Mensagem = "Taxa de Juros recuperada com sucesso.",
                    StatusCode = HttpStatusCode.OK,
                    Erros = new List<Error>()
                },
                Conteudo = juros
            };

            _configurationMock.Setup(_ => _.GetSection(It.IsAny<string>())).Returns(new Mock<IConfigurationSection>().Object);
            _webClientServiceMock.Setup(_ => _.GetRequest<Response<Juros>>(It.IsAny<string>())).ReturnsAsync(response);


            //Act
            var valorFinal = await _jurosService.CalculaJurosAsync(50, 3);

            //Assert
            valorFinal.Should().Be(51.51M);
            _webClientServiceMock.Verify(_ => _.GetRequest<Response<Juros>>(It.IsAny<string>()), Times.Once);
        }
    }
}
