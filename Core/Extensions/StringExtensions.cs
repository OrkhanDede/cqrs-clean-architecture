using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Core.Constants;

namespace Core.Extensions
{
    public static class StringExtensions
    {
        public static bool IsEmail(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;
            var emailRegex = RegexConstants.EmailRegex;
            var re = new Regex(emailRegex);
            return re.IsMatch(s);

        }

        public static bool IsUsername(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;
            var usernameRegex = RegexConstants.UserNameRegex;
            var re = new Regex(usernameRegex);
            return re.IsMatch(s);
        }
        public static string FirstLetterToUpper(this string str)
        {


            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }

        public static string ReplaceRandomChars(this string s, char c, int count = 1)
        {
            var content = s;
            var length = content.Length;

            if (count > length)
                throw new Exception("Replaced char count cant be higher than string length!");
            var replacedIndexes = new List<int>();
            for (int i = 0; i < count; i++)
            {
                int randomIndex = 0;
                bool isWhiteSpace = true;

                do
                {
                    randomIndex = new Random().Next(0, length - 1);
                    isWhiteSpace = Char.IsWhiteSpace(content[randomIndex]);
                    Console.WriteLine("Char :{0}   Result:{1}", content[randomIndex], isWhiteSpace);

                } while (replacedIndexes.Contains(randomIndex) || isWhiteSpace);

                StringBuilder sb = new StringBuilder(content);
                sb[randomIndex] = c;
                content = sb.ToString();
                replacedIndexes.Add(randomIndex);


            }

            return content;
        }

        public static List<string> SplitCsv(this string csvList, bool nullOrWhitespaceInputReturnsNull = false)
        {
            if (string.IsNullOrWhiteSpace(csvList))
                return nullOrWhitespaceInputReturnsNull ? null : new List<string>();

            return csvList
                .TrimEnd(',')
                .Split(',')
                .AsEnumerable<string>()
                .Select(s => s.Trim())
                .ToList();
        }

        public static string ReplaceValue(this string s, string v , string key="{{value}}")
        {
            return s.Replace(key, v);
        }

        public static string ToPascalCase(this string s)
        {
            var textInfo = CultureInfo.InvariantCulture.TextInfo;

            Regex invalidCharsRgx = new Regex("[^_a-zA-Z0-9]");
            Regex whiteSpace = new Regex(@"(?<=\s)");
            Regex startsWithLowerCaseChar = new Regex("^[a-z]");
            Regex firstCharFollowedByUpperCasesOnly = new Regex("(?<=[A-Z])[A-Z0-9]+$");
            Regex lowerCaseNextToNumber = new Regex("(?<=[0-9])[a-z]");
            Regex upperCaseInside = new Regex("(?<=[A-Z])[A-Z]+?((?=[A-Z][a-z])|(?=[0-9]))");

            // replace white spaces with undescore, then replace all invalid chars with empty string
            var pascalCase = invalidCharsRgx.Replace(whiteSpace.Replace(s, "_"), string.Empty)
                // split by underscores
                .Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries)
                // set first letter to uppercase
                .Select(w => startsWithLowerCaseChar.Replace(w, m => textInfo.ToUpper(m.Value)))
                // replace second and all following upper case letters to lower if there is no next lower (ABC -> Abc)
                .Select(w => firstCharFollowedByUpperCasesOnly.Replace(w, m => textInfo.ToLower(m.Value)))
                // set upper case the first lower case following a number (Ab9cd -> Ab9Cd)
                .Select(w => lowerCaseNextToNumber.Replace(w, m => textInfo.ToUpper(m.Value)))
                // lower second and next upper case letters except the last if it follows by any lower (ABcDEf -> AbcDef)
                .Select(w => upperCaseInside.Replace(w, m => textInfo.ToLower(m.Value)));
         
            return string.Concat(pascalCase);
        }

        public static string PhoneNumberEncrypted(this string phone)
        {
            if (string.IsNullOrEmpty(phone)) return phone;
            else if (phone.Length >= 10)
            {
                var phoneStarCount = phone.Length - 1;
                var phoneQaliqCount = phone.Length - phoneStarCount;
                var starts = new string('*', phoneStarCount);
                var lastTwoDigit = phone.Substring(phoneStarCount - 1, phoneQaliqCount + 1);
                var resulPhone = starts + lastTwoDigit;
                return resulPhone;
            }
            return phone;
        }
    }
}
