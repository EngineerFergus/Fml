using Fml.Core;

namespace Fml.Scanner
{
    internal sealed class Scanner
    {
        private readonly TextReader _reader;
        private readonly TokenDefinition[] _tokenDefinitions;

        private string? _lineRemaining;

        public string? TokenContents { get; private set; }
        public TokenKey Key { get; private set; }
        public int LineNumber { get; private set; }
        public int Position { get; private set; }

        public Scanner(TextReader reader, TokenDefinition[] tokenDefinitions)
        {
            _reader = reader;
            _tokenDefinitions = tokenDefinitions;
            LineNumber = -1;
            NextLine();
        }

        private void NextLine()
        {
            do
            {
                _lineRemaining = _reader.ReadLine();
                ++LineNumber;
                Position = 0;
            }
            while (_lineRemaining != null && _lineRemaining.Length == 0);
        }

        public FmlToken[] Tokenize()
        {
            List<FmlToken> tokens = new();
            int pastLine = LineNumber;

            while (TryParseToken(out FmlToken? token, tokens.Count))
            {
                if (token == null)
                {
                    throw new Exception();
                }

                if (token.Line > pastLine)
                {
                    FmlToken? lastToken = tokens.LastOrDefault();
                    int endPosition = lastToken != null ? lastToken.Position + lastToken.Contents.Length : 0;
                    FmlToken newLineToken = new(TokenKey.NewLine, "\n", tokens.Count, pastLine, endPosition);
                    tokens.Add(newLineToken);
                    token = token.MakeWithIndex(tokens.Count);
                    pastLine = token.Line;
                }

                tokens.Add(token);
            }

            return tokens.ToArray();
        }

        private bool TryParseToken(out FmlToken? token, int index)
        {
            token = null;

            if (_lineRemaining == null)
            {
                return false;
            }

            foreach (var def in _tokenDefinitions)
            {
                int matched = def.Matcher.Match(_lineRemaining);

                if (matched > 0)
                {
                    string contents = _lineRemaining.Substring(0, matched);
                    token = new FmlToken(def.Key, contents, index, LineNumber, Position);

                    Position += matched;
                    _lineRemaining = _lineRemaining.Substring(matched);


                    if (_lineRemaining.Length == 0)
                    {
                        NextLine();
                    }

                    return true;
                }
            }

            throw new Exception($"Unable to match against any tokens at line {LineNumber} position {Position}, \"{_lineRemaining}\"");
        }

        public bool TryParseToken()
        {
            if (_lineRemaining == null)
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
                        NextLine();
                    }

                    return true;
                }
            }

            throw new Exception($"Unable to match against any tokens at line {LineNumber} position {Position}, \"{_lineRemaining}\"");
        }
    }
}
