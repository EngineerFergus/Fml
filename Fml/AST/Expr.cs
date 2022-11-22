using Fml.Core;

namespace Fml.AST
{
    public class Expr
    {
        public ExprKey Key { get; }
        public FmlToken Identifier { get; }
        public FmlToken? Value { get; }

        public Expr(ExprKey key, FmlToken identifier, FmlToken? value)
        {
            Key = key;
            Identifier = identifier;
            Value = value;
        }
    }
}
