using Fml.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fml
{
    public class Tokenizer
    {
        private Lexer? _lexer;

        public Tokenizer()
        {

        }

        public FmlToken[] Tokenize(TextReader reader)
        {
            _lexer = new Lexer(reader, FmlTokenDefs.Tokens);

            List<FmlToken> tokens = new List<FmlToken>();

            while (_lexer.Next())
            {
                if (_lexer.TokenContents != null && _lexer.TokenContents != string.Empty)
                {
                    FmlToken token = new FmlToken(_lexer.Key, _lexer.TokenContents, tokens.Count);
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
