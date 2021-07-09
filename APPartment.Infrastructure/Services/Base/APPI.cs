﻿using APPartment.Common;
using APPartment.ORM.Framework.Declarations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace APPartment.Infrastructure.Services.Base
{
    public class APPI
    {
        private long? currentUserID;
        private long? currentHomeID;
        private string currentControllerName;
        private string currentAreaName;

        public APPI(long? currentUserID, long? currentHomeID, string currentControllerName, string currentAreaName)
        {
            this.currentUserID = currentUserID;
            this.currentHomeID = currentHomeID;
            this.currentControllerName = currentControllerName;
            this.currentAreaName = currentAreaName;
        }

        public async Task<T> RequestEntity<T>(long ID)
            where T : IBaseObject, new()
        {
            var model = new T();

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/{currentAreaName}/{currentControllerName}/{ID}";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", currentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", currentHomeID.ToString());

                using (var response = await httpClient.GetAsync(requestUri))
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        model = JsonConvert.DeserializeObject<T>(content);
                }
            }

            return model;
        }

        public async Task<T> RequestEntity<T>(long ID, string areaName, string controllerName)
            where T : IBaseObject, new()
        {
            var model = new T();

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/{areaName}/{controllerName}/{ID}";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", currentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", currentHomeID.ToString());

                using (var response = await httpClient.GetAsync(requestUri))
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        model = JsonConvert.DeserializeObject<T>(content);
                }
            }

            return model;
        }

        public async Task<List<T>> RequestEntities<T>()
            where T : IBaseObject, new()
        {
            var models = new List<T>();

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/{currentAreaName}/{currentControllerName}";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", currentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", currentHomeID.ToString());

                using (var response = await httpClient.GetAsync(requestUri))
                {
                    var content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        models = JsonConvert.DeserializeObject<List<T>>(content);
                }
            }

            return models;
        }

        public async Task<List<T>> RequestEntities<T>(string areaName, string controllerName)
            where T : IBaseObject, new()
        {
            var models = new List<T>();

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/{areaName}/{controllerName}";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", currentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", currentHomeID.ToString());

                using (var response = await httpClient.GetAsync(requestUri))
                {
                    var content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        models = JsonConvert.DeserializeObject<List<T>>(content);
                }
            }

            return models;
        }

        public async Task<T> RequestLookupEntity<T>(long ID)
            where T : ILookupObject, new()
        {
            var model = new T();

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/{currentAreaName}/{currentControllerName}/{ID}";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", currentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", currentHomeID.ToString());

                using (var response = await httpClient.GetAsync(requestUri))
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        model = JsonConvert.DeserializeObject<T>(content);
                }
            }

            return model;
        }

        public async Task<T> RequestLookupEntity<T>(long ID, string areaName, string controllerName)
            where T : ILookupObject, new()
        {
            var model = new T();

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/{areaName}/{controllerName}/{ID}";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", currentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", currentHomeID.ToString());

                using (var response = await httpClient.GetAsync(requestUri))
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        model = JsonConvert.DeserializeObject<T>(content);
                }
            }

            return model;
        }

        public async Task<List<T>> RequestLookupEntities<T>()
            where T : ILookupObject, new()
        {
            var models = new List<T>();

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/{currentAreaName}/{currentControllerName}";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", currentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", currentHomeID.ToString());

                using (var response = await httpClient.GetAsync(requestUri))
                {
                    var content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        models = JsonConvert.DeserializeObject<List<T>>(content);
                }
            }

            return models;
        }

        public async Task<List<T>> RequestLookupEntities<T>(string areaName, string controllerName)
            where T : ILookupObject, new()
        {
            var models = new List<T>();

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/{areaName}/{controllerName}";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", currentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", currentHomeID.ToString());

                using (var response = await httpClient.GetAsync(requestUri))
                {
                    var content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        models = JsonConvert.DeserializeObject<List<T>>(content);
                }
            }

            return models;
        }

        public async Task<bool> RequestPostEntity<T>(T model)
            where T : IBaseObject, new()
        {
            var responseIsSuccess = false;

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/{currentAreaName}/{currentControllerName}";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", currentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", currentHomeID.ToString());

                using (var response = await httpClient.PostAsJsonAsync(requestUri, model))
                {
                    var content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        responseIsSuccess = true;
                }
            }

            return responseIsSuccess;
        }

        public async Task<T> RequestPostEntity<T>(T model, string areaName, string controllerName)
            where T : IBaseObject, new()
        {
            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/{areaName}/{controllerName}";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", currentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", currentHomeID.ToString());

                using (var response = await httpClient.PostAsJsonAsync(requestUri, model))
                {
                    var content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        return JsonConvert.DeserializeObject<T>(content);
                }
            }

            return new T();
        }

        public async Task<bool> RequestDeleteEntity(long ID)
        {
            var responseIsSuccess = false;

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/{currentAreaName}/{currentControllerName}/{ID}";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", currentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", currentHomeID.ToString());

                using (var response = await httpClient.DeleteAsync(requestUri))
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        responseIsSuccess = true;
                }
            }

            return responseIsSuccess;
        }

        public async Task<bool> RequestDeleteEntity(long ID, string areaName, string controllerName)
        {
            var responseIsSuccess = false;

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/{areaName}/{controllerName}/{ID}";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", currentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", currentHomeID.ToString());

                using (var response = await httpClient.DeleteAsync(requestUri))
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        responseIsSuccess = true;
                }
            }

            return responseIsSuccess;
        }

        public async Task<int> RequestEntitiesCount()
        {
            var count = 0;

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/{currentAreaName}/{currentControllerName}/count";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", currentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", currentHomeID.ToString());

                using (var response = await httpClient.GetAsync(requestUri))
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        count = JsonConvert.DeserializeObject<int>(content);
                }
            }

            return count;
        }

        public async Task<int> RequestEntitiesCount(string areaName, string controllerName)
        {
            var count = 0;

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/{areaName}/{controllerName}/count";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", currentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", currentHomeID.ToString());

                using (var response = await httpClient.GetAsync(requestUri))
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        count = JsonConvert.DeserializeObject<int>(content);
                }
            }

            return count;
        }
    }
}