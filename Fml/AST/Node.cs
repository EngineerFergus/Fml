using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fml.Core;

namespace Fml.AST
{
    internal class Node
    {
        public Node? Left { get; }
        public Node? Right { get; }
        public ExprKey Key { get; }
        public FmlToken Token { get; }

        public Node(Node? left, Node? right, ExprKey key, FmlToken token)
        {
            Left = left;
            Right = right;
            Key = key;
            Token = token;
        }

        public override string ToString()
        {
            return $"({Left}) | [{Key} : {Token.Contents}] | ({Right})";
        }
    }
}
