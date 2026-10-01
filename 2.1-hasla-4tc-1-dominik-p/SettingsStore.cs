using System.IO;
using System.Text.Json;

namespace Haslogen;

public static class SettingsStore
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };

    public static string FilePath { get; } = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "Haslogen",
        "settings.json");

    public static PasswordOptions Load()
    {
        try
        {
            if (!File.Exists(FilePath))
            {
                return new PasswordOptions();
            }

            var json = File.ReadAllText(FilePath);
            var loaded = JsonSerializer.Deserialize<PasswordOptions>(json);
            if (loaded is null)
            {
                return new PasswordOptions();
            }

            loaded.Length = Math.Clamp(loaded.Length, 4, 64);
            if (loaded.SelectedSetCount == 0)
            {
                loaded.UseLowercase = true;
            }

            return loaded;
        }
        catch
        {
            return new PasswordOptions();
        }
    }

    public static void Save(PasswordOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var directory = Path.GetDirectoryName(FilePath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(FilePath, JsonSerializer.Serialize(options, JsonOptions));
    }
}
