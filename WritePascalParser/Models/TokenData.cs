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
        public int LineNumber { get; set; }
        public int LineOffset {  get; set; } // позиция в строке
        public int ErrorSymbolLineOffset { get; set; } // позиция в строке неверного символа (если есть)

        public TokenData(TokenEnum tokenEnum, string tokenValue, int lineNumber, 
            int lineOffset) 
        { 
            TokenEnum = tokenEnum;
            TokenValue = tokenValue;
            TokenNewValue = string.Empty;
            LineNumber = lineNumber;
            LineOffset = lineOffset;
            ErrorSymbolLineOffset = 1;
        }

        public string PrintTokenValue()
        {
            return TokenEnum.ToString() + " " + TokenValue + TokenNewValue + "\n";
        }
    }
}
