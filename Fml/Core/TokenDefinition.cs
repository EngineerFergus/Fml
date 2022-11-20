namespace Fml.Core
{
    public sealed class TokenDefinition
    {
        public readonly IMatcher Matcher;
        public readonly TokenKey Key;

        public TokenDefinition(string regex, TokenKey key)
        {
            Matcher = new RegexMatcher(regex);
            Key = key;
        }
    }
}
