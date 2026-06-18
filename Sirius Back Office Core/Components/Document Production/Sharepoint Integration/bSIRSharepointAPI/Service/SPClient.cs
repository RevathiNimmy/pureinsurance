using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;

namespace bSIRSharepointApi.Service
{
    public class SPClient
    {
        private HttpClient _httpClient;

        public SPClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Execute HTTP Get and POST
        /// </summary>
        /// <param name="httpMethod"></param>
        /// <param name="oDataUrl"></param>
        /// <param name="body"></param>
        /// <param name="bAddMergeHeaders"></param>
        /// <param name="byteArrayContent"></param>
        /// <returns></returns>
        public HttpResponseMessage ExcuteJson(HttpMethod httpMethod, string oDataUrl, [Optional] string body, [Optional] bool bAddMergeHeaders, [Optional] ByteArrayContent byteArrayContent)
        {
            HttpRequestMessage httpReqMsg = new HttpRequestMessage(HttpMethod.Get, oDataUrl);
            if (httpMethod.Method == HttpMethod.Post.Method)
            {
                httpReqMsg = new HttpRequestMessage(HttpMethod.Post, oDataUrl);
                if (body != null)
                {
                    httpReqMsg.Content = new StringContent(body);
                    MediaTypeHeaderValue sharePointJsonMediaType = null;
                    MediaTypeHeaderValue.TryParse("application/json;odata=verbose", out sharePointJsonMediaType);
                    httpReqMsg.Content.Headers.ContentType = sharePointJsonMediaType;
                }
                else if (byteArrayContent != null)
                {
                    httpReqMsg.Content = byteArrayContent;
                }
            }

            httpReqMsg.Headers.Add("Accept", "application/json;odata=light;q=1,application/json;odata=verbose;q=0.5");
            if (bAddMergeHeaders)
            {
                _httpClient.DefaultRequestHeaders.Add("X-HTTP-Method", "MERGE");
                _httpClient.DefaultRequestHeaders.Add("If-Match", "*");
            }
            HttpResponseMessage response = _httpClient.SendAsync(httpReqMsg).Result;
            if (bAddMergeHeaders)
            {
                _httpClient.DefaultRequestHeaders.Remove("X-HTTP-Method");
                _httpClient.DefaultRequestHeaders.Remove("If-Match");
            }
            
                return response;
       }
    }
}
