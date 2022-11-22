using Fml.Core;

namespace Fml.AST
{
    public class Expr
    {
        public ExprKey Key { get; }
        public FmlToken Identifier { get; }
        public FmlToken[] Values { get; }

        public Expr(ExprKey key, FmlToken identifier, FmlToken[] values)
        {
            Key = key;
            Identifier = identifier;
            Values = values;
        }
    }
}
