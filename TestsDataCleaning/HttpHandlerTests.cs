
using DataCleaning;
using DataCleaning.Interfaces;
using Moq;
using Xunit;

namespace TestsDataCleaning
{
    public class HttpHandlerTests
    {
        [Fact]
        public void TestHttpHandler_TestProcess_()
        {
            //Arrange
            
            var bookingcomHttpResultMock = new Mock<IHttpResult>();
            var url = "http://test.com/users/max/info?pass=123456";
            var requestBody = "http://test.com?user=max";
            var responseBody = "http://test.com?user=max&pass=123456";
            
            bookingcomHttpResultMock
                .SetupProperty(e => e.Url, url)
                .SetupProperty(e => e.ResponseBody, responseBody)
                .SetupProperty(e => e.RequestBody, requestBody);
            var bookingcomHttpResult = bookingcomHttpResultMock.Object;

            var httpResultCleaner = new Mock<IHttpResultCleaner>();
            httpResultCleaner
                .Setup(e => e.Clean(It.IsAny<IHttpResult>()))
                .Callback((IHttpResult result) =>
                {
                    result.Url = "http://test.com/users/XXX/info?pass=XXXXXX";
                    result.RequestBody = "http://test.com?user=XXX&pass=XXXXXX";
                    result.ResponseBody = "http://test.com?user=XXX&pass=XXXXXX";
                });
           
            var httpLogHandler = new HttpHandler();
 
 
            //Act
            httpLogHandler.Process( bookingcomHttpResult.Url, bookingcomHttpResult.RequestBody, bookingcomHttpResult.ResponseBody,httpResultCleaner.Object );
 
            //Assert
            httpResultCleaner.Verify(e => e.Clean(It.IsAny<IHttpResult>()), Times.Once());
            Assert.Equal("http://test.com/users/XXX/info?pass=XXXXXX", httpLogHandler.CurrentLog.Url);
            Assert.Equal("http://test.com?user=XXX&pass=XXXXXX", httpLogHandler.CurrentLog.RequestBody);
            Assert.Equal("http://test.com?user=XXX&pass=XXXXXX", httpLogHandler.CurrentLog.ResponseBody);

        }
    }
}