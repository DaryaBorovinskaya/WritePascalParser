using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WritePascalParser.Models
{
    public class RecursiveDescent
    {
        private List<TokenData> _tokens;
        private List<string> _errors;
        private bool _isFinal;

        private int _tokenCurrentIndex;

        private void ConditionWrite(int tokenIndex)
        {
            if (_tokens[tokenIndex + 1].TokenEnum == TokenEnum.OpenBracket)
            {
                _tokenCurrentIndex++;
                ConditionOpenBracket(tokenIndex + 1);
            }
        }

        private void ConditionOpenBracket(int tokenIndex)
        {
            if (_tokens[tokenIndex + 1].TokenEnum == TokenEnum.Arguments)
            {
                _tokenCurrentIndex++;
                ConditionStartArgument(tokenIndex + 1);
            }

            else if (_tokens[tokenIndex + 1].TokenEnum == TokenEnum.CloseBracket)
            {
                _tokenCurrentIndex++;
                ConditionEnd(tokenIndex + 1);
            }
        }

        private void ConditionStartArgument(int tokenIndex)
        {
            if (_tokens[tokenIndex + 1].TokenEnum == TokenEnum.CloseBracket)
            {
                _tokenCurrentIndex++;
                ConditionEnd(tokenIndex + 1);
            }

            else
            {
                ConditionSymbRem(tokenIndex);
            }

            
        }

        private void ConditionSymbRem(int tokenIndex)
        {
            if (_tokens[tokenIndex + 1].TokenEnum == TokenEnum.Comma)
            {
                _tokenCurrentIndex++;
                ConditionArgument(tokenIndex + 1);
            }

            else if (_tokens[tokenIndex + 1].TokenEnum == TokenEnum.CloseBracket)
            {
                _tokenCurrentIndex++;
                ConditionEnd(tokenIndex + 1);
            }
        }

        private void ConditionArgument(int tokenIndex)
        {
            if (_tokens[tokenIndex + 1].TokenEnum == TokenEnum.Arguments)
            {
                _tokenCurrentIndex++;
                ConditionSymbRem(tokenIndex+1);
            }
        }

        private void ConditionEnd(int tokenIndex)
        {
            if (tokenIndex == _tokens.Count - 1 || _tokens[tokenIndex + 1].TokenEnum == TokenEnum.EndLine)
            {
                _errors.Add("Ошибок нет");
                _isFinal = true;
                _tokenCurrentIndex++;
            }
        }

        public RecursiveDescent(List<TokenData> tokens) 
        {
            _tokens = tokens;
            _errors = new List<string>();
            _isFinal = false;
        }

        public List<string> Start()
        {
            _tokenCurrentIndex = 0;
            for (; _tokenCurrentIndex < _tokens.Count; _tokenCurrentIndex++)
            {
                if (!_isFinal)
                {
                    switch (_tokens[_tokenCurrentIndex].TokenEnum)
                    {
                        case TokenEnum.Write:
                            ConditionWrite(_tokenCurrentIndex);
                            break;
                        case TokenEnum.OpenBracket:
                            ConditionOpenBracket(_tokenCurrentIndex);
                            break;
                        case TokenEnum.Arguments:
                            ConditionStartArgument(_tokenCurrentIndex);
                            break;
                        case TokenEnum.Comma:
                            ConditionArgument(_tokenCurrentIndex);
                            break;
                        case TokenEnum.CloseBracket:
                            ConditionEnd(_tokenCurrentIndex);
                            break;
                        case TokenEnum.EndLine:
                            ConditionEnd(_tokenCurrentIndex);
                            break;
                    }
                }

                
            }

            return _errors;
        }

        public string PrintResultRecursiveDescent()
        {
            string result = string.Empty;
            for (int i = 0; i < _errors.Count; i++)
            {
                result += _errors[i] + "\n";
            }

            return result;
        }
    }
}
