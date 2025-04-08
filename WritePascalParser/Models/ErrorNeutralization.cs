using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WritePascalParser.Models
{
    public class ErrorNeutralization
    {
        public string ErrorValue {  get; set; }

        public TokenEnum? ExpectedToken { get; set; }
        public TokenData TokenData { get; set; }

        public  List<ErrorNeutralization> ErrorEarlyEnding {  get; private set; } 
       
        public ErrorNeutralization()
        {
            ErrorEarlyEnding = new List<ErrorNeutralization>
            {
                new(
                $"Строка {1} знак {1} " +
                $"ОШИБКА: ожидалось ключевое слово write",
                TokenEnum.Write,
                null),

                new(
                $"Строка {1} знак {1} " +
                $"ОШИБКА: ожидался символ \"(\"",
                TokenEnum.OpenBracket,
                null),

                new(
                $"Строка {1} знак {1} " +
                $"ОШИБКА: ожидался символ \")\"",
                TokenEnum.OpenBracket,
                null),

                new(
                $"Строка {1} знак {1} " +
                $"ОШИБКА: ожидался символ \";\"",
                TokenEnum.OpenBracket,
                null),

            };
        }

        public ErrorNeutralization (string errorValue, TokenEnum? expectedToken, TokenData tokenData)
        {
            ErrorValue = errorValue;
            ExpectedToken = expectedToken;
            TokenData = tokenData;
            
        }

       
    }
}
