using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Serilog;

namespace API2.WebApi.IntegrationTests
{
    public class JurosIntegrationTests
    {
        private readonly HttpClient _httpClient;

        public JurosIntegrationTests()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .UseSerilog((hostingContext, loggerConfig) =>
                    loggerConfig.ReadFrom.Configuration(hostingContext.Configuration)
                )
                .UseConfiguration(config)
                .UseStartup<Startup>());

            _httpClient = server.CreateClient();
        }

        [Test]
        public async Task CalcularJuros_DeveSer_Successful()
        {
            //Arrange
            var parametros = new Dictionary<string, string>
            {
                ["valorInicial"] = "50",
                ["meses"] = "3",
            };

            //Act
            var response = await _httpClient.PostAsync(QueryHelpers.AddQueryString("/api/juros/calculajuros/", parametros), null);

            //Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}