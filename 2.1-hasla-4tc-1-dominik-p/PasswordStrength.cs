namespace Haslogen;

public enum StrengthLevel
{
    Weak,
    Medium,
    Strong
}

public static class PasswordStrength
{
    public static StrengthLevel Evaluate(PasswordOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var lengthPoints = options.Length switch
        {
            < 8 => 1,
            < 12 => 2,
            < 16 => 3,
            _ => 4
        };

        var score = lengthPoints + options.SelectedSetCount;

        if (score <= 3)
        {
            return StrengthLevel.Weak;
        }

        if (score <= 5)
        {
            return StrengthLevel.Medium;
        }

        return StrengthLevel.Strong;
    }

    public static string ToPolish(StrengthLevel level) => level switch
    {
        StrengthLevel.Weak => "Słabe",
        StrengthLevel.Medium => "Średnie",
        StrengthLevel.Strong => "Silne",
        _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
    };
}
