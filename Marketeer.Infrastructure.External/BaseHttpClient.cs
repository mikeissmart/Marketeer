using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;

namespace Marketeer.Infrastructure.External
{
    public interface IHttpClient
    {

    }

    public abstract class BaseHttpClient : IHttpClient
    {
        protected readonly HttpClient _client;
        protected readonly ILogger _logger;
        protected readonly string _baseUri;

        protected BaseHttpClient(HttpClient client, ILogger logger, string uri)
        {
            _client = client;
            _logger = logger;
            _baseUri = uri;

            _client.BaseAddress = new Uri(uri);
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36");
        }

        protected async Task<T> GetAsync<T>(string action)
        {
            try
            {
                var response = await _client.GetAsync(action);
                response.EnsureSuccessStatusCode();
                var a = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync())!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GET: Action: {action}: Message: {ex.Message}");
                throw;
            }
        }

        protected async Task<TOut> PostAsync<TOut, TIn>(string action, TIn payload)
        {
            try
            {
                var response = await _client.PostAsync(action, JsonPayload(payload));
                response.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<TOut>(await response.Content.ReadAsStringAsync())!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"POST: Action: {action}: Payload: {JsonPayload(payload)} Message: {ex.Message}");
                throw;
            }
        }

        protected async Task<TOut> PutAsync<TOut, TIn>(string action, TIn payload)
        {
            try
            {
                var response = await _client.PutAsync(action, JsonPayload(payload));
                response.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<TOut>(await response.Content.ReadAsStringAsync())!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Put: Action: {action}: Payload: {JsonPayload(payload)} Message: {ex.Message}");
                throw;
            }
        }

        protected async Task<T> DeleteAsync<T>(string action)
        {
            try
            {
                var response = await _client.DeleteAsync(action);
                response.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync())!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GET: Action: {action}: Message: {ex.Message}");
                throw;
            }
        }

        private static HttpContent JsonPayload<T>(T payload)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateFormatHandling = DateFormatHandling.IsoDateFormat
            };
            var json = JsonConvert.SerializeObject(payload, settings);

            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}
