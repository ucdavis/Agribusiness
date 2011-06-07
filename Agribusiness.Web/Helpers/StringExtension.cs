using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Agribusiness.Web.Helpers
{
    public static class StringExtension
    {
        /// <summary>
        /// Returns the index of all occurances of a specific string
        /// </summary>
        /// <remarks>
        /// Code taken from: http://www.dijksterhuis.org/manipulating-strings-in-csharp-finding-all-occurrences-of-a-string-within-another-string/
        /// </remarks>
        /// <param name="source"></param>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public static IEnumerable IndexOfAll(this string source, string searchTerm)
        {
            int pos, offset = 0;

            while ((pos = source.IndexOf(searchTerm)) > 0)
            {
                source = source.Substring(pos + searchTerm.Length);
                offset += pos;
                yield return offset;
            }
        }

        /// <summary>
        /// Determines whether [is null or empty] [the specified source].
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="checkForSpacesOnly">if set to <c>true</c> [check for spaces only].</param>
        /// <returns>
        /// 	<c>true</c> if [is null or empty or spaces only] [the specified source]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty(this string source, bool checkForSpacesOnly)
        {
            if (string.IsNullOrEmpty(source)) return string.IsNullOrEmpty(source);

            if (checkForSpacesOnly)
            {
                source = source.Trim();
            }
            return string.IsNullOrEmpty(source);
        }


        public static string UpperFirstLetter(this string source, UpperFirstLetterOptions options)
        {
            if (source.IsNullOrEmpty(true))
            {
                return source;
            }
            if (options == UpperFirstLetterOptions.UseToTitle)
            {
                TextInfo myTi = new CultureInfo("en-US", false).TextInfo;
                return myTi.ToTitleCase(source);
            }
            else
            {
                char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
                var sourceCharArray = source.ToCharArray();
                var rtValue = string.Empty;
                for (int i = 0; i < sourceCharArray.Length; i++)
                {
                    if (i == 0)
                    {
                        sourceCharArray[i] = sourceCharArray[i].ToString().ToUpper()[0];
                    }
                    else
                    {
                        if (delimiterChars.Contains(sourceCharArray[i - 1]))
                        {
                            sourceCharArray[i] = sourceCharArray[i].ToString().ToUpper()[0];
                        }
                        else
                        {
                            if (options == UpperFirstLetterOptions.UpperWordsFirstLetterLowerOthers)
                            {
                                sourceCharArray[i] = sourceCharArray[i].ToString().ToLower()[0];
                            }
                        }
                    }
                    rtValue = rtValue + sourceCharArray[i];
                }


                return rtValue;
            }
        }

        public enum UpperFirstLetterOptions
        {
            UseToTitle,
            UpperWordsFirstLetter,
            UpperWordsFirstLetterLowerOthers
        }
    }
}