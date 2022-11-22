using Fml.AST;
using Fml.Core;
using Fml.Scanner;

namespace Fml.Tests.AST
{
    [TestClass]
    public class ParserTests
    {
        private static Parser MakeParser(string input)
        {
            FmlScanner scanner = new FmlScanner();
            var tokens = scanner.Tokenize(new StringReader(input));
            return new Parser(tokens);
        }

        private static List<Expr> ParseInput(string input)
        {
            var parser = MakeParser(input);
            return parser.Parse();
        }

        [DataTestMethod]
        [DataRow("[app]", 1)]
        [DataRow(" [app] ", 1)]
        [DataRow("version = 4.3.1.1", 1)]
        [DataRow("[app]\n version = 4.3.1.1", 2)]
        [DataRow("[app]\n#comment", 1)]
        [DataRow("[app]#comment", 1)]
        public void ParsesExpression_Length(string input, int expectedLength)
        {
            var exprs = ParseInput(input);
            Assert.AreEqual(expectedLength, exprs.Count);
        }

        [DataTestMethod]
        [DataRow("[app]", "app")]
        [DataRow(" [app] ", "app")]
        [DataRow("version = 4.3.1.1", "version")]
        [DataRow("version = v4.3 beta", "version")]
        [DataRow("version = v4.3 beta # comment", "version")]
        public void ParsesExpression_Identifier(string input, string identifier)
        {
            var exprs = ParseInput(input);
            Assert.AreEqual(identifier, exprs[0].Identifier.Contents);
        }

        [DataTestMethod]
        [DataRow("version = 4.3.1.1", "4.3.1.1")]
        [DataRow("version = v4.3 beta", "v4.3 beta")]
        [DataRow("version = []Hello wild input", "[]Hello wild input")]
        [DataRow("version = 4.3.1.1 # this is a comment", "4.3.1.1")]
        public void ParsesExpression_Value(string input, string value)
        {
            var exprs = ParseInput(input);
            FmlToken val = exprs[0].Value ?? throw new Exception("Got null value");
            Assert.AreEqual(value, val.Contents);
        }

        [DataTestMethod]
        [DataRow("version 1 = v1.0")]
        [DataRow("[[app]")]
        [DataRow("{{}")]
        public void ImproperInputThrowsException(string input)
        {
            Assert.ThrowsException<Exception>(() => { _ = ParseInput(input); });
        }
    }
}
