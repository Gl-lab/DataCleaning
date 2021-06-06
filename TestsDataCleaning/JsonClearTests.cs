using System.Collections.Generic;
using DataCleaning;
using Xunit;

namespace TestsDataCleaning
{
    public class JsonClearTests
    {
        [Fact]
        public void TestJsonClear_TestClear_BaseJsonWithString_ClearSecureData()
        {
            //Arrange
            const string dirtyString = "{users:\"max\"}";
            var options = new ClearOptions
            {
                SecureStrings = new List<string>
                {
                    "users",
                    "pass"
                }
            };
            var jsonClear = new JsonClear(options);
            
            //Act
            var cleanString = jsonClear.Clean(dirtyString);
            
            //Assert
            Assert.Equal("{users:\"XXX\"}",cleanString);
        }
        
        [Fact]
        public void TestJsonClear_TestClear_BaseJsonWithNumeric_ClearSecureData()
        {
            //Arrange
            const string dirtyString = "{pass:123456}";
            var options = new ClearOptions
            {
                SecureStrings = new List<string>
                {
                    "users",
                    "pass"
                }
            };
            var jsonClear = new JsonClear(options);
            
            //Act
            var cleanString = jsonClear.Clean(dirtyString);
            
            //Assert
            Assert.Equal("{pass:\"XXXXXX\"}",cleanString);
        }
        
        [Fact]
        public void TestJsonClear_TestClear_BaseJsonWithSpaces_ClearSecureData()
        {
            //Arrange
            const string dirtyString = "{users :  \"max\",pass   :    123456}";
            var options = new ClearOptions
            {
                SecureStrings = new List<string>
                {
                    "users",
                    "pass"
                }
            };
            var jsonClear = new JsonClear(options);
            
            //Act
            var cleanString = jsonClear.Clean(dirtyString);
            
            //Assert
            Assert.Equal("{users :  \"XXX\",pass   :    \"XXXXXX\"}",cleanString);
        }
    }
}