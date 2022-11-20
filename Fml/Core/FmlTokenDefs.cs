﻿namespace Fml.Core
{
    internal static class FmlTokenDefs
    {
        public static readonly TokenDefinition[] Tokens =
        {
            new TokenDefinition(@"([""'])(?:\\\1|.)*?\1", TokenKey.Value), // Quoted string
            new TokenDefinition(@"\[", TokenKey.LeftSquareBracket),
            new TokenDefinition(@"\]", TokenKey.RightSquareBracket),
            new TokenDefinition(@"\{", TokenKey.LeftCurlyBrace),
            new TokenDefinition(@"\}", TokenKey.RightSquareBracket),
        };
    }
}
