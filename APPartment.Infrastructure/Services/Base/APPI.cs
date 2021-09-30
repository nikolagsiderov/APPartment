using APPartment.Common;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace APPartment.Infrastructure.Services.Base
{
    public class APPI
    {
        public APPI()
        {
        }

        public async Task<T> RequestAsync<T>(string[] endpointParams, string currentUserID, string currentHomeID)
        {
            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/" + string.Join('/', endpointParams);
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", currentUserID);
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", currentHomeID);

                using (var response = await httpClient.GetAsync(requestUri))
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        return JsonConvert.DeserializeObject<T>(content);
                }
            }

            return default(T);
        }

        public async Task<List<T>> RequestManyAsync<T>(string[] endpointParams, string currentUserID, string currentHomeID)
        {
            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/" + string.Join('/', endpointParams);
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", currentUserID);
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", currentHomeID);

                using (var response = await httpClient.GetAsync(requestUri))
                {
                    var content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        return JsonConvert.DeserializeObject<List<T>>(content);
                }
            }

            return new List<T>();
        }

        public async Task<bool> PostAsync<T>(T model, string[] endpointParams, string currentUserID, string currentHomeID)
        {
            var responseIsSuccess = false;

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/" + string.Join('/', endpointParams);
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", currentUserID);
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", currentHomeID);

                using (var response = await httpClient.PostAsJsonAsync(requestUri, model))
                {
                    var content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        responseIsSuccess = true;
                }
            }

            return responseIsSuccess;
        }

        public async Task<T> PostReturnAsync<T>(T model, string[] endpointParams, string currentUserID, string currentHomeID)
        {
            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/" + string.Join('/', endpointParams);
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", currentUserID);
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", currentHomeID);

                using (var response = await httpClient.PostAsJsonAsync(requestUri, model))
                {
                    var content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        return JsonConvert.DeserializeObject<T>(content);
                }
            }

            return default(T);
        }

        public async Task<bool> DeleteAsync(string[] endpointParams, string currentUserID, string currentHomeID)
        {
            var responseIsSuccess = false;

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/" + string.Join('/', endpointParams);
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", currentUserID);
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", currentHomeID);

                using (var response = await httpClient.DeleteAsync(requestUri))
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        responseIsSuccess = true;
                }
            }

            return responseIsSuccess;
        }
    }
}