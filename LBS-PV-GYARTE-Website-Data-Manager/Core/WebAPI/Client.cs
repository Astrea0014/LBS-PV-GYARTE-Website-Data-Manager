using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using DataManager.Core.Exceptions;
using DataManager.Core.Exceptions.Json;
using DataManager.Core.Json;

namespace DataManager.Core.WebAPI
{
    //------- NOTE -------
    // This file is very much incomplete.
    // As all routes have not been implemented on the server, client side implementations of
    // all functionality according to specification is also missing as a result.
    //--------------------


    /// <summary>
    /// A class managing all communication with the content server.
    /// </summary>
    class Client : IDisposable
    {
        private readonly HttpClient _client;
        private CancellationTokenSource _cts;

        // Authentication state has to be tracked manually as the server requests no client access for authentication cookies.
        /// <summary>
        /// Indicates whether the current client instance is authenticated against the server.
        /// </summary>
        /// <remarks>
        /// Authentication state is tracked manually as the server specifies "secure" for the cookie, meaning the client cannot access it.
        /// The state is updated whenever <see cref="BeginSessionAsync(string, CancellationToken?)"/> or <see cref="EndSessionAsync"/> is called,
        /// or when the client receives a response with code <see cref="System.Net.HttpStatusCode.Unauthorized"/>.
        /// </remarks>
        public bool IsAuthenticated { get; private set; }

        public Client()
        {
            _client = new HttpClient(new HttpClientHandler()
            {
                UseCookies = true // Indicate that the client needs cookies for authentication.
            })
            {
                BaseAddress = new Uri("http://localhost:3000/")
            };

            // Allows asynchronous tasks to be universally cancelled if needed.
            _cts = new CancellationTokenSource();

            IsAuthenticated = false;
        }

        // Helper function to read the error from the response body and throw the right exception.
        private async Task HandleIsUnsuccessfulStatusCodeAsync(Route route, HttpMethod method, HttpResponseMessage response, CancellationToken? cancellationToken)
        {
            if (!response.IsSuccessStatusCode)
            {
                Exception? toThrow = null;

                try // Try to construct and throw an exception that the program can handle based on the HTTP-method and response code.
                {
                    // Read the error message from the response body.
                    FailureResponsePayload payload = JsonSerializer.Deserialize<FailureResponsePayload>(await response.Content.ReadAsStreamAsync(cancellationToken ?? _cts.Token));

                    // Get the translation table for HTTP status codes and client exceptions. Throw if it does not exist.
                    // A generic throw without setting 'toThrow' results in the default case exception being thrown.
                    var perStatusCodeExceptions = route.PerResponseExceptions[method] ?? throw new Exception();

                    // Get the constructor function for the translated exception.
                    var fnEx = perStatusCodeExceptions[response.StatusCode];

                    // Construct the translated exception and assign it to be thrown later.
                    toThrow = fnEx(payload);
                }
                // Make sure the application does not crash if any data for structured exception translation is missing.
                catch { }

                if (toThrow is AuthenticationException)
                    IsAuthenticated = false;

                // Throw the exception, or a generic if no exception was successfully constructed.
                throw toThrow ?? new HttpRequestException("Unknown HTTP exception occured.", null, response.StatusCode);
            }
        }

        /// <summary>
        /// Cancels all ongoing async operations that use the default cancellation token.
        /// </summary>
        public async Task CancelAllOperationsAsync()
        {
            await _cts.CancelAsync();
        }

        /// <summary>
        /// Resets the cancellation token if a cancellation has been previously requested.
        /// </summary>
        public void ResetCancellationToken()
        {
            if (!_cts.IsCancellationRequested)
                return;
            _cts.Dispose();
            _cts = new CancellationTokenSource();
        }

        /// <summary>
        /// Authenticates the client against the server to allow for subsequent API calls.
        /// </summary>
        /// <param name="master">The master key required by the server to authenticate as administrator.</param>
        /// <param name="cancellationToken">A cancellation token allowing for early termination of the task.</param>
        /// <returns>A task that will complete when the client authentication is complete.</returns>
        /// <exception cref="AlreadyAuthenticatedException">Thrown if the client already is authenticated against the server.</exception>
        public async Task BeginSessionAsync(string master, CancellationToken? cancellationToken = null)
        {
            // Post to login endpoint.

            if (IsAuthenticated)
                throw new AlreadyAuthenticatedException();

            using JsonContent body = new SessionBeginRequestPayload { Master = master }.SerializeContent();
            using HttpResponseMessage response = await _client.PostAsync(Routes.SessionBegin.RouteString, body, cancellationToken ?? _cts.Token);
            await HandleIsUnsuccessfulStatusCodeAsync(Routes.SessionBegin, HttpMethod.Post, response, cancellationToken);

            // Set authentication state.
            IsAuthenticated = true;
        }

        /// <summary>
        /// Unauthenticates the current client against the server.
        /// </summary>
        /// <returns>A task that will complete when the client unauthentication is complete.</returns>
        /// <exception cref="NotImplementedException">This function is yet to be implemented.</exception>
        public Task EndSessionAsync()
        {
            // Post to logout endpoint.
            // This endpoint is not yet implemented on the server.

            throw new NotImplementedException();
        }

        /// <summary>
        /// Fetches a list containing the usernames of all registered entities on the server.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token allowing for early termination of the task.</param>
        /// <returns>A task that will contain an array of strings containing the fetched usernames when completed.</returns>
        /// <exception cref="AuthenticationException">Thrown when the client is not authenticated against the server.</exception>
        /// <exception cref="HttpIOException">Thrown when the response message contains invalid content.</exception>
        public async Task<string[]> GetEntityNamesAsync(CancellationToken? cancellationToken = null)
        {
            if (!IsAuthenticated)
                throw new AuthenticationException("The client is not authenticated against the server yet.");

            using HttpResponseMessage response = await _client.GetAsync(Routes.Entity.RouteString, cancellationToken ?? _cts.Token);
            await HandleIsUnsuccessfulStatusCodeAsync(Routes.Entity, HttpMethod.Get, response, cancellationToken);

            Stream contentStream = await response.Content.ReadAsStreamAsync(cancellationToken ?? _cts.Token);
            string[]? ret = await JsonSerializer.DeserializeAsync<string[]>(contentStream, cancellationToken: cancellationToken ?? _cts.Token);

            return ret ?? throw new HttpIOException(HttpRequestError.InvalidResponse);
        }

        /// <summary>
        /// Fetches all information about the named entity.
        /// </summary>
        /// <param name="entityName">The username of the entity to fetch information about.</param>
        /// <param name="cancellationToken">A cancellation token allowing for early termination of the task.</param>
        /// <returns>A task containing an entity response payload which contains information about the entity when completed.</returns>
        /// <exception cref="AuthenticationException">Thrown when the client is not authenticated against the server.</exception>
        /// <exception cref="HttpIOException">Thrown when the response message contains invalid content.</exception>
        public async Task<EntityResponsePayload> GetEntityAsync(string entityName, CancellationToken? cancellationToken = null)
        {
            if (!IsAuthenticated)
                throw new AuthenticationException("The client is not authenticated against the server yet.");

            using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Routes.Entity.RouteString);
            request.Headers.Add("Entity-Username", entityName);

            using HttpResponseMessage response = await _client.SendAsync(request, cancellationToken ?? _cts.Token);
            await HandleIsUnsuccessfulStatusCodeAsync(Routes.Entity, HttpMethod.Get, response, cancellationToken);

            Stream contentStream = await response.Content.ReadAsStreamAsync(cancellationToken ?? _cts.Token);
            EntityResponsePayload? ret = await JsonSerializer.DeserializeAsync<EntityResponsePayload>(contentStream, cancellationToken: cancellationToken ?? _cts.Token);

            return ret ?? throw new HttpIOException(HttpRequestError.InvalidResponse);
        }

        /// <summary>
        /// Registers a new entity with the following credentials and accesses.
        /// </summary>
        /// <param name="username">A username for the entity to register.</param>
        /// <param name="password">A chosen password for the entity. If set to <see langword="null"/> the server will auto-generate a password.</param>
        /// <param name="accessNames">The names of the access levels to grant the newly registered entity.</param>
        /// <param name="cancellationToken">A cancellation token allowing for early termination of the task.</param>
        /// <returns>A task containing an entity response payload which contains information about the entity when completed.</returns>
        /// <exception cref="AuthenticationException">Thrown when the client is not authenticated against the server.</exception>
        /// <exception cref="HttpIOException">Thrown when the response message contains invalid content.</exception>
        public async Task<EntityResponsePayload> PostEntityAsync(string username, string? password, string[] accessNames, CancellationToken? cancellationToken = null)
        {
            if (!IsAuthenticated)
                throw new AuthenticationException("The client is not authenticated against the server yet.");

            using JsonContent body = new EntityPostRequestPayload { Username = username, Password = password, AccessNames = accessNames }.SerializeContent();
            using HttpResponseMessage response = await _client.PostAsync(Routes.Entity.RouteString, body, cancellationToken ?? _cts.Token);
            await HandleIsUnsuccessfulStatusCodeAsync(Routes.Entity, HttpMethod.Post, response, cancellationToken);

            Stream contentStream = await response.Content.ReadAsStreamAsync(cancellationToken ?? _cts.Token);
            EntityResponsePayload? ret = await JsonSerializer.DeserializeAsync<EntityResponsePayload>(contentStream, cancellationToken: cancellationToken ?? _cts.Token);

            return ret ?? throw new HttpIOException(HttpRequestError.InvalidResponse);
        }

        /// <summary>
        /// Edits an existing entity's information such as password or access control list.
        /// </summary>
        /// <param name="args">The patch arguments containing what information about the entity to edit.</param>
        /// <param name="cancellationToken">A cancellation token allowing for early termination of the task.</param>
        /// <returns>A task containing an entity response payload which contains information about the entity when completed.</returns>
        /// <exception cref="AuthenticationException">Thrown when the client is not authenticated against the server.</exception>
        /// <exception cref="HttpIOException">Thrown when the response message contains invalid content.</exception>
        public async Task<EntityResponsePayload> PatchEntityAsync(EntityPatchArgs args, CancellationToken? cancellationToken = null)
        {
            if (!IsAuthenticated)
                throw new AuthenticationException("The client is not authenticated against the server yet.");

            using JsonContent body = EntityPatchRequestPayload.FromArgs(args).SerializeContent();
            using HttpResponseMessage response = await _client.PatchAsync(Routes.Entity.RouteString, body, cancellationToken ?? _cts.Token);
            await HandleIsUnsuccessfulStatusCodeAsync(Routes.Entity, HttpMethod.Patch, response, cancellationToken);

            Stream contentStream = await response.Content.ReadAsStreamAsync(cancellationToken ?? _cts.Token);
            EntityResponsePayload? ret = await JsonSerializer.DeserializeAsync<EntityResponsePayload>(contentStream, cancellationToken: cancellationToken ?? _cts.Token);

            return ret ?? throw new HttpIOException(HttpRequestError.InvalidResponse);
        }

        public void Dispose()
        {
            _client.Dispose();
            _cts.Dispose();
            GC.SuppressFinalize(this);
        }

        ~Client() => Dispose();
    }
}
