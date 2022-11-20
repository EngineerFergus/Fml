namespace Fml.Core
{
    internal sealed class Lexer : IDisposable
    {
        private readonly TextReader _reader;
        private readonly TokenDefinition[] _tokenDefinitions;

        private string? _lineRemaining;

        public string? TokenContents { get; private set; }
        public TokenKey Key { get; private set; }
        public int LineNumber { get; private set; }
        public int Position { get; private set; }

        public Lexer(TextReader reader, TokenDefinition[] tokenDefinitions)
        {
            _reader = reader;
            _tokenDefinitions = tokenDefinitions;
            nextLine();
        }

        private void nextLine()
        {
            do
            {
                _lineRemaining = _reader.ReadLine();
                ++LineNumber;
                Position = 0;
            }
            while(_lineRemaining != null && _lineRemaining.Length == 0);
        }

        public bool Next()
        {
            if(_lineRemaining == null)
            {
                return false;
            }

            foreach (var def in _tokenDefinitions)
            {
                var matched = def.Matcher.Match(_lineRemaining);
                if (matched > 0)
                {
                    Position += matched;
                    Key = def.Key;
                    TokenContents = _lineRemaining.Substring(0, matched);
                    _lineRemaining = _lineRemaining.Substring(matched);
                    if (_lineRemaining.Length == 0)
                    {
                        nextLine();
                    }

                    return true;
                }
            }

            throw new Exception($"Unable to match against any tokens at line {LineNumber} position {Position}, \"{_lineRemaining}\"");
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
