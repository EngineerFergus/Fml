namespace Fml.Core
{
    public class FmlToken
    {
        public TokenKey Key { get; }
        public string Contents { get; }
        public int Idx { get; }
        public int Line { get; }
        public int Position { get; }

        public FmlToken(TokenKey key, string contents, int idx, int line, int position)
        {
            Key = key;
            Contents = contents;
            Idx = idx;
            Line = line;
            Position = position;
        }   
    }
}
