using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Stpm.WebApi.Extensions;

public static class StringExtensions
{
    public static IEnumerable<string> SplitCamelCase(this string input)
    {
        return Regex.Split(input, @"([A-Z]?[a-z]+)").Where(str => !string.IsNullOrEmpty(str));
    }

    public static string FirstCharUppercase(this string input)
    {
        return $"{char.ToUpper(input[0])}{input.Substring(1)}";
    }

    public static string GenerateSlug(this string slug)
    {
        slug = RemoveAccents(slug);
        var splitToValidFormat = slug.Split(new[] { " ", ",", ";", ".", "-", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < splitToValidFormat.Length; i++)
        {
            splitToValidFormat[i] = splitToValidFormat[i].FirstCharUppercase();
        }
        var refixAlphabet = splitToValidFormat;
        var slugFormat = string.Join("", refixAlphabet);
        var reflectionSlug = String.Join("-", slugFormat.SplitCamelCase());

        return reflectionSlug.ToLower();
    }

    public static string RemoveAccents(this string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return text;

        text = text.Replace("Đ", "D").Replace("đ", "d").Replace("Đ", "D").Replace("đ", "d");
        text = text.Normalize(NormalizationForm.FormD);
        char[] chars = text
            .Where(c => CharUnicodeInfo.GetUnicodeCategory(c)
            != UnicodeCategory.NonSpacingMark).ToArray();

        return new string(chars).Normalize(NormalizationForm.FormC);
    }
}
