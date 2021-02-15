using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;

namespace BeerQuest.Functions.Extensions
{
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// Converts a request's query string into an object
        /// </summary>
        /// <typeparam name="TObj"></typeparam>
        /// <param name="req"></param>
        /// <returns></returns>
        public static TObj GetQueryAsObject<TObj>(this HttpRequest req)
        {
            var dic = req.GetQueryParameterDictionary();

            var str = JsonConvert.SerializeObject(dic);
            var obj = JsonConvert.DeserializeObject<TObj>(str);

            return obj;
        }
    }
}
