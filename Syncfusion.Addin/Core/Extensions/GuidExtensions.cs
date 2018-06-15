using System;
using System.Text.RegularExpressions;

namespace Syncfusion.Core.Extensions
{
   public static class GuidExtensions
    {
        /// <summary>
        /// Determines whether the specified unique identifier is unique identifier.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns><c>true</c> if the specified unique identifier is unique identifier; otherwise, <c>false</c>.</returns>
        public static bool IsGuid(this string guid)
       {
           try
           {
                Match match = Regex.Match(guid, @"^[0-9a-f]{8}(-[0-9a-f]{4}){3}-[0-9a-f]{12}$", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    return true;
                }
            }
           catch (FormatException)
           {
               return false;
           }
            return false;
        }
    }
}
