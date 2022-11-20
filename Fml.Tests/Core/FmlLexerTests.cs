using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fml;
using Fml.Core;

namespace Fml.Tests.Core
{
    [TestClass]
    public class FmlLexerTests
    {
        private readonly Tokenizer _tokenizer = new();

        [TestMethod]
        public void MatchesSquareBrackets()
        {
            using TextReader reader = new StringReader("[]");
            var tokens = _tokenizer.Tokenize(reader);
            Assert.AreEqual(2, tokens.Length);
            Assert.AreEqual(TokenKey.LeftSquareBracket, tokens[0].Key);
            Assert.AreEqual("[", tokens[0].Contents);
            Assert.AreEqual(TokenKey.RightSquareBracket, tokens[1].Key);
            Assert.AreEqual("]", tokens[1].Contents);
        }

        [TestMethod]
        public void MatchesQuotedString()
        {
            string input = "\"My String\"";
            using TextReader reader = new StringReader(input);
            var tokens = _tokenizer.Tokenize(reader);
            Assert.AreEqual(1, tokens.Length);
            Assert.AreEqual(TokenKey.Value, tokens[0].Key);
            Assert.AreEqual(input, tokens[0].Contents);
        }

        [TestMethod]
        public void Matches
    }
}
