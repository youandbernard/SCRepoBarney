using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Amazon.Extensions.NETCore.Setup;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using CaseMix.Aws.OpenSearch.Model;
using System.Net.Http.Headers;
using CaseMix.Core;

namespace CaseMix.Aws.OpenSearch.Services
{
    public class OpenSearchService : IOpenSearchService
    {
        private HttpClient _httpClient;
        private readonly AWSOptions _options;

        public OpenSearchService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();

            var options = configuration.GetAWSOptions();
            _options = options;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<OpenSearchResponseDto> UploadAsync(string endpoint, string masterUser, string masterPassword, string data)
        {
            OpenSearchResponseDto result = new OpenSearchResponseDto();
            result.Success = false;

            try
            {
                var content = new StringContent(data, Encoding.UTF8, "application/json");

                _httpClient = SetHttpClient(masterUser, masterPassword);

                var response = await _httpClient.PostAsync(endpoint, content);
                if (response.IsSuccessStatusCode)
                {
                    result.Success = true;
                    result.SuccessMessage = "Successfully uploaded data to OpenSearch.";
                }
                else
                {
                    result.Success = false;
                    result.Errors = new List<string>();

                    var message = await response.Content.ReadAsStringAsync();
                    result.Errors.Add(JsonConvert.DeserializeObject(message).ToString());
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Errors = new List<string>();
                result.Errors.Add(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private HttpClient SetHttpClient(string username, string password)
        {
            HttpClient httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes($"{username}:{password}")));

            return httpClient;
        }
    }
}
