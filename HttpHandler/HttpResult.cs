using DataCleaning.Interfaces;

namespace DataCleaning
{
    public class HttpResult: IHttpResult
    {
        public string Url { get; set; }
        public string RequestBody { get; set; }
        public string ResponseBody { get; set; }
    }
}