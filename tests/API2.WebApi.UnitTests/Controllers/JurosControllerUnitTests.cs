using System.Threading.Tasks;
using API2.WebApi.Controllers;
using API2.WebApi.Models;
using API2.WebApi.Models.Response;
using API2.WebApi.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace API2.WebApi.UnitTests.Controllers
{
    public class JurosControllerUnitTests
    {
        private Mock<ILoggingService<JurosController>> _loggingServiceMock;
        private Mock<IJurosService> _jurosService;
        private JurosController _jurosController;

        [SetUp]
        public void Setup()
        {
            _loggingServiceMock = new Mock<ILoggingService<JurosController>>();
            _jurosService = new Mock<IJurosService>();
            _jurosController = new JurosController(_loggingServiceMock.Object, _jurosService.Object);
        }

        [Test]
        public async Task CalculaJuros_Deve_RetornarValorFinal_Successfuly()
        {
            //Arrange
            var valorInicial = 50M;
            var meses = 3;
            _jurosService.Setup(_ => _.CalculaJurosAsync(It.IsAny<decimal>(), It.IsAny<int>())).ReturnsAsync(51.51M);

            //Act
            var response = ((OkObjectResult) (await _jurosController.CalculaJuros(valorInicial, meses)).Result).Value as Response<Investimento>;

            //Assert
            response?.Conteudo.ValorFinal.Should().Be(51.51M);
            Assert.IsInstanceOf<Investimento>(response?.Conteudo);
            _loggingServiceMock.Verify(_ =>
                _.LogInfo(It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<int>()), Times.Once);
        }
    }
}
