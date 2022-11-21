using Fml.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fml.AST
{
    internal class Parser
    {
        private FmlToken[] _tokens;

        private FmlToken? _currentToken;
        private int _position;

        public Parser(FmlToken[] tokens)
        {
            _tokens = tokens;
            _position = -1;
        }

        public Node[] Parse()
        {
            List<Node> nodes = new List<Node>();

            while (TryParse(out Node? node))
            {
                if(node == null)
                {
                    throw new Exception("Retrieved null node!");
                }

                nodes.Add(node);
            }

            return nodes.ToArray();
        }

        private bool TryParse(out Node? node)
        {
            node = null;



            return false;
        }

        public Node Assignment()
        {
            throw new NotImplementedException();
        }

        public bool Section(out Node? node)
        {
            node = null;
            bool success = false;
            Advance();
            FmlToken token = _currentToken ??  throw new Exception();

            if (token.Key == TokenKey.LeftSquareBracket)
            {
                Advance();

                FmlToken valueToken = _currentToken ?? throw new Exception();

                if (valueToken.Key != TokenKey.Value)
                {
                    throw new Exception($"Found issue parsing section near \"{valueToken.Contents}\"");
                }

                do
                {
                    Advance();
                    FmlToken closingToken = _currentToken ?? throw new Exception();

                    if (closingToken.Key == TokenKey.NewLine) { throw new Exception($"Imbalanced bracket on line {closingToken.Line}"); }

                    if (closingToken.Key == TokenKey.Space) { continue; }

                    if (closingToken.Key == TokenKey.RightSquareBracket) { break; }

                    throw new Exception($"Sections cannot have mutliple values or spaces, line {closingToken.Line}");
                }
                while (true);

                node = new Node(null, null, ExprKey.Section, valueToken);
                success = true;
            }

            return success;
        }

        public bool Identifier(out Node? node)
        {
            node = null;
            bool success = false;
            Advance();
            FmlToken? token = _currentToken;

            if (token == null) { throw new Exception(); }

            if (token.Key == TokenKey.Value)
            {
                node = new Node(null, null, ExprKey.Identifier, token);
                success = true;
            }

            return success;
        }

        private bool Advance()
        {
            _position++;

            if (_position >= _tokens.Length)
            {
                _currentToken = null;
                return false;
            }

            _currentToken = _tokens[_position];
            return true;
        }
    }
}
