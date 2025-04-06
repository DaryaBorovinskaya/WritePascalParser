using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WritePascalParser.Models
{
    public class TokenConverter
    {
        private string _inputData;
        private Regex _regex; 
        public string InputData
        {
            get { return _inputData; }
            set { _inputData = value; }
        }

        public TokenConverter(string inputData)
        {
            InputData = inputData;
            _regex = new(@"write|((?!(write))[^\s();,])+|\(|\)|;|,|\s");
        }

        public List<TokenData> CreateTokens()
        {
            List<TokenData> tokens = new();
            int tempLineNumber = 1;
            int tempLineOffset = 1;

            MatchCollection matches = _regex.Matches(_inputData);
            if (matches.Count > 0)
                foreach (Match match in matches)
                {
                    switch (match.Value)
                    {
                        case "write":
                            tokens.Add(new(TokenEnum.Write, match.Value, tempLineNumber, tempLineOffset));
                            tempLineOffset += match.Value.Length;
                            break;
                        case "(":
                            tokens.Add(new(TokenEnum.OpenBracket, match.Value, tempLineNumber, tempLineOffset));
                            tempLineOffset += match.Value.Length;
                            break;
                        case ")":
                            tokens.Add(new(TokenEnum.CloseBracket, match.Value, tempLineNumber, tempLineOffset));
                            tempLineOffset += match.Value.Length;
                            break;
                        case ";":
                            tokens.Add(new(TokenEnum.EndLine, match.Value, tempLineNumber, tempLineOffset));
                            tempLineOffset += match.Value.Length;
                            break;
                        case ",":
                            tokens.Add(new(TokenEnum.Comma, match.Value, tempLineNumber, tempLineOffset));
                            tempLineOffset += match.Value.Length;
                            break;
                        case " ":
                            tokens.Add(new(TokenEnum.None, match.Value, tempLineNumber, tempLineOffset));
                            tempLineOffset += match.Value.Length;
                            break;
                        case "\n":
                            tempLineNumber += 1;
                            tempLineOffset = 1;
                            break;
                        case "\r":
                            break;
                        default:
                            tokens.Add(new(TokenEnum.Arguments, match.Value, tempLineNumber, tempLineOffset));
                            tempLineOffset += match.Value.Length;
                            break;
                    }
                }

            
            return tokens;

        }

        public void ChangeTokenEnum(TokenData tokenData)
        {
            MatchCollection matches = _regex.Matches(tokenData.TokenNewValue);
            if (matches.Count > 0)
                foreach (Match match in matches)
                {
                    switch (match.Value)
                    {
                        case "write":
                            tokenData.TokenEnum = TokenEnum.Write;
                            break;
                        case "(":
                            tokenData.TokenEnum = TokenEnum.OpenBracket;
                            break;
                        case ")":
                            tokenData.TokenEnum = TokenEnum.CloseBracket;
                            break;
                        case ";":
                            tokenData.TokenEnum = TokenEnum.EndLine;
                            break;
                        case ",":
                            tokenData.TokenEnum = TokenEnum.Comma;
                            break;
                        default:
                            tokenData.TokenEnum = TokenEnum.Arguments;
                            break;
                    }
                }
            
            // В исходной подстроке были все символы невалидные
            else if (tokenData.TokenNewValue.Length == 0)
            {
                tokenData.TokenEnum = TokenEnum.None;
            }
        }

        /// <summary>
        /// Удаление токенов, у которых TokenEnum равен None
        /// </summary>
        public List<TokenData> ClearTokens(List<TokenData> tokens)
        {
            List<TokenData> newTokens = new();

            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].TokenEnum != TokenEnum.None)
                    newTokens.Add(tokens[i]);
            }

            return newTokens;
        }

    }
}
