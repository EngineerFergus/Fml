using Fml.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fml
{
    public class FmlLexer
    {
        private Lexer? _lexer;

        public FmlLexer()
        {

        }

        public FmlToken[] Tokenize(TextReader reader)
        {
            _lexer = new Lexer(reader, FmlTokenDefs.Tokens);

            List<FmlToken> tokens = new();
            int pastLine = 2;

            while (_lexer.Next())
            {
                if (_lexer.TokenContents != null && _lexer.TokenContents != string.Empty)
                {
                    if (pastLine < _lexer.LineNumber)
                    {
                        FmlToken newLinetoken = new(TokenKey.NewLine, "\n", tokens.Count, _lexer.LineNumber,
                            _lexer.Position + _lexer.TokenContents.Length);
                        tokens.Add(newLinetoken);
                        pastLine = _lexer.LineNumber;
                    }

                    FmlToken token = new(_lexer.Key, _lexer.TokenContents, tokens.Count, _lexer.LineNumber, _lexer.Position);
                    tokens.Add(token);
                }
                else
                {
                    throw new Exception("Retrieved null or empty token contents!");
                }
            }

            return tokens.ToArray();
        }
    }
}
