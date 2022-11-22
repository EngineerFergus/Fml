using Fml.Core;

namespace Fml.AST
{
    public class Parser
    {
        private readonly FmlToken[] _tokens;

        private int _position;

        public Parser(FmlToken[] tokens)
        {
            _tokens = tokens;
            _position = 0;
        }

        public List<Expr> Parse()
        {
            List<Expr> exprs = new List<Expr>();

            while (!IsAtEnd())
            {
                bool parsed = ParseLine(out Expr? expr);
                if (parsed && expr != null)
                {
                    exprs.Add(expr);
                }
            }

            return exprs;
        }

        private bool ParseLine(out Expr? expr)
        {
            bool success = true;
            expr = null;

            List<FmlToken> line = ObtainLine();

            if(line.Count == 0) 
            {
                if (Current().Key == TokenKey.NewLine)
                {
                    Advance();
                }

                return false;
            }

            if(line.First().Key == TokenKey.LeftSquareBracket)
            {
                // make section declaration
                expr = Section(line);
                success = true;
            }
            else
            {
                // make assignment declaration
                expr = Assignment(line);
                success = true;
            }

            Advance(); // advance past newline

            return success;
        }

        private List<FmlToken> ObtainLine()
        {
            List<FmlToken> line = new();

            do
            {
                if (IsAtEnd()) { break; }

                if (Current().Key == TokenKey.Pound)
                {
                    ConsumeToLineEnd();
                    break;
                }

                line.Add(Current());
                Advance();
            }
            while (Current().Key != TokenKey.NewLine);

            line = TrimWhiteSpace(line);

            return line;
        }

        private void ConsumeToLineEnd()
        {
            while (Current().Key != TokenKey.NewLine && !IsAtEnd())
            {
                Advance();
            }
        }

        private List<FmlToken> TrimWhiteSpace(List<FmlToken> line)
        {
            if(line.Count == 0) { return line; }

            List<FmlToken> trimmed = new List<FmlToken>();

            int start = 0;
            int stop = line.Count;

            for (int i = 0; i < line.Count; i++)
            {
                if (line[i].Key == TokenKey.Space) { start++; }
                else
                {
                    break;
                }
            }

            for (int i = line.Count - 1; i >= 0; i--)
            {
                if (line[i].Key == TokenKey.Space) { stop--; }
                else
                {
                    break;
                }
            }

            for (int i = start; i < stop; i++)
            {
                trimmed.Add(line[i]);
            }

            return trimmed;
        }

        private Expr Assignment(List<FmlToken> line)
        {
            if (line.Count < 3)
            {
                ThrowParserException("Improper declaration", line[0].Line);
            }

            if (CountKeyType(line, TokenKey.Equals) != 1)
            {
                ThrowParserException("Expected single \'=\' for value assignment", line[0].Line);
            }

            int identifierIdx = 0, equalIdx = -1, valueStartIndex = -1;

            if (line[identifierIdx].Key != TokenKey.Value)
            {
                ThrowParserException("Unexpected identifier symbol", line[0].Line);
            }

            for(int i = 1; i < line.Count; i++)
            {
                if (line[i].Key == TokenKey.Equals)
                {
                    equalIdx = i;
                }

                if (line[i].Key != TokenKey.Equals && line[i].Key != TokenKey.Space && valueStartIndex == -1)
                {
                    valueStartIndex = i;
                }
            }

            if (valueStartIndex < equalIdx)
            {
                ThrowParserException("Value names cannot have more than a single identifier", line[0].Line);
            }

            FmlToken value = line[valueStartIndex];

            for (int i = valueStartIndex + 1; i < line.Count; i++)
            {
                value = value.MakeWithCombinedContents(line[i]);
            }

            return new Expr(ExprKey.Assignment, line[0], value);
        }

        private Expr Section(List<FmlToken> line)
        {
            if(line.Count < 3)
            {
                ThrowParserException("Improper declaration", line[0].Line);
            }

            if (!HasBalancedBraces(line))
            {
                ThrowParserException("Imbalanced brackets/braces", line[0].Line);
            }

            if (CountKeyType(line, TokenKey.LeftSquareBracket) > 1)
            {
                ThrowParserException("Only expected single square brackets for section declaration", line[0].Line);
            }

            FmlToken first = line.First();
            FmlToken last = line.Last();

            if(first.Key != TokenKey.LeftSquareBracket || last.Key != TokenKey.RightSquareBracket)
            {
                ThrowParserException("Invalid section declaration, expected opening and closing square brackets", line.First().Line);
            }

            FmlToken value = line[1];

            for (int i = 2; i < line.Count - 1; i++)
            {
                value = value.MakeWithCombinedContents(line[i]);
            }

            return new Expr(ExprKey.Section, value, null);
        }

        private static bool HasBalancedBraces(IEnumerable<FmlToken> line)
        {
            int leftSquareCount = line.Where(l => l.Key == TokenKey.LeftSquareBracket).Count();
            int rightSquareCount = line.Where(l => l.Key == TokenKey.RightSquareBracket).Count();
            int leftCurlyCount = line.Where(l => l.Key == TokenKey.LeftCurlyBrace).Count();
            int rightCurlyCount = line.Where(l => l.Key == TokenKey.RightCurlyBrace).Count();

            return leftSquareCount == rightSquareCount && leftCurlyCount == rightCurlyCount;
        }

        private static int CountKeyType(IEnumerable<FmlToken> line, TokenKey key)
        {
            return line.Where(l => l.Key == key).Count();
        }

        private void Advance()
        {
            _position++;
        }

        private bool Check(TokenKey key)
        {
            if (IsAtEnd()) { return false; }
            return Current().Key == key;
        }

        private bool IsAtEnd()
        {
            return _position >= _tokens.Length || Current().Key == TokenKey.EOF;
        }

        private FmlToken PastToken()
        {
            return _tokens[_position - 1];
        }

        private FmlToken Current()
        {
            return _tokens[_position];
        }

        private void ThrowParserException(string message, int line)
        {
            throw new Exception($"Parser error on line {line}, {message}");
        }
    }
}
