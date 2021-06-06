using System.Collections.Generic;
using DataCleaning;
using Xunit;

namespace TestsDataCleaning
{
    public class HttpGetStringClearTests
    {
        [Fact]
        public void TestHttpStringClear_TestClear_MixedHttpString_ClearSecureData()
        {
            //Arrange
            const string dirtyString = "http://test.com/users/max/info?pass=123456";
            var options = new ClearOptions
            {
                SecureStrings = new List<string>
                {
                    "users",
                    "pass"
                }
            };
            var httpStringClear = new HttpStringClear(options);
            
            //Act
            var cleanString = httpStringClear.Clean(dirtyString);
            
            //Assert
            Assert.Equal("http://test.com/users/XXX/info?pass=XXXXXX",cleanString);
        }
        
        [Fact]
        public void TestHttpStringClear_TestClean_RestHttpString_ClearSecureData()
        {
            //Arrange
            const string dirtyString = "http://users.com/users/max/";
            var options = new ClearOptions
            {
                SecureStrings = new List<string> {
                    "users",
                    "pass"
                }
            };
            
            var httpStringClear = new HttpStringClear(options);
            
            //Act
            var cleanString = httpStringClear.Clean(dirtyString);
            
            //Assert
            Assert.Equal("http://users.com/users/XXX/",cleanString);
        }
        
        [Fact]
        public void TestHttpStringClear_TestClean_MixedHttpStringWithNosecureData_ClearSecureData()
        {
            //Arrange
            const string dirtyString = "http://test.com/users/max?size=max";
            var options = new ClearOptions
            {
                SecureStrings = new List<string> {
                    "users",
                    "pass"
                }
            };
            
            var httpStringClear = new HttpStringClear(options);
            
            //Act
            var cleanString = httpStringClear.Clean(dirtyString);
            
            //Assert
            Assert.Equal("http://test.com/users/XXX?size=max",cleanString);
        }
        
        [Fact]
        public void TestHttpStringClear_TestClean_RestHttpStringTwoSecureData_ClearSecureData()
        {
            //Arrange
            const string dirtyString = "http://test.com/users/max/pass/dfsfds";
            var options = new ClearOptions
            {
                SecureStrings = new List<string> {
                    "users",
                    "pass"
                }
            };
            
            var httpStringClear = new HttpStringClear(options);
            
            //Act
            var cleanString = httpStringClear.Clean(dirtyString);
            
            //Assert
            Assert.Equal("http://test.com/users/XXX/pass/XXXXXX",cleanString);
        }
        
        
        [Fact]
        public void TestHttpStringClear_TestClean_RestHttpStringTwoSecureDataFirstEmpty_ClearSecureData()
        {
            //Arrange
            const string dirtyString = "http://test.com/users//pass/123456";
            var options = new ClearOptions
            {
                SecureStrings = new List<string> {
                    "users",
                    "pass"
                }
            };
            
            var httpStringClear = new HttpStringClear(options);
            
            //Act
            var cleanString = httpStringClear.Clean(dirtyString);
            
            //Assert
            Assert.Equal("http://test.com/users//pass/XXXXXX",cleanString);
        }
        
        [Fact]
        public void TestHttpStringClear_TestClean_IgnoreCase_ClearSecureData()
        {
            //Arrange
            const string dirtyString = "http://test.com/useRs/max";
            var options = new ClearOptions
            {
                SecureStrings = new List<string> {
                    "Users",
                    "pass"
                }
            };
            
            var httpStringClear = new HttpStringClear(options);
            
            //Act
            var cleanString = httpStringClear.Clean(dirtyString);
            
            //Assert
            Assert.Equal("http://test.com/useRs/XXX",cleanString);
        }

        [Fact]
        public void TestHttpStringClear_RestClean_IgnoreCase_NoChange()
        {
            //Arrange
            const string dirtyString = "http://test.com/users/max";
            var options = new ClearOptions
            {
                SecureStrings = new List<string> {
                    "Users",
                    "pass"
                },
                IsIgnoreCase = false
            };
            
            var httpStringClear = new HttpStringClear(options);
            
            //Act
            var cleanString = httpStringClear.Clean(dirtyString);
            
            //Assert
            Assert.Equal(dirtyString,cleanString);
        }
        
        [Fact]
        public void TestHttpStringClear_Clean_EmptyString_EmptyString()
        {
            //Arrange
            var options = new ClearOptions
            {
                SecureStrings = new List<string> {
                    "Users",
                    "pass"
                },
                IsIgnoreCase = false
            };
            
            var httpStringClear = new HttpStringClear(options);
            
            //Act
            var cleanString = httpStringClear.Clean(string.Empty);
            
            //Assert
            Assert.Equal(string.Empty,cleanString);
        }
    }
}