using Fml.Core;
using Fml.Scanner;
using System.Text;

namespace Fml.Tests.Scanner
{
    [TestClass]
    public class FmlScannerTests
    {
        private readonly FmlScanner _lexer = new();

        private FmlToken[] GetTokens(string input)
        {
            using TextReader reader = new StringReader(input);
            return _lexer.Tokenize(reader);
        }

        [DataTestMethod]
        [DataRow("[]", 3)]
        [DataRow("{}", 3)]
        [DataRow("name", 2)]
        [DataRow("\"name\"", 2)]
        [DataRow("=", 2)]
        [DataRow(" ", 2)]
        [DataRow("   ", 4)]
        [DataRow("[name]", 4)]
        [DataRow("[name] ", 5)]
        [DataRow("\"this is an input\"", 2)]
        [DataRow(",", 2)]
        [DataRow("#", 2)]
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
        [DataRow("#", TokenKey.Pound)]
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
        [DataRow("#")]
        [DataRow("\"name\"")]
        [DataRow("name")]
        [DataRow("c:\\temp\\data.sqlite")]
        public void MatchesCorrectly_Content(string input)
        {
            var tokens = GetTokens(input);
            Assert.AreEqual(input, tokens[0].Contents);
        }

        [DataTestMethod]
        [DataRow("[name]", new TokenKey[] { TokenKey.LeftSquareBracket, TokenKey.Value, TokenKey.RightSquareBracket, TokenKey.EOF })]
        [DataRow("name\nname", new TokenKey[] { TokenKey.Value, TokenKey.NewLine, TokenKey.Value, TokenKey.EOF })]
        [DataRow("name = fergus", new TokenKey[] { TokenKey.Value, TokenKey.Space, TokenKey.Equals, TokenKey.Space, TokenKey.Value, TokenKey.EOF })]
        [DataRow("{name,name}", new TokenKey[] { TokenKey.LeftCurlyBrace, TokenKey.Value, TokenKey.Comma, TokenKey.Value, TokenKey.RightCurlyBrace, TokenKey.EOF })]
        public void Test(string input, TokenKey[] expectedKeys)
        {
            var tokens = GetTokens(input);
            for (int i = 0; i < tokens.Length; i++)
            {
                Assert.AreEqual(expectedKeys[i], tokens[i].Key);
            }
        }

        [TestMethod]
        public void TestExtendedExampleA()
        {
            StringBuilder builder = new();
            builder.AppendLine("[app]");
            builder.AppendLine("name = Image Annotator");

            TokenKey[] expectedKeys =
            {
                TokenKey.LeftSquareBracket,
                TokenKey.Value,
                TokenKey.RightSquareBracket,
                TokenKey.NewLine,
                TokenKey.Value,
                TokenKey.Space,
                TokenKey.Equals,
                TokenKey.Space,
                TokenKey.Value,
                TokenKey.Space,
                TokenKey.Value,
                TokenKey.EOF,
            };

            var tokens = GetTokens(builder.ToString());
            Assert.AreEqual(expectedKeys.Length, tokens.Length);

            for (int i = 0; i < expectedKeys.Length; i++)
            {
                Assert.AreEqual(expectedKeys[i], tokens[i].Key);
            }
        }

        [TestMethod]
        public void TestExtendedExampleB()
        {
            StringBuilder builder = new();
            builder.AppendLine("[app]");
            builder.AppendLine("name = Image Annotator # comment");
            builder.AppendLine("# comment");
            builder.AppendLine("\" # quoted string \"");

            TokenKey[] expectedKeys =
            {
                TokenKey.LeftSquareBracket,
                TokenKey.Value,
                TokenKey.RightSquareBracket,
                TokenKey.NewLine,
                TokenKey.Value,
                TokenKey.Space,
                TokenKey.Equals,
                TokenKey.Space,
                TokenKey.Value,
                TokenKey.Space,
                TokenKey.Value,
                TokenKey.Space,
                TokenKey.Pound,
                TokenKey.Space,
                TokenKey.Value,
                TokenKey.NewLine,
                TokenKey.Pound,
                TokenKey.Space,
                TokenKey.Value,
                TokenKey.NewLine,
                TokenKey.Value,
                TokenKey.EOF,
            };

            var tokens = GetTokens(builder.ToString());
            Assert.AreEqual(expectedKeys.Length, tokens.Length);

            for (int i = 0; i < expectedKeys.Length; i++)
            {
                Assert.AreEqual(expectedKeys[i], tokens[i].Key);
            }
        }
    }
}
