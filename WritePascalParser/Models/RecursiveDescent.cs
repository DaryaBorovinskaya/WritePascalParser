using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WritePascalParser.Models
{
    public enum TokenCondition
    {
        Write,
        OpenBracket,
        StartArgument,
        SymbRem,
        Argument,
        End
    }


    public class RecursiveDescent
    {
        private List<TokenData> _tokens;
        private List<string> _errors;
        private bool _isFinal;

        private int _tokenCurrentIndex;

        
        private void CallCondition(TokenCondition condition, int tokenIndex)
        {
            switch(condition)
            {
                case TokenCondition.Write:
                    ConditionWrite(tokenIndex);
                    break;
                case TokenCondition.OpenBracket:
                    ConditionOpenBracket(tokenIndex);
                    break;
                case TokenCondition.StartArgument: 
                    ConditionStartArgument(tokenIndex);
                    break;
                case TokenCondition.SymbRem:
                    ConditionSymbRem(tokenIndex);
                    break;
                case TokenCondition.Argument:
                    ConditionArgument(tokenIndex);
                    break;
                case TokenCondition.End:
                    ConditionEnd(tokenIndex);
                    break;
            }
        }
        private void ConditionWrite(int tokenIndex)
        {
            // Верный вариант
            if (_tokens[tokenIndex].TokenEnum == TokenEnum.Write)
            {
                _tokenCurrentIndex++;
                ConditionOpenBracket(tokenIndex + 1);
            }

            // Неверный вариант - текущий токен не ключевое слово write
            else
            {
                _errors.Add($"ОШИБКА: ожидалось ключевое слово write, получено: \"{_tokens[tokenIndex].TokenValue}\"");
                
                if (_tokens.Count < 4)
                {
                    // PUSH - вызов следующего метода для текущего индекса
                    CallCondition(TokenCondition.OpenBracket, tokenIndex);
                }
                else
                {
                    // REPLACE - вызов следующего метода для следующего индекса
                    CallCondition(TokenCondition.OpenBracket, tokenIndex+1);
                }
            }
        }

        private void ConditionOpenBracket(int tokenIndex)
        {
            // Верные варианты
            if (_tokens[tokenIndex].TokenEnum == TokenEnum.OpenBracket)
            {
                // 1. Если есть аргументы у функции write
                if (_tokens[tokenIndex + 1].TokenEnum == TokenEnum.Arguments)
                {
                    _tokenCurrentIndex++;
                    ConditionStartArgument(tokenIndex + 1);
                }

                // 2. Если нет аргументов у функции write
                else if (_tokens[tokenIndex + 1].TokenEnum == TokenEnum.CloseBracket)
                {
                    _tokenCurrentIndex++;
                    ConditionEnd(tokenIndex + 1);
                }
            }
                

            // Неверные варианты - текущий токен не символ "("
            else
            {
                _errors.Add($"ОШИБКА: ожидался символ \"(\", получено: \"{_tokens[tokenIndex].TokenValue}\"");
                // 1. Если есть аргументы у функции write
                if (_tokens[tokenIndex].TokenEnum == TokenEnum.Arguments)
                {
                    CallCondition(TokenCondition.StartArgument, tokenIndex+1);
                }

                // 2. Если нет аргументов у функции write
                else if (_tokens[tokenIndex].TokenEnum == TokenEnum.CloseBracket)
                {
                    CallCondition(TokenCondition.End, tokenIndex+1);
                }

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
            // Верный вариант
            if (_errors.Count == 0 && (tokenIndex == _tokens.Count - 1 && 
                _tokens[tokenIndex].TokenEnum == TokenEnum.EndLine
                || tokenIndex == _tokens.Count - 2 &&
                _tokens[tokenIndex+1].TokenEnum == TokenEnum.EndLine))
            {
                _errors.Add("Ошибок нет");
                _isFinal = true;
                _tokenCurrentIndex++;
            }

            else
            {
                if (tokenIndex == _tokens.Count-1 && _tokens[tokenIndex].TokenEnum != TokenEnum.EndLine
                    ||
                     (tokenIndex == _tokens.Count - 2 && _tokens[tokenIndex + 1].TokenEnum != TokenEnum.EndLine))
                {
                    _errors.Add($"ОШИБКА: ожидался символ \";\"");
                }

                else if (tokenIndex < _tokens.Count-1 && _tokens[tokenIndex + 1].TokenEnum != TokenEnum.EndLine)
                {
                    _errors.Add($"ОШИБКА: ожидался символ \";\", получено: {_tokens[tokenIndex + 1].TokenValue}");
                    _tokenCurrentIndex++;
                    CallCondition(TokenCondition.End, tokenIndex+1);
                }

            }
        }

        public RecursiveDescent(List<TokenData> tokens, List<string> errors) 
        {
            _tokens = tokens;
            _errors = errors;
            _isFinal = false;
        }

        public List<string> Start()
        {
            _tokenCurrentIndex = 0;
            ConditionWrite(_tokenCurrentIndex);
            /*for (; _tokenCurrentIndex < _tokens.Count; _tokenCurrentIndex++)
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
            */
            return _errors;
        }

        public string PrintResultRecursiveDescent()
        {
            string result = string.Empty;
            if (_errors != null)
            {
                for (int i = 0; i < _errors.Count; i++)
                {
                    result += _errors[i] + "\n";
                }
            }
            

            return result;
        }
    }
}
