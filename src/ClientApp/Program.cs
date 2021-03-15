using IdentityModel.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ClientApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new HttpClient();
            var disco = await client
                .GetDiscoveryDocumentAsync("https://localhost:5001");
            if (disco.IsError) EventLogger(disco.Error, EventType.Error);

            var tokenResponse = await client
                .RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest
                {
                    Address = disco.TokenEndpoint,
                    ClientId = "client",
                    ClientSecret = "secret",
                    Scope = "api1"
                });

            if (tokenResponse.IsError) EventLogger(tokenResponse.Error, EventType.Error);

            EventLogger(tokenResponse.Json, EventType.Info);

            client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);
            var response = await client
                .GetAsync("https://localhost:5002/api/v1/product/secret");

            if (!response.IsSuccessStatusCode)
                EventLogger(response.StatusCode, EventType.Error);

            var content = await response.Content.ReadAsStringAsync();
            EventLogger(JArray.Parse(content), EventType.Info);

            Console.ReadKey();
        }

        private static void EventLogger(object source, EventType @event)
        {
            SetColor(@event);
            Console.WriteLine("===================================================");
            Console.WriteLine(source);
            Console.WriteLine("===================================================");
            Console.WriteLine(Environment.NewLine);
            Console.ResetColor();
            return;
        }

        private static void SetColor(EventType @event)
        {
            Console.ResetColor();
            if (@event == EventType.Error)
                Console.ForegroundColor = ConsoleColor.Red;
            else
                Console.ForegroundColor = ConsoleColor.DarkGreen;
        }

        private enum EventType
        {
            Info,
            Error
        }
    }
}
