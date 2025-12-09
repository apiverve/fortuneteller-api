using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
#if NET45 || NETSTANDARD2_0 || NETSTANDARD2_1 || NET6_0
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
#else
using System.Net;
#endif

namespace APIVerve.API.FortuneTeller
{
    /// <summary>
    /// Client for the APIVerve.API.FortuneTeller API
    /// </summary>
    public class FortuneTellerAPIClient
#if NET45 || NETSTANDARD2_0 || NETSTANDARD2_1 || NET6_0
        : IDisposable
#endif
    {
        private readonly string _apiEndpoint = "https://api.apiverve.com/v1/fortuneteller";
        private readonly string _method = "GET";

#if NET45 || NETSTANDARD2_0 || NETSTANDARD2_1 || NET6_0
        private readonly HttpClient _httpClient;
        private readonly bool _disposeHttpClient;
#endif

        private string _apiKey { get; set; }
        private bool _isSecure { get; set; }
        private bool _isDebug { get; set; }
        private int _maxRetries { get; set; }
        private int _retryDelayMs { get; set; }
#if NET45 || NETSTANDARD2_0 || NETSTANDARD2_1 || NET6_0
        private Action<string> _logger { get; set; }
#endif
        private Dictionary<string, string> _customHeaders { get; set; }

        /// <summary>
        /// Initialize the API client with your API key
        /// </summary>
        /// <param name="apiKey">Your API key from https://apiverve.com</param>
        /// <exception cref="ArgumentException">Thrown when API key is invalid</exception>
        public FortuneTellerAPIClient(string apiKey)
#if NET45 || NETSTANDARD2_0 || NETSTANDARD2_1 || NET6_0
            : this(apiKey, true, false, null)
#endif
        {
#if NET20 || NET35 || NET40
            ValidateApiKey(apiKey);
            _apiKey = apiKey;
            _isSecure = true;
            _isDebug = false;
            _maxRetries = 0;
            _retryDelayMs = 1000;
            _customHeaders = new Dictionary<string, string>();
#endif
        }

        /// <summary>
        /// Initialize the API client with your API key and security settings
        /// </summary>
        /// <param name="apiKey">Your API key from https://apiverve.com</param>
        /// <param name="isSecure">Use HTTPS (recommended)</param>
        /// <param name="isDebug">Enable debug logging</param>
        /// <exception cref="ArgumentException">Thrown when API key is invalid</exception>
        public FortuneTellerAPIClient(string apiKey, bool isSecure, bool isDebug)
#if NET45 || NETSTANDARD2_0 || NETSTANDARD2_1 || NET6_0
            : this(apiKey, isSecure, isDebug, null)
#endif
        {
#if NET20 || NET35 || NET40
            ValidateApiKey(apiKey);
            _apiKey = apiKey;
            _isSecure = isSecure;
            _isDebug = isDebug;
            _maxRetries = 0;
            _retryDelayMs = 1000;
            _customHeaders = new Dictionary<string, string>();
#endif
        }

#if NET45 || NETSTANDARD2_0 || NETSTANDARD2_1 || NET6_0
        /// <summary>
        /// Initialize the API client with your API key and a custom HttpClient
        /// </summary>
        /// <param name="apiKey">Your API key from https://apiverve.com</param>
        /// <param name="isSecure">Use HTTPS (recommended)</param>
        /// <param name="isDebug">Enable debug logging</param>
        /// <param name="httpClient">Custom HttpClient instance (optional). If null, a new instance will be created.</param>
        /// <exception cref="ArgumentException">Thrown when API key is invalid</exception>
        public FortuneTellerAPIClient(string apiKey, bool isSecure, bool isDebug, HttpClient httpClient)
        {
            // Validate API key format
            ValidateApiKey(apiKey);

            _apiKey = apiKey;
            _isSecure = isSecure;
            _isDebug = isDebug;
            _maxRetries = 0;
            _retryDelayMs = 1000;
            _customHeaders = new Dictionary<string, string>();

            if (httpClient != null)
            {
                _httpClient = httpClient;
                _disposeHttpClient = false;
            }
            else
            {
                _httpClient = new HttpClient();
                _disposeHttpClient = true;
            }

            // Set default headers
            _httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);
            _httpClient.DefaultRequestHeaders.Add("auth-mode", "nuget");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
#endif

        /// <summary>
        /// Validates the API key format
        /// </summary>
        /// <param name="apiKey">API key to validate</param>
        /// <exception cref="ArgumentException">Thrown when API key is invalid</exception>
        private void ValidateApiKey(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey) || apiKey.Trim().Length == 0)
            {
                throw new ArgumentException("API key is required. Get your API key at: https://apiverve.com");
            }

            // Validate API key format (GUID or alphanumeric with hyphens)
            if (!System.Text.RegularExpressions.Regex.IsMatch(apiKey, @"^[a-zA-Z0-9-]+$"))
            {
                throw new ArgumentException("Invalid API key format. API key must be alphanumeric and may contain hyphens. Get your API key at: https://apiverve.com");
            }

            // Check minimum length (GUIDs are typically 36 chars with hyphens, or 32 without)
            string trimmedKey = apiKey.Replace("-", "");
            if (trimmedKey.Length < 32)
            {
                throw new ArgumentException("Invalid API key. API key appears to be too short. Get your API key at: https://apiverve.com");
            }
        }

        /// <summary>
        /// Gets whether HTTPS is enabled
        /// </summary>
        public bool GetIsSecure() => _isSecure;

        /// <summary>
        /// Gets whether debug mode is enabled
        /// </summary>
        public bool GetIsDebug() => _isDebug;

        /// <summary>
        /// Gets the API endpoint URL
        /// </summary>
        public string GetApiEndpoint() => _apiEndpoint;

        /// <summary>
        /// Sets the API key
        /// </summary>
        /// <param name="apiKey">Your API key from https://apiverve.com</param>
        /// <exception cref="ArgumentException">Thrown when API key is invalid</exception>
        public void SetApiKey(string apiKey)
        {
            ValidateApiKey(apiKey);
            _apiKey = apiKey;
#if NET45 || NETSTANDARD2_0 || NETSTANDARD2_1 || NET6_0
            _httpClient.DefaultRequestHeaders.Remove("x-api-key");
            _httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);
#endif
        }

        /// <summary>
        /// Sets whether to use HTTPS
        /// </summary>
        /// <param name="isSecure">Use HTTPS (recommended)</param>
        public void SetIsSecure(bool isSecure) => _isSecure = isSecure;

        /// <summary>
        /// Sets whether to enable debug logging
        /// </summary>
        /// <param name="isDebug">Enable debug logging</param>
        public void SetIsDebug(bool isDebug) => _isDebug = isDebug;

        /// <summary>
        /// Sets the maximum number of retry attempts for failed requests
        /// </summary>
        /// <param name="maxRetries">Maximum retry attempts (default: 0, max: 3)</param>
        public void SetMaxRetries(int maxRetries) => _maxRetries = Math.Max(0, Math.Min(3, maxRetries));

        /// <summary>
        /// Sets the delay between retry attempts in milliseconds
        /// </summary>
        /// <param name="retryDelayMs">Delay in milliseconds (default: 1000)</param>
        public void SetRetryDelay(int retryDelayMs) => _retryDelayMs = Math.Max(0, retryDelayMs);

#if NET45 || NETSTANDARD2_0 || NETSTANDARD2_1 || NET6_0
        /// <summary>
        /// Sets a custom logger for request/response debugging
        /// </summary>
        /// <param name="logger">Action to call with log messages</param>
        public void SetLogger(Action<string> logger) => _logger = logger;
#endif

        /// <summary>
        /// Adds a custom header to all requests
        /// </summary>
        /// <param name="key">Header name</param>
        /// <param name="value">Header value</param>
        public void AddCustomHeader(string key, string value)
        {
            if (string.IsNullOrEmpty(key) || key.Trim().Length == 0)
            {
                throw new ArgumentException("Header key cannot be empty", "key");
            }

            _customHeaders[key] = value;
        }

        /// <summary>
        /// Removes a custom header
        /// </summary>
        /// <param name="key">Header name</param>
        public void RemoveCustomHeader(string key)
        {
            _customHeaders.Remove(key);
        }

        /// <summary>
        /// Clears all custom headers
        /// </summary>
        public void ClearCustomHeaders()
        {
            _customHeaders.Clear();
        }

#if NET20 || NET35 || NET40
        /// <summary>
        /// Delegate for async callback pattern
        /// </summary>
        /// <param name="result">The API response object</param>
        public delegate void ExecuteAsyncCallback(ResponseObj result);

        /// <summary>
        /// Execute the API call asynchronously using callback pattern
        /// </summary>
        /// <param name="callback">Callback to invoke with the result</param>
        /// <param name="options">Query parameters</param>
        public void ExecuteAsync(ExecuteAsyncCallback callback, FortuneTellerQueryOptions options = null)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }

            System.Threading.ThreadPool.QueueUserWorkItem(state =>
            {
                try
                {
                    ResponseObj result = Execute(options);
                    callback(result);
                }
                catch (Exception ex)
                {
                    callback(new ResponseObj
                    {
                        Status = "error",
                        Error = ex.Message
                    });
                }
            });
        }
#else
        /// <summary>
        /// Delegate for async callback pattern
        /// </summary>
        /// <param name="result">The API response object</param>
        public delegate void ExecuteAsyncCallback(ResponseObj result);

        /// <summary>
        /// Execute the API call asynchronously using callback pattern (legacy)
        /// </summary>
        /// <param name="callback">Callback to invoke with the result</param>
        /// <param name="options">Query parameters</param>
        [Obsolete("Use ExecuteAsync() with async/await pattern instead")]
        public void ExecuteAsync(ExecuteAsyncCallback callback, FortuneTellerQueryOptions options = null)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }

            Task.Run(async () =>
            {
                try
                {
                    ResponseObj result = await ExecuteAsync(options).ConfigureAwait(false);
                    callback(result);
                }
                catch (Exception ex)
                {
                    callback(new ResponseObj
                    {
                        Status = "error",
                        Error = ex.Message
                    });
                }
            });
        }
#endif

        /// <summary>
        /// Execute the API call synchronously
        /// </summary>
        /// <param name="options">Query parameters</param>
        /// <returns>The API response</returns>
        /// <exception cref="WebException">Thrown when the request fails (.NET 2.0-4.0)</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails (.NET 4.5+)</exception>
        public ResponseObj Execute(FortuneTellerQueryOptions options = null)
        {
#if NET45 || NETSTANDARD2_0 || NETSTANDARD2_1 || NET6_0
            return ExecuteAsync(options).GetAwaiter().GetResult();
#else
            return ExecuteWithWebRequest(options);
#endif
        }

#if NET45 || NETSTANDARD2_0 || NETSTANDARD2_1 || NET6_0
        /// <summary>
        /// Execute the API call asynchronously
        /// </summary>
        /// <param name="options">Query parameters</param>
        /// <returns>Task containing the API response</returns>
        /// <exception cref="HttpRequestException">Thrown when the request fails</exception>
        public Task<ResponseObj> ExecuteAsync(FortuneTellerQueryOptions options = null)
        {
            return ExecuteAsync(options, CancellationToken.None);
        }

        /// <summary>
        /// Execute the API call asynchronously with cancellation support
        /// </summary>
        /// <param name="options">Query parameters</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>Task containing the API response</returns>
        /// <exception cref="HttpRequestException">Thrown when the request fails</exception>
        /// <exception cref="OperationCanceledException">Thrown when the operation is cancelled</exception>
        public async Task<ResponseObj> ExecuteAsync(FortuneTellerQueryOptions options, CancellationToken cancellationToken)
        {
            int attempt = 0;
            Exception lastException = null;

            while (attempt <= _maxRetries)
            {
                try
                {
                    if (attempt > 0)
                    {
                        Log(string.Format("Retry attempt {0} of {1}...", attempt, _maxRetries));
                        await Task.Delay(_retryDelayMs, cancellationToken).ConfigureAwait(false);
                    }

                    return await ExecuteRequestAsync(options, cancellationToken).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    // Don't retry on cancellation
                    throw;
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    attempt++;

                    if (attempt > _maxRetries)
                    {
                        Log(string.Format("Request failed after {0} retries: {1}", _maxRetries, ex.Message));
                        break;
                    }

                    // Only retry on transient errors
                    if (!IsTransientError(ex))
                    {
                        Log(string.Format("Non-transient error encountered, not retrying: {0}", ex.Message));
                        break;
                    }
                }
            }

            // If we got here, all retries failed
            throw lastException ?? new HttpRequestException("Request failed");
        }

        /// <summary>
        /// Executes the actual HTTP request
        /// </summary>
        private async Task<ResponseObj> ExecuteRequestAsync(FortuneTellerQueryOptions options, CancellationToken cancellationToken)
        {
            try
            {
                Log("Executing API request...");

                var url = ConstructURL(options);
                Log(string.Format("URL: {0}", url));

                HttpResponseMessage response;

                if (_method == "POST")
                {
                    if (options == null)
                    {
                        throw new ArgumentException("Options are required for this API call");
                    }

                    var body = JsonConvert.SerializeObject(options);
                    Log(string.Format("Request body: {0}", body));

                    var content = new StringContent(body, Encoding.UTF8, "application/json");

                    // Add custom headers to request
                    var request = new HttpRequestMessage(HttpMethod.Post, url)
                    {
                        Content = content
                    };

                    AddCustomHeadersToRequest(request);

                    response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
                }
                else // GET
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, url);
                    AddCustomHeadersToRequest(request);

                    response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
                }

                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                Log(string.Format("Response: {0}", responseString));

                if (string.IsNullOrEmpty(responseString))
                {
                    throw new HttpRequestException("No response from the server");
                }

                var responseObj = JsonConvert.DeserializeObject<ResponseObj>(responseString);
                return responseObj;
            }
            catch (Exception ex)
            {
                Log(string.Format("Error: {0}", ex.Message));
                throw;
            }
        }

        /// <summary>
        /// Adds custom headers to the HTTP request
        /// </summary>
        private void AddCustomHeadersToRequest(HttpRequestMessage request)
        {
            foreach (var header in _customHeaders)
            {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        /// <summary>
        /// Determines if an error is transient and worth retrying
        /// </summary>
        private bool IsTransientError(Exception ex)
        {
            if (ex is HttpRequestException)
            {
                // Network errors are typically transient
                return true;
            }

            if (ex is TaskCanceledException)
            {
                // Timeout errors are transient
                return true;
            }

            return false;
        }
#endif

#if NET20 || NET35 || NET40
        /// <summary>
        /// Execute the API call using WebRequest (for .NET 2.0-4.0)
        /// </summary>
        private ResponseObj ExecuteWithWebRequest(FortuneTellerQueryOptions options)
        {
            int attempt = 0;
            Exception lastException = null;

            while (attempt <= _maxRetries)
            {
                try
                {
                    if (attempt > 0)
                    {
                        Log(string.Format("Retry attempt {0} of {1}...", attempt, _maxRetries));
                        System.Threading.Thread.Sleep(_retryDelayMs);
                    }

                    return ExecuteWebRequestInternal(options);
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    attempt++;

                    if (attempt > _maxRetries)
                    {
                        Log(string.Format("Request failed after {0} retries: {1}", _maxRetries, ex.Message));
                        break;
                    }

                    // Only retry on transient errors
                    if (!IsTransientWebError(ex))
                    {
                        Log(string.Format("Non-transient error encountered, not retrying: {0}", ex.Message));
                        break;
                    }
                }
            }

            throw lastException ?? new Exception("Request failed");
        }

        /// <summary>
        /// Internal WebRequest execution
        /// </summary>
        private ResponseObj ExecuteWebRequestInternal(FortuneTellerQueryOptions options)
        {
            try
            {
                Log("Executing API request...");

                var url = ConstructURL(options);
                Log(string.Format("URL: {0}", url));

                var request = WebRequest.Create(url);
                request.Headers["x-api-key"] = _apiKey;
                request.Headers["auth-mode"] = "nuget";
                request.Method = _method;

                // Add custom headers
                foreach (var header in _customHeaders)
                {
                    request.Headers[header.Key] = header.Value;
                }

                if (_method == "POST")
                {
                    if (options == null)
                    {
                        throw new Exception("Options are required for this call");
                    }

                    var body = JsonConvert.SerializeObject(options);
                    Log(string.Format("Request body: {0}", body));

                    var data = Encoding.UTF8.GetBytes(body);

                    request.ContentType = "application/json";
                    request.ContentLength = data.Length;

                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }

                string responseString;
                try
                {
                    using (var response = request.GetResponse())
                    {
                        using (var reader = new StreamReader(response.GetResponseStream()))
                        {
                            responseString = reader.ReadToEnd();
                        }
                    }
                }
                catch (WebException e)
                {
                    if (e.Response != null)
                    {
                        using (var reader = new StreamReader(e.Response.GetResponseStream()))
                        {
                            responseString = reader.ReadToEnd();
                        }
                    }
                    else
                    {
                        throw;
                    }
                }

                Log(string.Format("Response: {0}", responseString));

                if (string.IsNullOrEmpty(responseString))
                {
                    throw new Exception("No response from the server");
                }

                var responseObj = JsonConvert.DeserializeObject<ResponseObj>(responseString);
                return responseObj;
            }
            catch (Exception e)
            {
                Log(string.Format("Error: {0}", e.Message));
                throw;
            }
        }

        /// <summary>
        /// Determines if a WebException is transient
        /// </summary>
        private bool IsTransientWebError(Exception ex)
        {
            if (ex is WebException)
            {
                return true;
            }
            return false;
        }
#endif

        /// <summary>
        /// Logs a message if debug mode is enabled or a custom logger is set
        /// </summary>
        private void Log(string message)
        {
#if NET45 || NETSTANDARD2_0 || NETSTANDARD2_1 || NET6_0
            if (_logger != null)
            {
                _logger(message);
            }
            else if (_isDebug)
            {
                Console.WriteLine(message);
            }
#else
            if (_isDebug)
            {
                Console.WriteLine(message);
            }
#endif
        }

        /// <summary>
        /// Constructs the full API URL with query parameters
        /// </summary>
        private string ConstructURL(FortuneTellerQueryOptions options)
        {
            string url = _apiEndpoint;

            if (options != null && _method == "GET")
            {
                var queryParams = new List<string>();

                foreach (var prop in options.GetType().GetProperties())
                {
                    var value = prop.GetValue(options, null);
                    if (value != null)
                    {
                        // Get the JsonProperty attribute name if present, otherwise use property name
                        string paramName = prop.Name;
                        var jsonPropertyAttr = prop.GetCustomAttributes(typeof(JsonPropertyAttribute), false);
                        if (jsonPropertyAttr.Length > 0)
                        {
                            var attr = (JsonPropertyAttribute)jsonPropertyAttr[0];
                            if (!string.IsNullOrEmpty(attr.PropertyName))
                            {
                                paramName = attr.PropertyName;
                            }
                        }

                        queryParams.Add(string.Format("{0}={1}", paramName, Uri.EscapeDataString(value.ToString())));
                    }
                }

                if (queryParams.Count > 0)
                {
#if NET20 || NET35
                    url += "?" + string.Join("&", queryParams.ToArray());
#else
                    url += "?" + string.Join("&", queryParams);
#endif
                }
            }

            return url;
        }

#if NET45 || NETSTANDARD2_0 || NETSTANDARD2_1 || NET6_0
        /// <summary>
        /// Disposes the HttpClient if it was created internally
        /// </summary>
        public void Dispose()
        {
            if (_disposeHttpClient && _httpClient != null)
            {
                _httpClient.Dispose();
            }
        }
#endif
    }
}
