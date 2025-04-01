using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WritePascalParser.Models
{
    public class Parser
    {
        private string _inputData;
        private string _copyInputData;
        private TokenConverter _tokenConverter;
        private List<TokenData> _tokens;
        private List<string> _errors;
        private List<string> _inputLines;
        private RecursiveDescent _recursiveDescent;
        public string InputData 
        { 
            get { return _inputData;}
            set { _inputData = value; }
        }

        public string CopyInputData
        {
            get { return _copyInputData; }
            set { _copyInputData = value; }
        }

        /// <summary>
        /// Подготовительный этап перед парсингом
        /// </summary>
        private void PreparatoryProcessing()
        {
            _inputData = _inputData.ToLower();
            _inputLines = CopyInputData.Split('\n').ToList();
        }

        private string PrintTokens()
        {
            string outputData = string.Empty;
            _tokens.ForEach(token =>
            {
                outputData += token.PrintTokenValue();
            });
            return outputData;
        }

        private string PrintErrors()
        {
            string outputData = string.Empty;
            _errors.ForEach(error =>
            {
                outputData += error;
            });
            return outputData;
        }

        

        private void CheckArguments()
        {
            string forbiddenChars = "+-!№#$%^&?*()<>[]:;@\\,\"\t\n\r";
            string tempTokenValue = string.Empty;

            List<string> errors = new();
            List<string> tempErrors = new();
            List<int> invalidTokenIndexes = new();
            bool isInvalidToken = true;
            
            for (int i = 0; i< _tokens.Count; i++)
            {
                isInvalidToken = true;
                tempErrors.Clear();

                // Перебираем все токены, которые могут быть невалидными
                if (_tokens[i].TokenEnum == TokenEnum.Arguments)
                {
                    tempTokenValue = _tokens[i].TokenValue;
                    for (int j = 0; j < tempTokenValue.Length; j++)
                    {
                        char item = tempTokenValue[j];
                        if (forbiddenChars.Contains(item)
                            || (j == 0 && char.IsDigit(item)))
                        {
                            tempErrors.Add($"Строка {_tokens[i].LineNumber} знак {_tokens[i].LineOffset + j} ОШИБКА: некорректный символ {item} в слове {tempTokenValue}" + "\n");
                            invalidTokenIndexes.Add(i);
                            isInvalidToken = false;
                        }
                        else
                        {
                            _tokens[i].TokenNewValue += item;
                        }
                    }

                    // Если аргумент изначально верно написан
                    if (_tokens[i].TokenValue == _tokens[i].TokenNewValue)
                    {
                        _tokens[i].TokenNewValue = string.Empty;
                    }

                    // Если аргумент был написан неверно
                    if (!isInvalidToken)
                    {
                        _tokenConverter.ChangeTokenEnum(_tokens[i]);
                    }

                    // В исходной подстроке были все символы невалидные
                    if (_tokens[i].TokenEnum == TokenEnum.None)
                    {
                        errors.Add($"Строка {_tokens[i].LineNumber} знак {_tokens[i].LineOffset} ОШИБКА: некорректное слово {tempTokenValue}" + "\n");
                    }
                    else
                    {
                        errors.AddRange(tempErrors);
                    }
                }
            }

            invalidTokenIndexes = new HashSet<int>(invalidTokenIndexes).ToList();

            List<TokenData> validTokens = new List<TokenData>();

            // Если в исходной строке есть ошибки
            if (invalidTokenIndexes.Count != 0)
            {
                for (int i = 0; i < _tokens.Count; i++)
                {
                    for (int j = 0; j < invalidTokenIndexes.Count; j++)
                    {
                        if (_tokens[i].TokenEnum != TokenEnum.None)
                            validTokens.Add(_tokens[i]);

                        if (i == invalidTokenIndexes[j])
                            break;
                    }
                }
                _tokens = new HashSet<TokenData>(validTokens).ToList();
                
            }
            _errors = errors;


        }

        public Parser(string inputData) 
        {
            InputData = inputData;
            CopyInputData = inputData;
        }


        public string StartParse()
        {
            PreparatoryProcessing();

            _tokenConverter = new TokenConverter(InputData);
            _tokens = _tokenConverter.CreateTokens();

            string printTokens = PrintTokens();

            _tokens = _tokenConverter.ClearTokens(_tokens);
            CheckArguments();
            

            _recursiveDescent = new(_tokens, _errors);
            string errors = string.Empty;
            if (_tokens.Count != 0)
            {
                _recursiveDescent.Start();
                errors = _recursiveDescent.PrintResultRecursiveDescent();
            }
            else
            {
                errors = PrintErrors();
            }
            

            return errors;
        }
    }
}
