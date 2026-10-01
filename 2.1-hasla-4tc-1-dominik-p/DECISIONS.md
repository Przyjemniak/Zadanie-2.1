# Decyzje projektowe — Haslogen

Każda decyzja zawiera wybraną opcję oraz odrzuconą alternatywę.

## 1. Brak pakietów NuGet

**Wybrane:** wyłącznie biblioteki wbudowane w .NET 8 / WPF (`System.Text.Json`, `System.Security.Cryptography`, schowek WPF).

**Odrzucone:** CommunityToolkit.Mvvm, Newtonsoft.Json, biblioteki UI (MahApps, MaterialDesign). Aplikacja jest mała; dodatkowe zależności nie są potrzebne.

## 2. UI: suwak zamiast spinnera

**Wybrane:** `Slider` 4–64 z etykietą aktualnej wartości.

**Odrzucone:** `IntegerUpDown` (wymagałby pakietu zewnętrznego) albo para przycisków +/-. Suwak jest wbudowany i wystarcza do zakresu 4–64.

## 3. Architektura: code-behind + klasy pomocnicze

**Wybrane:** logika okna w `MainWindow.xaml.cs`, generator / siła / zapis w osobnych klasach statycznych.

**Odrzucone:** pełne MVVM. Dla jednego okna narzut (komendy, binding, ViewModel) nie poprawia czytelności.

## 4. Źródło losowości

**Wybrane:** `RandomNumberGenerator.GetInt32` (kryptograficzne).

**Odrzucone:** `System.Random` — łatwiejsze, ale nieodpowiednie do haseł.

## 5. Gwarancja obecności zestawów

**Wybrane:** najpierw po jednym znaku z każdego włączonego zestawu, reszta z puli łączonej, potem tasowanie Fisher–Yates.

**Odrzucone:** losowanie wyłącznie z puli łączonej. Przy krótkim haśle i kilku zestawach mogłoby zabraknąć np. cyfry mimo zaznaczenia „Cyfry”.

## 6. Zestaw znaków specjalnych

**Wybrane:** `!@#$%^&*()-_=+[]{};:,.?`

**Odrzucone:** pełny zakres ASCII 33–126 albo znaki łatwo mylone w powłoce (`'`, `"`, `\`, `` ` ``, spacja). Wybrany zestaw jest czytelny i zwykle akceptowany przez serwisy.

## 7. Próg siły hasła

Wynik = punkty za długość + liczba włączonych zestawów (1–4).

Punkty za długość:

| Długość | Punkty |
|---------|--------|
| 4–7     | 1      |
| 8–11    | 2      |
| 12–15   | 3      |
| 16–64   | 4      |

Klasyfikacja:

| Suma | Etykieta |
|------|----------|
| ≤ 3  | Słabe    |
| 4–5  | Średnie  |
| ≥ 6  | Silne    |

Przykłady: 8 znaków + 1 zestaw = 3 (słabe); 12 znaków + 2 zestawy = 5 (średnie); 16 znaków + 3 zestawy = 7 (silne).

**Odrzucone:** ocena wyłącznie po długości albo entropia w bitach (mniej czytelna dla użytkownika). Siła jest liczona z **ustawień** (długość + zestawy), nie z konkretnego wylosowanego ciągu — ten sam zestaw opcji daje ten sam wskaźnik.

## 8. Przechowywanie ustawień

**Wybrane:** `%AppData%\Haslogen\settings.json` (UTF-8, wcięty JSON). Zapis przy każdej zmianie suwaka/checkboxa oraz przy zamykaniu okna.

**Odrzucone:** rejestr Windows (trudniejszy backup), `IsolatedStorage` (mniej przejrzyste), plik obok pliku .exe (ginie przy aktualizacji / innym katalogu).

Domyślne ustawienia przy braku pliku: długość 16, wszystkie cztery zestawy włączone.

## 9. Ostatni zestaw nie może zostać wyłączony

**Wybrane:** odznaczenie ostatniego checkboxa ponownie włącza „Małe litery” i pokazuje komunikat.

**Odrzucone:** wyłączanie checkboxów w XAML (`IsEnabled=false`) — gorsze UX przy przełączaniu między zestawami.

## 10. Kopiowanie i stan przycisku „Kopiuj”

**Wybrane:** `Clipboard.SetText`; przycisk nieaktywny, dopóki nie ma wygenerowanego hasła.

**Odrzucone:** automatyczne kopiowanie przy generowaniu (użytkownik może nie chcieć nadpisywać schowka).

## 11. Brak automatycznego generowania przy starcie

**Wybrane:** pole hasła puste do kliknięcia „Generuj”.

**Odrzucone:** hasło od razu przy otwarciu — niepotrzebne zużycie schowka wzrokowego i wrażenie, że aplikacja „coś już zrobiła” bez zgody.

## 12. Hasło w zwykłym polu tekstowym (odczyt)

**Wybrane:** `TextBox` tylko do odczytu, czcionka Consolas.

**Odrzucone:** `PasswordBox` (maskowanie). To generator — użytkownik ma widzieć wynik, żeby je przepisać lub skopiować.
