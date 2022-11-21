using Fml.Core;

namespace Fml.Scanner
{
    public class FmlScanner
    {
        private Scanner? _lexer;

        public FmlScanner()
        {

        }

        public FmlToken[] Tokenize(TextReader reader)
        {
            _lexer = new Scanner(reader, FmlTokenDefs.Tokens);

            return _lexer.Tokenize();
        }
    }
}
