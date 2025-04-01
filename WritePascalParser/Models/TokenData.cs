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
        public string TokenNewValue { get; set; }

        public TokenData(TokenEnum tokenEnum, string tokenValue) 
        { 
            TokenEnum = tokenEnum;
            TokenValue = tokenValue;
            TokenNewValue = string.Empty;
        }

        public string PrintTokenValue()
        {
            return TokenEnum.ToString() + " " + TokenValue + TokenNewValue + "\n";
        }
    }
}
