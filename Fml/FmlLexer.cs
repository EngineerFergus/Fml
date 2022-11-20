using Fml.Core;

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

            return _lexer.Tokenize();
        }
    }
}
