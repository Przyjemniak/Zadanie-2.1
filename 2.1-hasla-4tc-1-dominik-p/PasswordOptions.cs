namespace Haslogen;

public sealed class PasswordOptions
{
    public int Length { get; set; } = 16;
    public bool UseUppercase { get; set; } = true;
    public bool UseLowercase { get; set; } = true;
    public bool UseDigits { get; set; } = true;
    public bool UseSpecial { get; set; } = true;

    public int SelectedSetCount
    {
        get
        {
            var count = 0;
            if (UseUppercase) count++;
            if (UseLowercase) count++;
            if (UseDigits) count++;
            if (UseSpecial) count++;
            return count;
        }
    }
}
