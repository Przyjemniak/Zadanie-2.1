# Haslogen

Prosty generator losowych haseł na Windows (C# / .NET 8 / WPF).

## Wymagania

- Windows
- [SDK .NET 8](https://dotnet.microsoft.com/download/dotnet/8.0) (lub nowszy SDK potrafiący kompilować `net8.0-windows`)

## Uruchomienie

W katalogu projektu:

```powershell
dotnet build
dotnet run
```

Albo po kompilacji: `bin\Debug\net8.0-windows\Haslogen.exe`.

## Jak używać

1. Ustaw długość suwakiem (4–64).
2. Zaznacz zestawy: wielkie litery, małe litery, cyfry, znaki specjalne (co najmniej jeden).
3. Kliknij **Generuj**.
4. Kliknij **Kopiuj**, aby wkleić hasło ze schowka.

Wskaźnik siły (słabe / średnie / silne) zależy od długości i liczby zestawów — progi są opisane w `DECISIONS.md`.

Ostatnie ustawienia są zapisywane w `%AppData%\Haslogen\settings.json`.

## Czego nie dokończono

- Brak instalacji MSI / MSIX i skrótu w menu Start.
- Brak historii wcześniej wygenerowanych haseł.
- Brak eksportu / zapisu haseł do pliku (świadomie — generator, nie menedżer haseł).
- Brak testów jednostkowych.
- Interfejs tylko po polsku, bez przełącznika języka.
