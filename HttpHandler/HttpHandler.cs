using DataCleaning.Interfaces;

namespace DataCleaning
{
    public class HttpHandler
    {
        IHttpResult _currentLog;
        public IHttpResult CurrentLog => _currentLog;

        public string Process( string url, string body, string response, IHttpResultCleaner httpResultCleaner)
        {
            var httpResult = new HttpResult
            {
                Url = url,
                RequestBody = body,
                ResponseBody = response
            };
            
            httpResultCleaner.Clean(httpResult);
        
            Log( httpResult );
            return response;
        }
 
        /// <summary>
        /// Логирует данные запроса, они должны быть уже без данных которые нужно защищать 
        /// </summary>
        /// <param name="result"></param>
        protected void Log( HttpResult result )
        {
            _currentLog = new HttpResult
            {
                Url = result.Url,
                RequestBody = result.RequestBody,
                ResponseBody = result.ResponseBody
            };
        }
    }

}