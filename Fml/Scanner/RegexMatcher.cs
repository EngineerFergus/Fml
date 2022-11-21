using System.Text.RegularExpressions;

namespace Fml.Scanner
{
    internal class RegexMatcher : IMatcher
    {
        private readonly Regex _regex;

        public RegexMatcher(string regex)
        {
            _regex = new Regex(string.Format("^{0}", regex));
        }

        public int Match(string text)
        {
            var m = _regex.Match(text);
            return m.Success ? m.Length : 0;
        }

        public override string ToString()
        {
            return _regex.ToString();
        }
    }
}
