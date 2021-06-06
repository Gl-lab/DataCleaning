namespace DataCleaning.Interfaces
{
    public interface IHttpResult
    {
        public string Url { get; set; }
        public string RequestBody  { get; set; }
        public string ResponseBody  { get; set; }
    }
}