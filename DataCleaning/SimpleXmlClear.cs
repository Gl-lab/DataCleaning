using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DataCleaning.Interfaces;
using Microsoft.VisualBasic;

namespace DataCleaning
{
    public class SimpleXmlClear: ICleanString
    {
        private readonly ClearOptions _options;
        public SimpleXmlClear(ClearOptions options)
        {
            _options = options;
        }
        public string Clean(string cleaningString)
        {
            var replace = new List<Tuple<string,string>>();
            foreach (var secureString in _options.SecureStrings)
            {
                var regex = new Regex("<"+secureString+">[a-z0-9_-]{1,16}</"+secureString+">",
                    _options.IsIgnoreCase?RegexOptions.IgnoreCase:RegexOptions.None);

                replace.AddRange(GetReplacement(cleaningString, regex,secureString));
            }

            foreach (var (oldString, newString) in replace)
            {
                cleaningString = cleaningString.Replace(oldString, newString);
            }

            return cleaningString;
        }

        private IEnumerable<Tuple<string, string>> GetReplacement(string cleaningString, Regex regex,
            string secureString)
        {
            var matches = regex.Matches(cleaningString);

            return matches.Select(m =>
            {
                var firstIndex = m.Value.IndexOf("<" + secureString + ">", _options.IsIgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal)+secureString.Length+1;
                var lastIndex = m.Value.IndexOf("</" + secureString + ">", firstIndex, _options.IsIgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
                return new Tuple<string, string>(m.Value,
                    m.Value[..(firstIndex + 1)] +
                    Strings.StrDup(lastIndex - firstIndex-1, "X")+
                    m.Value[lastIndex..]);

            });
        }
    }
}