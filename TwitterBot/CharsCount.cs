using System.Collections.Generic;

namespace TwitterBot
{
    /// <summary>
    /// Contains extension method for counting characters in IEnumerable<string>
    /// </summary>
    static class CharsCount
    {
        /// <summary>
        /// counts all character occurences in the collection of strings
        /// </summary>
        /// <param name="strs">collection of strings</param>
        /// <returns>(character, number of occurences)</returns>
        public static IDictionary<char, int> CountChars(this IEnumerable<string> strs)
        {
            return CountChars(string.Join("", strs));
        }

        static IDictionary<char, int> CountChars(string str)
        {
            var normalized = str.ToLower();
            var mostCommonCharCounts = new Dictionary<char, int>();
            foreach (var c in normalized)
            {
                if (!mostCommonCharCounts.ContainsKey(c))
                {
                    mostCommonCharCounts.Add(c, 1);
                }
                else
                {
                    mostCommonCharCounts[c]++;
                }
            }
            return mostCommonCharCounts;
        }
    }
}
