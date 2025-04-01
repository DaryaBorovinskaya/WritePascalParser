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
        private TokenConverter _tokenConverter;
        private List<TokenData> _tokens;
        private List<string> _errors;
        private RecursiveDescent _recursiveDescent;
        public string InputData 
        { 
            get { return _inputData;}
            set { _inputData = value; }
        }

        /// <summary>
        /// Подготовительный этап перед парсингом
        /// </summary>
        private void PreparatoryProcessing()
        {
            _inputData = _inputData.ToLower();

            /*string newInputData = string.Empty;

            for(int i=0; i < _inputData.Length;i++)
            {
                if (!char.IsWhiteSpace(_inputData[i]))
                    newInputData += _inputData[i];
            }
            _inputData = newInputData;*/
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

        private void CheckArguments()
        {
            string forbiddenChars = " +-!№#$%^&?*()<>[]:;@\\,\"\t\n\r";
            string tempTokenValue = string.Empty;

            List<string> errors = new();

            
            List<int> invalidTokenIndexes = new();

            
            for (int i = 0; i< _tokens.Count; i++)
            {
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
                            errors.Add($"ОШИБКА: некорректный символ {item} в слове {tempTokenValue}" + "\n");
                            invalidTokenIndexes.Add(i);
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
                }
            }

            invalidTokenIndexes = new HashSet<int>(invalidTokenIndexes).ToList();

            List<TokenData> validTokens = new List<TokenData>();

            for (int i = 0; i < _tokens.Count; i++)
            {
                for (int j = 0; j < invalidTokenIndexes.Count; j++)
                {
                    if (i == invalidTokenIndexes[j])
                    {
                        _tokenConverter.ChangeTokenEnum(_tokens[i]);
                        if (_tokens[i].TokenEnum != TokenEnum.None) 
                            validTokens.Add(_tokens[i]);
                        break;
                    }
                    
                    validTokens.Add(_tokens[i]);
                    
                }
            }

            _tokens = new HashSet<TokenData>( validTokens).ToList();
            _errors = errors;

        }

        public Parser(string inputData) 
        {
            InputData = inputData;
        }


        public string StartParse()
        {
            PreparatoryProcessing();

            _tokenConverter = new TokenConverter(InputData);
            _tokens = _tokenConverter.CreateTokens();

            string printTokens = PrintTokens();

            CheckArguments();
            

            _recursiveDescent = new(_tokens, _errors);
           _recursiveDescent.Start();
            string errors = _recursiveDescent.PrintResultRecursiveDescent();

            return errors;
        }
    }
}
