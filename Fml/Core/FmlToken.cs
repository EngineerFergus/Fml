namespace Fml.Core
{
    public class FmlToken
    {
        public TokenKey Key { get; }
        public string Contents { get; }
        public int Idx { get; }

        public FmlToken(TokenKey key, string contents, int idx)
        {
            Key = key;
            Contents = contents;
            Idx = idx;
        }   
    }
}
