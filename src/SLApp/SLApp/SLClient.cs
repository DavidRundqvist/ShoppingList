using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
namespace SLApp
{
    public class SLClient
    {
        private readonly HttpClient _httpClient;
        private readonly Uri _uri;

        public SLClient(Uri uri)
        {
            _uri = uri;
            _httpClient = new HttpClient();
        }

        public async Task AddItems(string[] itemsToBuy)
        {
            // Add items to buy
            foreach (var item in itemsToBuy)
            {
                var message = $"{_uri}/AddNew?itemName={item}";
                HttpResponseMessage response = null;
                try
                {
                    response = await _httpClient.PostAsync(message, new StringContent(""));
                    Console.WriteLine($"Added {item} to {_uri}, status code: {response.StatusCode}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error trying to add {item} to {_uri}: {ex.Message}");
                }
            }

        }

    }
}
