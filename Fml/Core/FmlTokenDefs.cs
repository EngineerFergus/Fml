namespace Fml.Core
{
    internal static class FmlTokenDefs
    {
        public static readonly TokenDefinition[] Tokens =
        {
            new TokenDefinition(@"\[", TokenKey.LeftSquareBracket),
            new TokenDefinition(@"\]", TokenKey.RightSquareBracket),
            new TokenDefinition(@"\{", TokenKey.LeftCurlyBrace),
            new TokenDefinition(@"\}", TokenKey.RightCurlyBrace),
            new TokenDefinition(@"\=", TokenKey.Equals),
            new TokenDefinition(@"\ ", TokenKey.Space),
            new TokenDefinition(@"\,", TokenKey.Comma),
            new TokenDefinition(@"/[\r\n]+/", TokenKey.NewLine),
            new TokenDefinition(@"\#", TokenKey.Pound),
            new TokenDefinition(@"([""'])(?:\\\1|.)*?\1", TokenKey.Value), // Quoted string
            new TokenDefinition(@"[^\[\]\{\}\=\ \,\#]+", TokenKey.Value), // Unquoted string without reserved characters
        };
    }
}
