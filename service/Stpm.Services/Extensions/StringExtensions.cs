using Microsoft.AspNetCore.Identity;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Stpm.Services.Extensions;

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

        text = text.Replace("Đ", "D").Replace("đ", "d");
        text = text.Normalize(NormalizationForm.FormD);
        char[] chars = text
            .Where(c => CharUnicodeInfo.GetUnicodeCategory(c)
            != UnicodeCategory.NonSpacingMark).ToArray();

        return new string(chars).Normalize(NormalizationForm.FormC);
    }

    /// <summary>
    /// Generates a Random Password
    /// respecting the given strength requirements.
    /// </summary>
    /// <param name="opts">A valid PasswordOptions object
    /// containing the password strength requirements.</param>
    /// <returns>A random password</returns>
    public static string GenerateRandomPassword(PasswordOptions opts = null)
    {
        if (opts == null) opts = new PasswordOptions()
        {
            RequiredLength = 8,
            RequiredUniqueChars = 4,
            RequireDigit = true,
            RequireLowercase = true,
            RequireNonAlphanumeric = true,
            RequireUppercase = true
        };

        string[] randomChars = new[] {
            "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase 
            "abcdefghijkmnopqrstuvwxyz",    // lowercase
            "0123456789",                   // digits
            "!@$?_-"                        // non-alphanumeric
        };

        Random rand = new Random(Environment.TickCount);
        List<char> chars = new List<char>();

        if (opts.RequireUppercase)
            chars.Insert(rand.Next(0, chars.Count),
                randomChars[0][rand.Next(0, randomChars[0].Length)]);

        if (opts.RequireLowercase)
            chars.Insert(rand.Next(0, chars.Count),
                randomChars[1][rand.Next(0, randomChars[1].Length)]);

        if (opts.RequireDigit)
            chars.Insert(rand.Next(0, chars.Count),
                randomChars[2][rand.Next(0, randomChars[2].Length)]);

        if (opts.RequireNonAlphanumeric)
            chars.Insert(rand.Next(0, chars.Count),
                randomChars[3][rand.Next(0, randomChars[3].Length)]);

        for (int i = chars.Count; i < opts.RequiredLength
            || chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
        {
            string rcs = randomChars[rand.Next(0, randomChars.Length)];
            chars.Insert(rand.Next(0, chars.Count),
                rcs[rand.Next(0, rcs.Length)]);
        }

        return new string(chars.ToArray());
    }
}
