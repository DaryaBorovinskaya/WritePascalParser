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
        private List<ErrorNeutralization> _errors;
        private bool _isFinal;
        private List<TokenData> _deleteTokens;
        private bool _isEmptyTokens;

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
                _errors.Add(new(
                    $"Строка {_tokens[tokenIndex].LineNumber} знак {_tokens[tokenIndex].LineOffset} " +
                    $"ОШИБКА: ожидалось ключевое слово write, получено: \"{_tokens[tokenIndex].TokenValue}\"", 
                    TokenEnum.Write, 
                    _tokens[tokenIndex]
                ));

                _deleteTokens.Add(_tokens[tokenIndex]);

                if (tokenIndex < _tokens.Count - 1)
                {
                    // DELETE - вызов текущего метода для следующего индекса
                    CallCondition(TokenCondition.Write, tokenIndex + 1);
                }

                // Прошли все токены и при этом не встретилось ключ. слово write
                else
                {
                    // Подготовительный этап перед заменой или добавлением
                    tokenIndex -= _deleteTokens.Count - 1;
                    _errors.RemoveRange(_errors.Count - (_deleteTokens.Count - 1), _deleteTokens.Count-1);




                    // Не очень работающий вариант
                    // Отсутствует токен перед символом "("
                    if (_deleteTokens[0].TokenEnum >= TokenEnum.OpenBracket
                        && _deleteTokens[0].TokenEnum != TokenEnum.Arguments)
                    {
                        _deleteTokens.Clear();
                        // PUSH - вызов следующего метода для текущего индекса
                        CallCondition(TokenCondition.OpenBracket, tokenIndex);
                    }

                    // Вместо ключ. слово write написано что-то иное (Token Arguments)
                    else
                    {
                        _deleteTokens.Clear();
                        // REPLACE - вызов следующего метода для следующего индекса
                        CallCondition(TokenCondition.OpenBracket, tokenIndex + 1);
                    }
                    
                }
                
                
                /*if (_tokens.Count < 4)
                {
                    // PUSH - вызов следующего метода для текущего индекса
                    CallCondition(TokenCondition.OpenBracket, tokenIndex);
                }
                else
                {
                    // REPLACE - вызов следующего метода для следующего индекса
                    CallCondition(TokenCondition.OpenBracket, tokenIndex+1);
                }*/
            }
        }

        private void ConditionOpenBracket(int tokenIndex)
        {
            // Возможно верные варианты
            if (_tokens[tokenIndex].TokenEnum == TokenEnum.OpenBracket)
            {
                if (tokenIndex < _tokens.Count - 1)
                {
                    // Верные варианты

                    // 1. Если есть аргументы у функции write
                    if (_tokens[tokenIndex + 1].TokenEnum == TokenEnum.Arguments 
                        || _tokens[tokenIndex + 1].TokenEnum == TokenEnum.Comma)
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

                    // Пока здесь, но как будто надо бы перенести куда-нибудь

                    // Неверный вариант - следующий токен это символ ";"
                    else if (_tokens[tokenIndex + 1].TokenEnum == TokenEnum.EndLine)
                    {
                        _errors.Add(new(
                        $"Строка {_tokens[tokenIndex+1].LineNumber} знак {_tokens[tokenIndex+1].LineOffset} " +
                        $"ОШИБКА: ожидался символ \")\", получено: \"{_tokens[tokenIndex+1].TokenValue}\"",
                        TokenEnum.OpenBracket,
                        _tokens[tokenIndex+1]
                        ));
                        // PUSH - вызов следующего метода для текущего индекса
                        CallCondition(TokenCondition.End, tokenIndex);
                    }
                }

                
            }
                

            // Неверные варианты - текущий токен не символ "("
            else
            {
                _errors.Add(new(
                    $"Строка {_tokens[tokenIndex].LineNumber} знак {_tokens[tokenIndex].LineOffset} " +
                    $"ОШИБКА: ожидался символ \"(\", получено: \"{_tokens[tokenIndex].TokenValue}\"",
                    TokenEnum.OpenBracket, 
                    _tokens[tokenIndex]
                ));

                _deleteTokens.Add(_tokens[tokenIndex]);

                if (tokenIndex < _tokens.Count - 1)
                {
                    // DELETE - вызов текущего метода для следующего индекса
                    CallCondition(TokenCondition.OpenBracket, tokenIndex + 1);
                }
                // Прошли все токены и при этом не встретился символ "("
                else
                {
                    tokenIndex -= _deleteTokens.Count - 1;
                    _errors.RemoveRange(_errors.Count - (_deleteTokens.Count - 1), _deleteTokens.Count - 1);


                    // Отсутствует токен перед символом ")" или перед аргументами
                    if (_deleteTokens[0].TokenEnum >= TokenEnum.Arguments)
                    {

                        

                        // 1. Если есть аргументы у функции write
                        if (_tokens[tokenIndex].TokenEnum == TokenEnum.Arguments)
                        {
                            // REPLACE - вызов следующего метода для следующего индекса
                            CallCondition(TokenCondition.StartArgument, tokenIndex+1);
                        }

                        // 2. Если нет аргументов у функции write
                        else if (_tokens[tokenIndex].TokenEnum == TokenEnum.CloseBracket)
                        {
                            // PUSH - вызов следующего метода для текущего индекса
                            CallCondition(TokenCondition.End, tokenIndex);
                        }

                    }

                    //// Вместо ключ. слово write написано что-то иное (Token Arguments)
                    //else
                    //{
                    //    // REPLACE - вызов следующего метода для следующего индекса
                        
                    //    // 1. Если есть аргументы у функции write
                    //    if (_tokens[tokenIndex+1].TokenEnum == TokenEnum.Arguments)
                    //    {
                    //        CallCondition(TokenCondition.StartArgument, tokenIndex + 1);
                    //    }

                    //    // 2. Если нет аргументов у функции write
                    //    else if (_tokens[tokenIndex+1].TokenEnum == TokenEnum.CloseBracket)
                    //    {
                    //        CallCondition(TokenCondition.End, tokenIndex + 1);
                    //    }
                    //}




                    
                }

            }
        }

        private void ConditionStartArgument(int tokenIndex)
        {
            if (_tokens[tokenIndex].TokenEnum == TokenEnum.Comma &&
                _tokens[tokenIndex + 1].TokenEnum == TokenEnum.CloseBracket)
            {
                _errors.Add(new(
                $"Строка {_tokens[tokenIndex].LineNumber} знак {_tokens[tokenIndex].LineOffset} " +
                $"ОШИБКА: ожидался(лись) аргумент(ы) функции, получено: \"{_tokens[tokenIndex].TokenValue}\"",
                TokenEnum.Arguments,
                _tokens[tokenIndex]
                ));
                _tokenCurrentIndex++;
                // REPLACE - вызов следующего метода для следующего индекса
                ConditionEnd(tokenIndex + 1);
            }
            

            else if (_tokens[tokenIndex + 1].TokenEnum == TokenEnum.CloseBracket)
            {
                _tokenCurrentIndex++;
                // Верный вариант - только один аргумент
                ConditionEnd(tokenIndex + 1);

            }

            else
            {
                ConditionSymbRem(tokenIndex);
            }

            
        }

        private void ConditionSymbRem(int tokenIndex)
        {
            // Верные варианты

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

            // Неверный вариант - нет запятой между аргументами
            else if (_tokens[tokenIndex + 1].TokenEnum == TokenEnum.Arguments)
            {
                _tokenCurrentIndex++;
                tokenIndex++;

                // Определяем место, куда можно было бы добавить запятую 
                int spaceLineOffset = _tokens[tokenIndex].LineOffset - 1;
                _errors.Add(new(
                    $"Строка {_tokens[tokenIndex].LineNumber} знак {spaceLineOffset} " +
                    $"ОШИБКА: ожидался  символ \",\" между аргументами {_tokens[tokenIndex-1].TokenValue} и {_tokens[tokenIndex].TokenValue}",
                    TokenEnum.Comma,
                    _tokens[tokenIndex]
                ));
                // PUSH - вызов следующего метода для текущего индекса
                CallCondition(TokenCondition.Argument, tokenIndex);
            }
            else if (_tokens[tokenIndex + 1].TokenEnum == TokenEnum.EndLine)
            {
                tokenIndex++;
                _errors.Add(new(
                    $"Строка {_tokens[tokenIndex].LineNumber} знак {_tokens[tokenIndex].LineOffset} " +
                    $"ОШИБКА: ожидался  символ \")\", получено \"{_tokens[tokenIndex].TokenValue}\"",
                    TokenEnum.CloseBracket,
                    _tokens[tokenIndex]
                ));
            }
        }

        private void ConditionArgument(int tokenIndex)
        {
            if (_tokens[tokenIndex + 1].TokenEnum == TokenEnum.Arguments
                && _tokens[tokenIndex].TokenEnum == TokenEnum.Comma)
            {
                _tokenCurrentIndex++;
                ConditionSymbRem(tokenIndex+1);
            }

            // Если между вторым и третьим (и последующими аргументами) нет символа ","
            else if (_tokens[tokenIndex].TokenEnum != TokenEnum.Comma)
            {
                ConditionSymbRem(tokenIndex);
            }
        }

        private void ConditionEnd(int tokenIndex)
        {
            // Верные варианты
            if (_errors.Count == 0 && tokenIndex == _tokens.Count - 1 &&
                _tokens[tokenIndex].TokenEnum == TokenEnum.EndLine)
            {

                _errors.Add(new("Ошибок нет", TokenEnum.EndLine, _tokens[tokenIndex]));
                _isFinal = true;
                _tokenCurrentIndex++;
            }

            else if (_errors.Count == 0 && tokenIndex == _tokens.Count - 2 &&
            _tokens[tokenIndex + 1].TokenEnum == TokenEnum.EndLine)
            {
                _errors.Add(new("Ошибок нет", TokenEnum.EndLine, _tokens[tokenIndex + 1]));
                _isFinal = true;
                _tokenCurrentIndex++;
            }


            else
            {
                // На месте последнего токена не символ ";"
                if (tokenIndex == _tokens.Count-1 && _tokens[tokenIndex].TokenEnum != TokenEnum.EndLine)
                {
                    _errors.Add(new(
                        $"Строка {_tokens[tokenIndex].LineNumber} знак {_tokens[tokenIndex].LineOffset+1} " +
                        $"ОШИБКА: ожидался символ \";\"",
                        TokenEnum.EndLine,
                        _tokens[tokenIndex]
                        ));
                }
                else if (tokenIndex == _tokens.Count - 2 && _tokens[tokenIndex + 1].TokenEnum != TokenEnum.EndLine)
                {
                    _errors.Add(new(
                        $"Строка {_tokens[tokenIndex+1].LineNumber} знак {_tokens[tokenIndex+1].LineOffset} " +
                        $"ОШИБКА: ожидался символ \";\"",
                        TokenEnum.EndLine,
                        _tokens[tokenIndex + 1]
                        ));
                }

                else if (tokenIndex < _tokens.Count-1 && _tokens[tokenIndex + 1].TokenEnum != TokenEnum.EndLine)
                {
                    _errors.Add(new(
                        $"Строка {_tokens[tokenIndex+1].LineNumber} знак {_tokens[tokenIndex+1].LineOffset} " +
                        $"ОШИБКА: ожидался символ \";\", получено: {_tokens[tokenIndex + 1].TokenValue}",
                        TokenEnum.EndLine, 
                        _tokens[tokenIndex + 1]
                    ));
                    _tokenCurrentIndex++;
                    CallCondition(TokenCondition.End, tokenIndex+1);
                }

               
            }
        }

        private List<ErrorNeutralization> ToNeutralizationFromLexer(List<ErrorLexer> errors)
        {
            List<ErrorNeutralization> errorNeutralizations = new();
            errors.ForEach((e) =>
            {
                errorNeutralizations.Add(new(e.ErrorValue, null, e.TokenData));
            });
            return errorNeutralizations;
        }

        private bool CheckEmptyTokens()
        {
            if (_tokens.Count == 0)
            {
                return true;
            }
            return false;
        }

        private void ErrorsEmptyTokens()
        {
            _errors.Add(new(
                $"Строка {1} знак {1} " +
                $"ОШИБКА: ожидалось ключевое слово write",
                TokenEnum.Write,
                null
            ));

            _errors.Add(new(
                $"Строка {1} знак {1} " +
                $"ОШИБКА: ожидался символ \"(\"",
                TokenEnum.OpenBracket,
                null
            ));

            _errors.Add(new(
                $"Строка {1} знак {1} " +
                $"ОШИБКА: ожидался символ \")\"",
                TokenEnum.OpenBracket,
                null
            ));
            _errors.Add(new(
                $"Строка {1} знак {1} " +
                $"ОШИБКА: ожидался символ \";\"",
                TokenEnum.OpenBracket,
                null
            ));
        }

        public RecursiveDescent(List<TokenData> tokens, List<ErrorLexer> errors) 
        {
            _tokens = tokens;
            _errors = new();
            _errors.AddRange(ToNeutralizationFromLexer(errors));
            _isFinal = false;
            _deleteTokens = new();

            if (CheckEmptyTokens())
            {
                _isEmptyTokens = true;
                ErrorsEmptyTokens();
            }
            else
            {
                _isEmptyTokens = false;
            }
        }

        public void Start()
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
            
        }

        public string PrintResultRecursiveDescent()
        {
            string result = string.Empty;
            if (!_isEmptyTokens)
            {
                _errors = _errors.OrderBy(e => e.TokenData.LineNumber).ToList().OrderBy(e => e.TokenData.LineOffset).ToList();
            }

            if (_errors != null)
            {
                for (int i = 0; i < _errors.Count; i++)
                {
                    result += _errors[i].ErrorValue + "\n";
                }
            }
            return result;
        }
    }
}
