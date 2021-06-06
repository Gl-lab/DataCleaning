using DataCleaning.Interfaces;

namespace DataCleaning
{
    public class HttpResultCleaner: IHttpResultCleaner
    {
        private readonly ICleanString _urlCleaner;
        private readonly ICleanString _requestBodyCleaner;
        private readonly ICleanString _responseBodyCleaner;

        public HttpResultCleaner(ICleanString urlCleaner, 
            ICleanString requestBodyCleaner,
            ICleanString responseBodyCleaner)
        {
            _urlCleaner = urlCleaner;
            _requestBodyCleaner = requestBodyCleaner;
            _responseBodyCleaner = responseBodyCleaner;
        }

        public void Clean(IHttpResult httpResult)
        {
            httpResult.Url = _urlCleaner.Clean(httpResult.Url);
            httpResult.RequestBody = _requestBodyCleaner.Clean(httpResult.RequestBody);
            httpResult.ResponseBody = _responseBodyCleaner.Clean(httpResult.ResponseBody);
        }
        
        
        
    }
}