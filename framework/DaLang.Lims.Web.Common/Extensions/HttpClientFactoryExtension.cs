using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DaLang.Lims.Web.Common.Extensions;

public static class HttpClientFactoryExtension
{
    public static async Task<string> HttpSendAsync(this IHttpClientFactory httpClientFactory,
        HttpMethod method,
        string url,
        string param,
        Dictionary<string, string> headers = null,
        int ApiTimeout = 100
        )
    {
        var client = httpClientFactory.CreateClient();
        client.Timeout = TimeSpan.FromSeconds(ApiTimeout);

        var postContent = param == null ? null : new StringContent(param, Encoding.UTF8, "application/json");
        // var content = new StringContent("");
        // content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
        var request = new HttpRequestMessage(method, url)
        {
            Content = postContent
        };
        if (headers != null)
        {
            foreach (var header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }
        }

        HttpResponseMessage httpResponseMessage = await client.SendAsync(request);

        var result = await httpResponseMessage.Content
            .ReadAsStringAsync();
        return result;

    }
}
