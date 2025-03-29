using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WritePascalParser.Models
{
    public class Parser
    {
        private string _inputData;
        private TokenConverter _tokenConverter;
        private List<TokenData> _tokens;
        private RecursiveDescent _recursiveDescent;
        public string InputData 
        { 
            get { return _inputData;}
            set { _inputData = value; }
        }

        /// <summary>
        /// Удаление незначащих пробелов
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

        private string CheckArguments()
        {
            string forbiddenChars = " +-!№#$%^&?*()<>[]:;@\\,\"\t\n\r";
            string tempTokenValue = string.Empty;

            string errors = string.Empty;

            _tokens.Where((token)=> token.TokenEnum == TokenEnum.Arguments).ToList().ForEach((token) =>
            {
                tempTokenValue = token.TokenValue;
                for (int i = 0; i< tempTokenValue.Length;i++)
                {
                    char item = tempTokenValue[i];
                    if (forbiddenChars.Contains(item) || (i == 0 && char.IsDigit(item)))
                    {
                        errors += $"Некорректный символ {item} в слове {tempTokenValue}" + "\n";
                    }
                }
                
            });

            if (errors == string.Empty)
            {
                errors = "Верно";
            }

            return errors;
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

            string checkArguments = CheckArguments();
            string printTokens = PrintTokens();

            _recursiveDescent = new(_tokens);
            _recursiveDescent.Start();

            return printTokens;
        }
    }
}
