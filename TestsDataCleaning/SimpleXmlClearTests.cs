using System.Collections.Generic;
using DataCleaning;
using Xunit;

namespace TestsDataCleaning
{
    public class SimpleXmlClearTests
    {
        [Fact]
        public void TestSimpleXmlClear_TestClear_BaseXml_ClearSecureData()
        {
            //Arrange
            const string dirtyString = "<?xml version=\"1.0\"?><CAT><USERS>max</USERS></CAT>";
            var options = new ClearOptions
            {
                SecureStrings = new List<string>
                {
                    "users",
                    "pass"
                }
            };
            var simpleXmlClear = new SimpleXmlClear(options);
            
            //Act
            var cleanString = simpleXmlClear.Clean(dirtyString);
            
            //Assert
            Assert.Equal("<?xml version=\"1.0\"?><CAT><USERS>XXX</USERS></CAT>",cleanString);
        }
        
        [Fact]
        public void TestSimpleXmlClear_TestClear_IsNotIgnoreCase_NoChanges()
        {
            //Arrange
            const string dirtyString = "<?xml version=\"1.0\"?><CAT><USERS>max</USERS></CAT>";
            var options = new ClearOptions
            {
                SecureStrings = new List<string>
                {
                    "users",
                    "pass"
                },IsIgnoreCase = false
            };
            var simpleXmlClear = new SimpleXmlClear(options);
            
            //Act
            var cleanString = simpleXmlClear.Clean(dirtyString);
            
            //Assert
            Assert.Equal("<?xml version=\"1.0\"?><CAT><USERS>max</USERS></CAT>",cleanString);
        }
    }
}