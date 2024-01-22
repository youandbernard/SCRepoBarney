// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace SnomedApi
{
    using Microsoft.Rest;
    using Models;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// FindDescriptionsUsingGET operations.
    /// </summary>
    public partial class FindDescriptionsUsingGET : IServiceOperations<Snowstorm>, IFindDescriptionsUsingGET
    {
        /// <summary>
        /// Initializes a new instance of the FindDescriptionsUsingGET class.
        /// </summary>
        /// <param name='client'>
        /// Reference to the service client.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when a required parameter is null
        /// </exception>
        public FindDescriptionsUsingGET(Snowstorm client)
        {
            if (client == null)
            {
                throw new System.ArgumentNullException("client");
            }
            Client = client;
        }

        /// <summary>
        /// Gets a reference to the Snowstorm
        /// </summary>
        public Snowstorm Client { get; private set; }

        /// <summary>
        /// Search descriptions across multiple Code Systems.
        /// </summary>
        /// <param name='term'>
        /// term
        /// </param>
        /// <param name='acceptLanguage'>
        /// Accept-Language
        /// </param>
        /// <param name='active'>
        /// active
        /// </param>
        /// <param name='module'>
        /// module
        /// </param>
        /// <param name='language'>
        /// Set of two character language codes to match. The English language code
        /// 'en' will not be added automatically, in contrast to the Accept-Language
        /// header which always includes it. Accept-Language header still controls
        /// result FSN and PT language selection.
        /// </param>
        /// <param name='type'>
        /// Set of description types to include. Pick descendants of
        /// '900000000000446008 | Description type (core metadata concept) |'.
        /// </param>
        /// <param name='conceptActive'>
        /// conceptActive
        /// </param>
        /// <param name='contentScope'>
        /// contentScope. Possible values include: 'ALL_PUBLISHED_CONTENT'
        /// </param>
        /// <param name='offset'>
        /// offset
        /// </param>
        /// <param name='limit'>
        /// limit
        /// </param>
        /// <param name='customHeaders'>
        /// Headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="HttpOperationException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="SerializationException">
        /// Thrown when unable to deserialize the response
        /// </exception>
        /// <exception cref="ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when a required parameter is null
        /// </exception>
        /// <return>
        /// A response object containing the response body and response headers.
        /// </return>
        public async Task<HttpOperationResponse<ItemsPageBrowserDescriptionSearchResult>> OneWithHttpMessagesAsync(string term, string acceptLanguage, bool? active = default(bool?), string module = default(string), IList<string> language = default(IList<string>), IList<long?> type = default(IList<long?>), bool? conceptActive = default(bool?), string contentScope = default(string), int? offset = 0, int? limit = 50, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (term == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "term");
            }
            if (acceptLanguage == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "acceptLanguage");
            }
            // Tracing
            bool _shouldTrace = ServiceClientTracing.IsEnabled;
            string _invocationId = null;
            if (_shouldTrace)
            {
                _invocationId = ServiceClientTracing.NextInvocationId.ToString();
                Dictionary<string, object> tracingParameters = new Dictionary<string, object>();
                tracingParameters.Add("term", term);
                tracingParameters.Add("active", active);
                tracingParameters.Add("module", module);
                tracingParameters.Add("language", language);
                tracingParameters.Add("type", type);
                tracingParameters.Add("conceptActive", conceptActive);
                tracingParameters.Add("contentScope", contentScope);
                tracingParameters.Add("offset", offset);
                tracingParameters.Add("limit", limit);
                tracingParameters.Add("acceptLanguage", acceptLanguage);
                tracingParameters.Add("cancellationToken", cancellationToken);
                ServiceClientTracing.Enter(_invocationId, this, "One", tracingParameters);
            }
            // Construct URL
            var _baseUrl = Client.BaseUri.AbsoluteUri;
            var _url = new System.Uri(new System.Uri(_baseUrl + (_baseUrl.EndsWith("/") ? "" : "/")), "multisearch/descriptions").ToString();
            List<string> _queryParameters = new List<string>();
            if (term != null)
            {
                _queryParameters.Add(string.Format("term={0}", System.Uri.EscapeDataString(term)));
            }
            if (active != null)
            {
                _queryParameters.Add(string.Format("active={0}", System.Uri.EscapeDataString(Microsoft.Rest.Serialization.SafeJsonConvert.SerializeObject(active, Client.SerializationSettings).Trim('"'))));
            }
            if (module != null)
            {
                _queryParameters.Add(string.Format("module={0}", System.Uri.EscapeDataString(module)));
            }
            if (language != null)
            {
                if (language.Count == 0)
                {
                    _queryParameters.Add(string.Format("language={0}", System.Uri.EscapeDataString(string.Empty)));
                }
                else
                {
                    foreach (var _item in language)
                    {
                        _queryParameters.Add(string.Format("language={0}", System.Uri.EscapeDataString("" + _item)));
                    }
                }
            }
            if (type != null)
            {
                if (type.Count == 0)
                {
                    _queryParameters.Add(string.Format("type={0}", System.Uri.EscapeDataString(string.Empty)));
                }
                else
                {
                    foreach (var _item in type)
                    {
                        _queryParameters.Add(string.Format("type={0}", System.Uri.EscapeDataString("" + _item)));
                    }
                }
            }
            if (conceptActive != null)
            {
                _queryParameters.Add(string.Format("conceptActive={0}", System.Uri.EscapeDataString(Microsoft.Rest.Serialization.SafeJsonConvert.SerializeObject(conceptActive, Client.SerializationSettings).Trim('"'))));
            }
            if (contentScope != null)
            {
                _queryParameters.Add(string.Format("contentScope={0}", System.Uri.EscapeDataString(Microsoft.Rest.Serialization.SafeJsonConvert.SerializeObject(contentScope, Client.SerializationSettings).Trim('"'))));
            }
            if (offset != null)
            {
                _queryParameters.Add(string.Format("offset={0}", System.Uri.EscapeDataString(Microsoft.Rest.Serialization.SafeJsonConvert.SerializeObject(offset, Client.SerializationSettings).Trim('"'))));
            }
            if (limit != null)
            {
                _queryParameters.Add(string.Format("limit={0}", System.Uri.EscapeDataString(Microsoft.Rest.Serialization.SafeJsonConvert.SerializeObject(limit, Client.SerializationSettings).Trim('"'))));
            }
            if (_queryParameters.Count > 0)
            {
                _url += "?" + string.Join("&", _queryParameters);
            }
            // Create HTTP transport objects
            var _httpRequest = new HttpRequestMessage();
            HttpResponseMessage _httpResponse = null;
            _httpRequest.Method = new HttpMethod("GET");
            _httpRequest.RequestUri = new System.Uri(_url);
            // Set Headers
            if (acceptLanguage != null)
            {
                if (_httpRequest.Headers.Contains("Accept-Language"))
                {
                    _httpRequest.Headers.Remove("Accept-Language");
                }
                _httpRequest.Headers.TryAddWithoutValidation("Accept-Language", acceptLanguage);
            }


            if (customHeaders != null)
            {
                foreach(var _header in customHeaders)
                {
                    if (_httpRequest.Headers.Contains(_header.Key))
                    {
                        _httpRequest.Headers.Remove(_header.Key);
                    }
                    _httpRequest.Headers.TryAddWithoutValidation(_header.Key, _header.Value);
                }
            }

            // Serialize Request
            string _requestContent = null;
            // Send Request
            if (_shouldTrace)
            {
                ServiceClientTracing.SendRequest(_invocationId, _httpRequest);
            }
            cancellationToken.ThrowIfCancellationRequested();
            _httpResponse = await Client.HttpClient.SendAsync(_httpRequest, cancellationToken).ConfigureAwait(false);
            if (_shouldTrace)
            {
                ServiceClientTracing.ReceiveResponse(_invocationId, _httpResponse);
            }
            HttpStatusCode _statusCode = _httpResponse.StatusCode;
            cancellationToken.ThrowIfCancellationRequested();
            string _responseContent = null;
            if ((int)_statusCode != 200 && (int)_statusCode != 401 && (int)_statusCode != 403 && (int)_statusCode != 404)
            {
                var ex = new HttpOperationException(string.Format("Operation returned an invalid status code '{0}'", _statusCode));
                if (_httpResponse.Content != null) {
                    _responseContent = await _httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                }
                else {
                    _responseContent = string.Empty;
                }
                ex.Request = new HttpRequestMessageWrapper(_httpRequest, _requestContent);
                ex.Response = new HttpResponseMessageWrapper(_httpResponse, _responseContent);
                if (_shouldTrace)
                {
                    ServiceClientTracing.Error(_invocationId, ex);
                }
                _httpRequest.Dispose();
                if (_httpResponse != null)
                {
                    _httpResponse.Dispose();
                }
                throw ex;
            }
            // Create Result
            var _result = new HttpOperationResponse<ItemsPageBrowserDescriptionSearchResult>();
            _result.Request = _httpRequest;
            _result.Response = _httpResponse;
            // Deserialize Response
            if ((int)_statusCode == 200)
            {
                _responseContent = await _httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                try
                {
                    _result.Body = Microsoft.Rest.Serialization.SafeJsonConvert.DeserializeObject<ItemsPageBrowserDescriptionSearchResult>(_responseContent, Client.DeserializationSettings);
                }
                catch (JsonException ex)
                {
                    _httpRequest.Dispose();
                    if (_httpResponse != null)
                    {
                        _httpResponse.Dispose();
                    }
                    throw new SerializationException("Unable to deserialize the response.", _responseContent, ex);
                }
            }
            if (_shouldTrace)
            {
                ServiceClientTracing.Exit(_invocationId, _result);
            }
            return _result;
        }

    }
}
