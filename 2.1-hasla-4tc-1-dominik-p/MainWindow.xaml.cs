using System.Windows;
using System.Windows.Media;

namespace Haslogen;

public partial class MainWindow : Window
{
    private bool _updatingUi;
    private bool _settingsLoaded;

    public MainWindow()
    {
        InitializeComponent();
        LoadSettingsIntoUi();
        UpdateStrength();
        _settingsLoaded = true;
    }

    private void LoadSettingsIntoUi()
    {
        var options = SettingsStore.Load();
        _updatingUi = true;
        LengthSlider.Value = options.Length;
        LengthValueText.Text = options.Length.ToString();
        UppercaseCheck.IsChecked = options.UseUppercase;
        LowercaseCheck.IsChecked = options.UseLowercase;
        DigitsCheck.IsChecked = options.UseDigits;
        SpecialCheck.IsChecked = options.UseSpecial;
        _updatingUi = false;
        EnsureAtLeastOneCharset();
    }

    private PasswordOptions ReadOptionsFromUi()
    {
        return new PasswordOptions
        {
            Length = (int)LengthSlider.Value,
            UseUppercase = UppercaseCheck.IsChecked == true,
            UseLowercase = LowercaseCheck.IsChecked == true,
            UseDigits = DigitsCheck.IsChecked == true,
            UseSpecial = SpecialCheck.IsChecked == true
        };
    }

    private void LengthSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (LengthValueText is null)
        {
            return;
        }

        LengthValueText.Text = ((int)LengthSlider.Value).ToString();
        PersistAndRefresh();
    }

    private void Charset_Changed(object sender, RoutedEventArgs e)
    {
        if (_updatingUi)
        {
            return;
        }

        EnsureAtLeastOneCharset();
        PersistAndRefresh();
    }

    private void EnsureAtLeastOneCharset()
    {
        if (UppercaseCheck.IsChecked == true
            || LowercaseCheck.IsChecked == true
            || DigitsCheck.IsChecked == true
            || SpecialCheck.IsChecked == true)
        {
            return;
        }

        _updatingUi = true;
        LowercaseCheck.IsChecked = true;
        _updatingUi = false;
        StatusText.Text = "Musi być wybrany co najmniej jeden zestaw znaków.";
    }

    private void PersistAndRefresh()
    {
        if (!_settingsLoaded)
        {
            return;
        }

        SettingsStore.Save(ReadOptionsFromUi());
        UpdateStrength();
    }

    private void UpdateStrength()
    {
        if (StrengthText is null)
        {
            return;
        }

        var level = PasswordStrength.Evaluate(ReadOptionsFromUi());
        StrengthText.Text = PasswordStrength.ToPolish(level);
        StrengthText.Foreground = level switch
        {
            StrengthLevel.Weak => new SolidColorBrush(Color.FromRgb(192, 57, 43)),
            StrengthLevel.Medium => new SolidColorBrush(Color.FromRgb(211, 84, 0)),
            StrengthLevel.Strong => new SolidColorBrush(Color.FromRgb(39, 174, 96)),
            _ => Brushes.Black
        };
    }

    private void GenerateButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var password = PasswordGenerator.Generate(ReadOptionsFromUi());
            PasswordBox.Text = password;
            CopyButton.IsEnabled = true;
            StatusText.Text = "Wygenerowano nowe hasło.";
            UpdateStrength();
        }
        catch (Exception ex)
        {
            StatusText.Text = ex.Message;
        }
    }

    private void CopyButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(PasswordBox.Text))
        {
            StatusText.Text = "Najpierw wygeneruj hasło.";
            return;
        }

        Clipboard.SetText(PasswordBox.Text);
        StatusText.Text = "Hasło skopiowane do schowka.";
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        SettingsStore.Save(ReadOptionsFromUi());
    }
}
