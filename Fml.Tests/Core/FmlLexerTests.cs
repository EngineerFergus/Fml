using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fml;
using Fml.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fml.Tests.Core
{
    [TestClass]
    public class FmlLexerTests
    {
        private readonly FmlLexer _lexer = new();

        private FmlToken[] GetTokens(string input)
        {
            using TextReader reader = new StringReader(input);
            return _lexer.Tokenize(reader);
        }

        [DataTestMethod]
        [DataRow("[]", 2)]
        [DataRow("{}", 2)]
        [DataRow("name", 1)]
        [DataRow("\"name\"", 1)]
        [DataRow("=", 1)]
        [DataRow(" ", 1)]
        [DataRow("   ", 3)]
        [DataRow("[name]", 3)]
        [DataRow("[name] ", 4)]
        [DataRow("\"this is an input\"", 1)]
        [DataRow(",", 1)]
        public void MatchesCorrectly_Length(string input, int desiredLength)
        {
            var tokens = GetTokens(input);
            Assert.AreEqual(desiredLength, tokens.Length);
        }

        [DataTestMethod]
        [DataRow("[", TokenKey.LeftSquareBracket)]
        [DataRow("]", TokenKey.RightSquareBracket)]
        [DataRow("{", TokenKey.LeftCurlyBrace)]
        [DataRow("}", TokenKey.RightCurlyBrace)]
        [DataRow("=", TokenKey.Equals)]
        [DataRow(" ", TokenKey.Space)]
        [DataRow("\"name\"", TokenKey.Value)]
        [DataRow("name", TokenKey.Value)]
        [DataRow("c:\\temp\\data.sqlite", TokenKey.Value)]
        [DataRow(",", TokenKey.Comma)]
        public void MatchesCorrectly_Key(string input, TokenKey desiredKey)
        {
            var tokens = GetTokens(input);
            Assert.AreEqual(desiredKey, tokens[0].Key);
        }

        [DataTestMethod]
        [DataRow("[")]
        [DataRow("]")]
        [DataRow("{")]
        [DataRow("}")]
        [DataRow("=")]
        [DataRow(" ")]
        [DataRow(",")]
        [DataRow("\"name\"")]
        [DataRow("name")]
        [DataRow("c:\\temp\\data.sqlite")]
        public void MatchesCorrectly_Content(string input)
        {
            var tokens = GetTokens(input);
            Assert.AreEqual(input, tokens[0].Contents);
        }

        [DataTestMethod]
        [DataRow("[name]", new TokenKey[] { TokenKey.LeftSquareBracket, TokenKey.Value, TokenKey.RightSquareBracket })]
        [DataRow("name\nname", new TokenKey[] { TokenKey.Value, TokenKey.NewLine, TokenKey.Value })]
        [DataRow("name = fergus", new TokenKey[] { TokenKey.Value, TokenKey.Space, TokenKey.Equals, TokenKey.Space, TokenKey.Value })]
        [DataRow("{name,name}", new TokenKey[] { TokenKey.LeftCurlyBrace, TokenKey.Value, TokenKey.Comma, TokenKey.Value, TokenKey.RightCurlyBrace })]
        public void Test(string input, TokenKey[] expectedKeys)
        {
            var tokens = GetTokens(input);
            for (int i = 0; i < tokens.Length; i++)
            {
                Assert.AreEqual(expectedKeys[i], tokens[i].Key);
            }
        }
    }
}
