using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WritePascalParser.Models
{
    public class ErrorLexer
    {
        public string ErrorValue { get; set; }
        public TokenData TokenData { get; set; }

        public ErrorLexer(string errorValue, TokenData tokenData)
        {
            ErrorValue = errorValue;
            TokenData = tokenData;
        }
    }
}
