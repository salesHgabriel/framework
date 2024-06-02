using DotNetEnv;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spark.Library.Extensions;

public static class StringExtensions
{

    private static Dictionary<string, Dictionary<string, string>> _snakeCache = new Dictionary<string, Dictionary<string, string>>();

    public static string Clamp(this string value, int maxChars)
    {
        return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
    }

    public static string ToSlug(this string input)
    {
        string str = input.RemoveDiacritics().ToLower();
        // invalid chars
        str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
        // convert multiple spaces into one space
        str = Regex.Replace(str, @"\s+", " ").Trim();
        // cut and trim
        str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
        // hyphens
        str = Regex.Replace(str, @"\s", "-");
        return str;
    }

    private static string RemoveDiacritics(this string str)
    {
        var normalizedString = str.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder(capacity: normalizedString.Length);

        for (int i = 0; i < normalizedString.Length; i++)
        {
            char c = normalizedString[i];
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder
            .ToString()
            .Normalize(NormalizationForm.FormC);
    }

    /// <summary>
    /// Returns the string with the first letter set to uppercase.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToUpperFirst(this string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        if (value.Length == 1)
        {
            return value.ToUpper();
        }

        char firstChar = char.ToUpper(value[0]);
        string restOfString = value[1..];
        return firstChar + restOfString;
    }


    /// <summary>
    /// The mask method masks a portion of a string with a repeated character, and may be used to obfuscate segments of strings such as email addresses and phone numbers:
    /// </summary>
    /// <param name="str"></param>
    /// <param name="character"></param>
    /// <param name="index"></param>
    /// <param name="length"></param>
    /// <param name="encoding"></param>
    /// <returns>string</returns>
    public static string Mask(this string str, string character, int index, int? length = null, string encoding = "UTF-8")
    {
        if (character == "")
        {
            return str;
        }

        string segment = length != null ? str.Substring(index, length.Value) : str.Substring(index);

        if (segment == "")
        {
            return str;
        }

        int strlen = Encoding.GetEncoding(encoding).GetByteCount(str);
        int startIndex = index;

        if (index < 0)
        {
            startIndex = index < -strlen ? 0 : strlen + index;
        }

        string start = str.Substring(0, startIndex);
        int segmentLen = Encoding.GetEncoding(encoding).GetByteCount(segment);
        string end = str.Substring(startIndex + segmentLen);

        return start + new string(character[0], segmentLen) + end;
    }


    /// <summary>
    /// The snake method converts the given string to snake_case:
    /// </summary>
    /// <param name="value"></param>
    /// <param name="delimiter"></param>
    /// <returns>string</returns>
    public static string Snake(this string value, string delimiter = "_")
    {
        string key = value;

        if (_snakeCache.ContainsKey(key) && _snakeCache[key].ContainsKey(delimiter))
        {
            return _snakeCache[key][delimiter];
        }

        if (!string.IsNullOrEmpty(value) && !value.All(char.IsLower))
        {
            value = Regex.Replace(value, @"\s+", string.Empty);
            value = Regex.Replace(value, @"(.)(?=[A-Z])", "$1" + delimiter);
            value = value.ToLowerInvariant();
        }

        string result = value;

        if (_snakeCache.ContainsKey(key))
        {
            _snakeCache[key].Add(delimiter, result);
        }
        else
        {
            _snakeCache.Add(key, new Dictionary<string, string> { { delimiter, result } });
        }

        return result;
    }


    /// <summary>
    /// The method determines if the given string is valid JSON:
    /// </summary>
    /// <param name="value"></param>
    /// <returns>bool</returns>
    public static bool IsJson(this string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        try
        {
            var result = JsonSerializer.Deserialize<dynamic>(value);
            return true;
        }
        catch (JsonException)
        {
            return false;
        }
    }

}
