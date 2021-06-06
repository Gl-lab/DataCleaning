using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DataCleaning.Interfaces;
using Microsoft.VisualBasic;

namespace DataCleaning
{
    public class JsonClear: ICleanString
    {
        private readonly ClearOptions _options;
        public JsonClear(ClearOptions options)
        {
            _options = options;
        }
        public string Clean(string cleaningString)
        {
           
            var replace = new List<Tuple<string,string>>();
            foreach (var secureString in _options.SecureStrings)
            {
                var regex = new Regex(secureString+"\\s*:\\s*[0-9]{1,16}",
                    _options.IsIgnoreCase?RegexOptions.IgnoreCase:RegexOptions.None);

                replace.AddRange(GetReplacementForNumericList(cleaningString, regex));
                
                regex = new Regex(secureString+"\\s*:\\s*\\\"[a-z0-9_-]{1,16}\\\"",
                    _options.IsIgnoreCase?RegexOptions.IgnoreCase:RegexOptions.None);

                replace.AddRange(GetReplacementForStringsList(cleaningString, regex));
            }

            foreach (var (oldString, newString) in replace)
            {
                cleaningString = cleaningString.Replace(oldString, newString);
            }

            return cleaningString;
        }
        
        private IEnumerable<Tuple<string,string>> GetReplacementForStringsList(string cleaningString, Regex regex)
        {
            var matches = regex.Matches(cleaningString);
           
            return matches.Select(m =>
            {
                var firstQuoteIndex = m.Value.IndexOf("\"", StringComparison.Ordinal);
                var lastQuoteIndex = m.Value.IndexOf("\"", firstQuoteIndex+1, StringComparison.Ordinal);
                return new Tuple<string, string>(m.Value,
                    m.Value[..(firstQuoteIndex+ 1)] +
                    Strings.StrDup((lastQuoteIndex - firstQuoteIndex-1), "X")+"\"");

            });
        }
        private IEnumerable<Tuple<string,string>> GetReplacementForNumericList(string cleaningString, Regex regex)
        {
            var matches = regex.Matches(cleaningString);
           
            return matches.Select(m =>
            {
                var index = m.Value.IndexOf(":",StringComparison.Ordinal);
                var firstDigit = m.Value.IndexOfAny(new[] {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'}, index);
                return new Tuple<string, string>(m.Value,
                    m.Value[..firstDigit] + "\""+
                    Strings.StrDup(m.Value.Length - firstDigit, "X")+"\"");

            });
        }
    }
}