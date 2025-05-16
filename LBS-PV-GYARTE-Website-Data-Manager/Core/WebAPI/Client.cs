using System.Net.Http;

namespace DataManager.Core.WebAPI
{
    class Client : IDisposable
    {
        private readonly HttpClient _client;

        public bool IsAuthenticated { get; private set; }

        public Client()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:3000/")
            };

            IsAuthenticated = false;
        }

        public async void ExecuteActionAsync(Route route, Action action)
        {

        }

        public async void BeginSessionAsync(string master)
        {
            
        }

        public async void EndSessionAsync()
        {

        }

        public void Dispose()
        {
            _client.Dispose();
            GC.SuppressFinalize(this);
        }
        ~Client() => Dispose();
    }
}
