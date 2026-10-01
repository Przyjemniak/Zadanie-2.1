using System.Security.Cryptography;

namespace Haslogen;

public static class PasswordGenerator
{
    public const string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public const string Lowercase = "abcdefghijklmnopqrstuvwxyz";
    public const string Digits = "0123456789";
    public const string Special = "!@#$%^&*()-_=+[]{};:,.?";

    public static string Generate(PasswordOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        if (options.Length is < 4 or > 64)
        {
            throw new ArgumentOutOfRangeException(nameof(options), "Długość hasła musi być w zakresie 4–64.");
        }

        var pools = new List<string>();
        if (options.UseUppercase) pools.Add(Uppercase);
        if (options.UseLowercase) pools.Add(Lowercase);
        if (options.UseDigits) pools.Add(Digits);
        if (options.UseSpecial) pools.Add(Special);

        if (pools.Count == 0)
        {
            throw new InvalidOperationException("Musi być wybrany co najmniej jeden zestaw znaków.");
        }

        if (options.Length < pools.Count)
        {
            throw new ArgumentException("Długość hasła nie może być mniejsza niż liczba wybranych zestawów.");
        }

        var combined = string.Concat(pools);
        var chars = new char[options.Length];
        var index = 0;

        foreach (var pool in pools)
        {
            chars[index++] = Pick(pool);
        }

        while (index < options.Length)
        {
            chars[index++] = Pick(combined);
        }

        Shuffle(chars);
        return new string(chars);
    }

    private static char Pick(string alphabet)
    {
        return alphabet[RandomNumberGenerator.GetInt32(alphabet.Length)];
    }

    private static void Shuffle(char[] chars)
    {
        for (var i = chars.Length - 1; i > 0; i--)
        {
            var j = RandomNumberGenerator.GetInt32(i + 1);
            (chars[i], chars[j]) = (chars[j], chars[i]);
        }
    }
}
