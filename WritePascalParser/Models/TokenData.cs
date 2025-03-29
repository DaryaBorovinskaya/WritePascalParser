using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WritePascalParser.Models
{
    public class TokenData
    {
        public TokenEnum TokenEnum { get; set; }
        public string TokenValue { get; set; }

        public TokenData(TokenEnum tokenEnum, string tokenValue) 
        { 
            TokenEnum = tokenEnum;
            TokenValue = tokenValue;
        }

        public string PrintTokenValue()
        {
            return TokenEnum.ToString() + " " + TokenValue.ToString() + "\n";
        }
    }
}
