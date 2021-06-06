using System.Collections.Generic;

namespace DataCleaning
{
    public class ClearOptions
    {
        public List<string> SecureStrings { get; set; }
        public bool IsIgnoreCase { get; set; } = true;
    }
}