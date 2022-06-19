using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Test.API.Controller
{
    public class BaseController : Build
    {
        protected StringContent PostType(object request)
        {
            return new StringContent(JsonConvert.SerializeObject(request).ToString(),
            Encoding.UTF8, "application/json");
        }
    }
}