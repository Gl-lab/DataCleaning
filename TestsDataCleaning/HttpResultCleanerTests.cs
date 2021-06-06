using System;
using DataCleaning;
using DataCleaning.Interfaces;
using Moq;
using Xunit;

namespace TestsDataCleaning
{
    public class HttpResultCleanerTests
    {
        [Fact]
        public void TestHttpResultCleaner_TestClean_MockHttpResult_ClearSecureData()
        {
            //Arrange
            var urlCleanMock = new Mock<ICleanString>();
            urlCleanMock
                .Setup(e => e.Clean(It.Is<string>(s => s ==  "http://test.com/users/max/info?pass=123456" )))
                .Returns("http://test.com/users/XXX/info?pass=XXXXXX");
            var requestBodyCleanMock = new Mock<ICleanString>();
            requestBodyCleanMock
                .Setup(e => e.Clean(It.Is<string>(s => s ==  "http://test.com?user=max" )))
                .Returns("http://test.com?user=XXX");
            var responseBodyCleanMock = new Mock<ICleanString>();
            responseBodyCleanMock
                .Setup(e => e.Clean(It.Is<string>(s => s == "http://test.com?user=max&pass=123456" )))
                .Returns("http://test.com?user=XXX&pass=XXXXXX");
            var bookingcomHttpResult = new Mock<IHttpResult>();
            var url = "http://test.com/users/max/info?pass=123456";
            var requestBody = "http://test.com?user=max";
            var responseBody = "http://test.com?user=max&pass=123456";
            
            bookingcomHttpResult
                .SetupProperty(e => e.Url, url)
                .SetupProperty(e => e.ResponseBody, responseBody)
                .SetupProperty(e => e.RequestBody, requestBody);
            
            var httpResultCleaner = new HttpResultCleaner(urlCleanMock.Object, 
                requestBodyCleanMock.Object,
                responseBodyCleanMock.Object);
            
            //Act
            httpResultCleaner.Clean(bookingcomHttpResult.Object);
            
            //Assert
            Assert.Equal("http://test.com/users/XXX/info?pass=XXXXXX",bookingcomHttpResult.Object.Url);
            Assert.Equal("http://test.com?user=XXX",bookingcomHttpResult.Object.RequestBody);
            Assert.Equal("http://test.com?user=XXX&pass=XXXXXX",bookingcomHttpResult.Object.ResponseBody);

        }
    }
}