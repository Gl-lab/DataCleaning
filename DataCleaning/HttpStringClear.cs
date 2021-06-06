using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using DataCleaning.Interfaces;
using Microsoft.VisualBasic;

namespace DataCleaning
{
    public class HttpStringClear: ICleanString
    {

        private readonly ClearOptions _options;
        public HttpStringClear(ClearOptions options)
        {
            _options = options;
        }
        private IEnumerable<Tuple<string,string>> GetReplacementList(string cleaningString, Regex regex,
            string secureString, ICollection<char> endChars)
        {
            var matches = regex.Matches(cleaningString);
           
            return matches.Select(m =>
            {
                var index = m.Value.IndexOf(secureString,
                    _options.IsIgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
                var endChar = m.Value.Last();
                return new Tuple<string, string>(m.Value,
                    m.Value[..(index + secureString.Length + 1)] +
                    Strings.StrDup(m.Value.Length - (endChars.Contains(endChar) ? 1 : 0) - 
                                   (index + secureString.Length + 1), "X") +
                    (endChars.Contains(endChar) ? endChar : string.Empty));
            });
        }

        public string Clean(string cleaningString)
        {
            var endChar = new List<char> {'/', '?', '&'};
            var replace = new List<Tuple<string,string>>();
            foreach (var secureString in _options.SecureStrings)
            {
                var regex = new Regex("[&?]"+secureString+@"=[a-z0-9_-]{1,16}([&]|$)",
                    _options.IsIgnoreCase?RegexOptions.IgnoreCase:RegexOptions.None);

                replace.AddRange(GetReplacementList(cleaningString, regex, secureString, endChar));
                
                regex = new Regex("/"+secureString+@"/[a-z0-9_-]{1,16}([/\?]|$)",
                    _options.IsIgnoreCase?RegexOptions.IgnoreCase:RegexOptions.None); 
                replace.AddRange(GetReplacementList(cleaningString, regex, secureString, endChar));
            }

            foreach (var (oldString, newString) in replace)
            {
                cleaningString = cleaningString.Replace(oldString, newString);
            }

            return cleaningString;
        }
    }
}