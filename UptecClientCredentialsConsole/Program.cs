using Microsoft.Identity.Client;
using System.Net.Http.Headers;

namespace UptecClientCredentialsConsole
{
    public class Program
    {
        #region Private Members
        private static readonly string _authority = "https://login.microsoftonline.com/61936779-8f05-434a-ac81-99da453d652c";
        private static readonly string _clientId = "CLIENT_ID";
        private static readonly string _clientSecret = "CLIENT_SECRET";
        private static readonly string[] _scopes =
        {
            "api://3a9b9211-6791-4992-b779-bb05935f708b/.default"
        };

        //web api base address
        private static readonly string _apiBaseAddress = "https://localhost:8181";
        private static string _accessToken = "";
        #endregion
        static async Task InitiateApp()
        {
            IConfidentialClientApplication app =
                ConfidentialClientApplicationBuilder.Create(_clientId)
                .WithClientSecret(_clientSecret)
                .WithAuthority(_authority)
                .Build();
            try
            {
                var result = await app.AcquireTokenForClient(_scopes).ExecuteAsync();
                if (result != null)
                {
                    _accessToken = result.AccessToken;
                    Console.WriteLine(_accessToken);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        static HttpClient GetHttpClient()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(_apiBaseAddress)
            };
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _accessToken);
            return client;

        }
        static async Task InvokeApiEndpoint()
        {
            var client = GetHttpClient();
            string path = "/WeatherForecast";
            Console.WriteLine($"\nCalling {path}....");
            string response = await client.GetStringAsync(path);
            Console.WriteLine("Response:");
            Console.WriteLine(response);
        }
        public static void Main(string[] args)
        {
            InitiateApp().Wait();
            InvokeApiEndpoint().Wait();
        }
    }
}