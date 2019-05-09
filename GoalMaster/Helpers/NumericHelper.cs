using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GoalMaster.Helpers
{
    public static class NumericHelper
    {
        private static readonly Regex _regex = new Regex("[^0-9.,]+"); //regex that matches disallowed text

        public static bool IsTextNumericOnly(string text)
        {
            if (text == null) return true;

            if (!isFirstLetterDotOrComma(text) && isMaxSingleDotOrComma(text))
            {
                return !_regex.IsMatch(text);
            }
            else return false;
        }
        private static bool isMaxSingleDotOrComma(string text)
        {
            if (text == null) return true;
            int dots = 0;
            int commas = 0;
            foreach (var sign in text)
            {
                if (sign == '.') dots++;
                if (sign == ',') commas++;
            }
            if (dots > 0 && commas > 0)
                return false;
            return (dots == 1 || dots == 0) && (commas==0 || commas==1);
        }
        private static bool isFirstLetterDotOrComma(string text)
        {
            if (text != "" && text!=null)
                return text.First() == '.' || text.First()==',';
            else return false;
        }
        public static bool isLastLetterDotOrComma(string text)
        {
            if (text != "" && text != null)
                return text.Last() == '.' || text.Last() == ',';
            else return false;
        }
    }
}
