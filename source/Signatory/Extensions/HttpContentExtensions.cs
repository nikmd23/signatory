using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Signatory.Extensions
{
    public static class HttpContentExtensions
    {
        public static async Task<dynamic> ReadAsDynamicJsonAsync(this HttpContent content)
        {
            return JArray.Parse(await content.ReadAsStringAsync());
        }

        public static async Task<dynamic> ReadAsDynamicJsonObjectAsync(this HttpContent content)
        {
            return JObject.Parse(await content.ReadAsStringAsync());
        }
    }
}