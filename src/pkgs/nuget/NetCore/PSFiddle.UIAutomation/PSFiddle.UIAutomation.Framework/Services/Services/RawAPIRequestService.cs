using MC.Track.TestSuite.Model.Domain;
using MC.Track.TestSuite.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Interfaces.Attributes;

namespace MC.Track.TestSuite.Services.Services
{
    [AthenaRegister(typeof(IRawAPIRequestService), Model.Enums.AthenaRegistrationType.Singleton)]
    public class RawAPIRequestService : IRawAPIRequestService
    {
        public async Task<ApiResult> ExecuteApiAsync(string requestUrl, AuthenticationType authenticationType, string accesstoken, string httpMethod, string contentType, string requestContent = "")
        {
            var data = String.Empty;
            var result = new ApiResult();
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            using (HttpClient client = new HttpClient())
            {

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                if (authenticationType == AuthenticationType.SharedAccessSignature)
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("SharedAccessSignature", accesstoken);
                else if (authenticationType == AuthenticationType.Bearer)
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);

                HttpResponseMessage response = null;
                try
                {
                    if (httpMethod.ToLower() == "put")
                    {
                        var content = new StringContent(requestContent, Encoding.UTF8, contentType);
                        response = await client.PutAsync(requestUrl, content).ConfigureAwait(false);
                    }
                    else if (httpMethod.ToLower() == "post")
                    {
                        var content = new StringContent(requestContent, Encoding.UTF8, contentType);
                        response = await client.PostAsync(requestUrl, content).ConfigureAwait(false);
                    }
                    else if (httpMethod.ToLower() == "get")
                    {
                        response = await client.GetAsync(requestUrl).ConfigureAwait(false);
                    }
                    else
                    {
                        throw new ArgumentException("Please use a valid http method (get, put, post");
                    }
                    result.ReturnValue = response.StatusCode;
                    data = await response?.Content?.ReadAsStringAsync();
                    result.Data = data;
                }
                catch (HttpRequestException ex)
                {
                    result.Data = $"Message: {ex.Message} Inner Exception: {ex.InnerException}";
                    result.ReturnValue = System.Net.HttpStatusCode.InternalServerError;
                }
                result.ContentType = contentType;
                result.HttpMethod = httpMethod;
                result.Url = requestUrl;

            }

            return result;
        }
    }
}
