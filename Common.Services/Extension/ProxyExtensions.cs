using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;

namespace Common.Services.Extension
{
    public static class ProxyExtensions
    {
        public static IServiceCollection AddProxy<TClient, TImplementation>( this IServiceCollection services , IConfiguration configuration )
            where TClient : class
            where TImplementation : class, TClient
        {
            // Get the base URL from appsettings.json
            var configBaseAddress = configuration[$"{typeof ( TImplementation ).Name}:BaseUrl"];
            var baseUrl = configBaseAddress ?? "https://localhost:7150/";

            // Add Proxy service with the client base address from configuration
            services.AddHttpClient<TClient , TImplementation> (
                client =>
                {
                    client.BaseAddress = new Uri ( baseUrl ); // Base address of your api
                } );

            return services;
        }

        public static async Task<TResponseClass?> GetAsync<TResponseClass>( this HttpClient client , string relativeUrl )
        {
            var response = await client.GetFromJsonAsync<TResponseClass?> ( relativeUrl );
            return response;
        }

        public static async Task PostAsync<TBodyClass>( this HttpClient client , string relativeUrl , TBodyClass body )
        {
            using var response = await client.PostAsJsonAsync ( relativeUrl , body );
            response.EnsureSuccessStatusCode ();
        }

        public static async Task PutAsync<TBodyClass>( this HttpClient client , string relativeUrl , TBodyClass body )
        {
            using var response = await client.PutAsJsonAsync ( relativeUrl , body );
            response.EnsureSuccessStatusCode ();
        }

        public static async Task PatchAsync<TBodyClass>( this HttpClient client , string relativeUrl , TBodyClass body )
        {
            using var response = await client.PatchAsJsonAsync ( relativeUrl , body );
            response.EnsureSuccessStatusCode ();
        }

        public static async Task DeleteAsync<TBodyClass>( this HttpClient client , string relativeUrl )
        {
            using var response = await client.DeleteAsync ( relativeUrl );
            response.EnsureSuccessStatusCode ();
        }
    }
}
