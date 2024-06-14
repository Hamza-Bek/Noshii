using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions
{
    public class HttpClientService(IHttpClientFactory httpClientFactory, LocalStorageService localStorageService)
    {
        private HttpClient CreateClient() => httpClientFactory!.CreateClient(Constants.HttpClientName);
        public HttpClient GetPublicClient() => CreateClient();

        public async Task<HttpClient> GetPrivateClient()
        {
            try
            {
                var client = CreateClient();
                var localStorageDTO = await localStorageService.GetModelFromToken();
                if (string.IsNullOrEmpty(localStorageDTO.Token))
                    return client;

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.HttpClientHeaderScheme, localStorageDTO.Token);
                return client;
            }
            catch
            {
                return new HttpClient();
            }
        }
    }
}
