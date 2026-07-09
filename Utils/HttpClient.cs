
using System.Text;
using System.Text.Json;

namespace dotnet_starter.Utils
{
    public interface IHttpClientHelper
    {
        Task<(bool Success, string Response, string? Error)> CallApi(string queryUrl, object request);
        Task<(bool Success, string Response, string? Error)> CallApiAuth(string queryUrl, string token, object request);
    }

    public class HttpClientHelper : IHttpClientHelper
    {
        private readonly HttpClient _httpClient;

        public HttpClientHelper(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<(bool Success, string Response, string? Error)> CallApi(string queryUrl, object request)
        {
            try
            {
                string jsonRequest = JsonSerializer.Serialize(request);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(queryUrl, content);
                string responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return (true, responseBody, null);
                }


                if (string.IsNullOrWhiteSpace(responseBody) || !response.Content.Headers.ContentType?.MediaType?.Contains("application/json") == true)
                {
                    return (false, "", $"HTTP {response.StatusCode}: {response.ReasonPhrase}");
                }

                var errorMessage = ExtractErrorMessage(responseBody);
                return (false, "", errorMessage);
            }
            catch (Exception ex)
            {
                return (false, "", $"Error: {ex.Message}");
            }
        }



        public async Task<(bool Success, string Response, string? Error)> CallApiAuth(string queryUrl, string token, object request)
        {
            try
            {
                string jsonRequest = JsonSerializer.Serialize(request);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, queryUrl)
                {
                    Content = content
                };
                requestMessage.Headers.Add("Authorization", $"Bearer {token}");

                HttpResponseMessage response = await _httpClient.SendAsync(requestMessage);
                string responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return (true, responseBody, null);
                }


                if (string.IsNullOrWhiteSpace(responseBody) ||
                    !response.Content.Headers.ContentType?.MediaType?.Contains("application/json") == true)
                {
                    return (false, "", $"HTTP {response.StatusCode}: {response.ReasonPhrase}");
                }

                var errorMessage = ExtractErrorMessage(responseBody);
                return (false, "", errorMessage);
            }
            catch (Exception ex)
            {
                return (false, "", $"Error: {ex.Message}");
            }
        }

        private string ExtractErrorMessage(string responseBody)
        {
            try
            {
                var errorDict = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);
                if (errorDict != null)
                {

                    if (errorDict.ContainsKey("message"))
                    {
                        return errorDict["message"]?.ToString() ?? "Unknown error";
                    }


                    if (errorDict.ContainsKey("error"))
                    {
                        return errorDict["error"]?.ToString() ?? "Unknown error";
                    }
                }

                return "API call failed.";
            }
            catch
            {
                return "Failed to parse error message.";
            }
        }

    }
}
