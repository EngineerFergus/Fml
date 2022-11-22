using Fml.AST;
using Fml.Core;
using Fml.Scanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fml
{
    public class FmlMap
    {
        public Dictionary<string, string> GlobalPairs { get; } = new();

        public Dictionary<string, Dictionary<string, string>> Sections { get; } = new();

        private readonly List<Expr> _exprs;

        private FmlMap(TextReader reader)
        {
            FmlScanner scanner = new();
            var tokens = scanner.Tokenize(reader);
            Parser parser = new(tokens);
            _exprs = parser.Parse();
        }

        private void MapExprs()
        {
            bool hitFirstSection = false;
            string currentSection = string.Empty;

            foreach (Expr expr in _exprs)
            {
                if(expr.Key == Core.ExprKey.Section)
                {
                    hitFirstSection = true;
                    currentSection = expr.Identifier.Contents;
                    continue;
                }

                if (hitFirstSection)
                {
                    if (Sections.ContainsKey(currentSection) == false)
                    {
                        Sections.Add(currentSection, new());
                    }

                    Dictionary<string, string> section = Sections[currentSection];
                    
                    if (section.ContainsKey(expr.Identifier.Contents))
                    {
                        throw new Exception($"Cannot have multiple identifiers of the same name, " +
                            $"identifier: {expr.Identifier.Contents}, line: {expr.Identifier.Line}");
                    }

                    FmlToken value = expr.Value ?? throw new Exception($"Retrieved a null token value, {expr.Identifier.Line}");
                    section.Add(expr.Identifier.Contents, value.Contents);

                }
                else
                {
                    if (GlobalPairs.ContainsKey(expr.Identifier.Contents))
                    {
                        throw new Exception($"Cannot have multiple identifiers of the same name, " +
                            $"identifier :{expr.Identifier.Contents}, line: {expr.Identifier.Line}");
                    }

                    FmlToken value = expr.Value ?? throw new Exception($"Retrieved a null value token, {expr.Identifier.Line}");
                    GlobalPairs.Add(expr.Identifier.Contents, value.Contents);
                }
            }
        }

        public static FmlMap ReadFile(string filePath)
        {
            using TextReader reader = new StreamReader(filePath);
            return ReadStream(reader);
        }

        public static FmlMap ReadStream(TextReader reader)
        {
            FmlMap map = new FmlMap(reader);
            map.MapExprs();
            return map;
        }
    }
}
