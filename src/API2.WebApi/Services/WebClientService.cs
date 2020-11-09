using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace API2.WebApi.Services
{
    /// <summary>
    /// Implementação para ><see cref="IWebClientService"/>.
    /// </summary>
    public class WebClientService : IWebClientService
    {
        /// <see cref="IWebClientService.GetRequest{TResponse}"/>
        public async Task<TResponse> GetRequest<TResponse>(string uri)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    using (HttpResponseMessage response = await client.GetAsync(uri))
                    {
                        response.EnsureSuccessStatusCode();
                        var bodyContent = await response.Content.ReadAsStringAsync();

                        return JsonConvert.DeserializeObject<TResponse>(bodyContent);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
	}
}
