using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WritePascalParser.Models
{
    public class ErrorNeutralization
    {
        public string ErrorValue {  get; set; }

        public TokenEnum? ExpectedToken { get; set; }
        public TokenData TokenData { get; set; }


        
        public ErrorNeutralization (string errorValue, TokenEnum? expectedToken, TokenData tokenData)
        {
            ErrorValue = errorValue;
            ExpectedToken = expectedToken;
            TokenData = tokenData;
        }
    }
}
