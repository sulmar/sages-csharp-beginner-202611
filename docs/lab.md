# Projekt laboratorium `Lab` – projekt

Ten dokument definiuje jeden projekt `Lab`, który rośnie przez 12 kroków (patrz [plan.md](plan.md), sekcja 3). Dema z [demos.md](demos.md) używają innych domen i pokazują te same mechanizmy, ale nie są rozwiązaniem `Lab`.

Kod po angielsku, opisy i komunikaty po polsku. Target: .NET 10, C# 14.

## 1. Decyzje

1. **Płatnik: `switch` do kroku 05, interfejs `IDiscountStrategy` od kroku 07.** Kroki 05 i 07 w tabeli są źródłem prawdy.
2. **`Patient` dostaje `Pesel` jako `string` w kroku 04, a jako `readonly struct Pesel` w kroku 06.** Refaktoryzacja jest celowa i pokazana na sali.
3. **Interfejsy w `Lab`:** `IDiscountStrategy` w kroku 07, `IEntityRepository<T>` w kroku 08. `IRenderable` z dema 10 zostaje demo-only.
4. **Zapis JSON nie serializuje obiektów domeny.** `Sample` trzyma strategię rabatu (interfejs), której `System.Text.Json` nie odtworzy. W kroku 10 `SampleDto` przechowuje `PayerType` i mapuje się do domeny przez `DiscountStrategies.For`. To jest punkt do omówienia, nie błąd.
5. **Jedno badanie na próbkę.** Upraszcza model i sumy w raportach.
6. **Cena jest liczona przy rejestracji i zapisywana w `Sample.Price`.** Zmiana cennika nie zmienia istniejących próbek.
7. **Jeśli dema 03 pokazuje `int.TryParse` z `out`, zanim `out` zostanie omówiony w module 05.** Krok 01 używa `int.TryParse(input, out var choice)`. Krok 02 pokazuje `out` jako wzorzec, a krok 06 zamienia walidację na strukturę.

## 2. Model domeny (stan po kroku 12)

Sygnatury zweryfikowane kompilacją na .NET 10 SDK (`Lab.Domain`, bez ostrzeżeń). Implementacja w kroku 12 jest tym samym kodem, co tu, z pełnymi ciałami.

```csharp
namespace Lab.Domain;

public enum Priority { Routine, Urgent, Cito }
public enum PayerType { Private, Nfz, Senior, Company }
public enum SampleStatus { Registered, InAnalysis, Completed, Cancelled }

public abstract class Entity { public int Id { get; set; } }

public readonly struct Pesel : IEquatable<Pesel>      // krok 06; walidacja: tylko długość 11 i cyfry
{
    public Pesel(string value);                       // rzuca ArgumentException, gdy nieprawidłowy
    public static bool IsValid(string? input);
    public static bool TryParse(string? input, out Pesel pesel);
    public string Value { get; }
    public bool Equals(Pesel other);                  // równość po wartości
    public static bool operator ==(Pesel left, Pesel right);
    public static bool operator !=(Pesel left, Pesel right);
}

public class Patient : Entity                         // Pesel: string w kroku 04, Pesel od kroku 06
{
    public Patient(string firstName, string lastName, Pesel pesel);
    public string FirstName { get; }
    public string LastName { get; }
    public Pesel Pesel { get; }
}

public class LabTest
{
    public LabTest(string code, string name, decimal basePrice, string tubeType);
    public string Code { get; }
    public string Name { get; }
    public decimal BasePrice { get; }
    public string TubeType { get; }
}

public static class TestCatalog                       // krok 03: Dictionary<string, decimal>; krok 04: klasa statyczna
{
    public static IReadOnlyDictionary<string, LabTest> Tests { get; }
}

public interface IDiscountStrategy                    // krok 07 (od kroku 05 zastępuje switch po PayerType)
{
    PayerType PayerType { get; }
    string Description { get; }
    decimal Apply(decimal price);
}
public static class DiscountStrategies
{
    public static IDiscountStrategy For(PayerType type);
}
public class NoDiscountStrategy : IDiscountStrategy { }               // prywatnie, bez zniżki
public class NfzDiscountStrategy : IDiscountStrategy { }              // 30 %
public class SeniorDiscountStrategy : IDiscountStrategy { }          // 15 %
public class CompanySubscriptionStrategy : IDiscountStrategy { }     // 10 %

public class Sample : Entity                          // Entity od kroku 08
{
    public Sample(string code, Patient patient, LabTest test, Priority priority, IDiscountStrategy discount, DateTime registeredAt);
    public string Code { get; }                       // "LAB-2026-000123"
    public Patient Patient { get; }
    public LabTest Test { get; }
    public Priority Priority { get; }
    public IDiscountStrategy Discount { get; }
    public DateTime RegisteredAt { get; }
    public decimal Price { get; }                     // liczona w konstruktorze
    public SampleStatus Status { get; }

    public event EventHandler<SampleStatusChangedEventArgs>? StatusChanged;   // krok 09

    public void Start();     // Registered -> InAnalysis
    public void Complete();  // InAnalysis -> Completed
    public void Cancel();    // Registered lub InAnalysis -> Cancelled
}

public class SampleStatusChangedEventArgs : EventArgs  // krok 09
{
    public SampleStatus OldStatus { get; }
    public SampleStatus NewStatus { get; }
}

public class InvalidStatusTransitionException : Exception   // krok 11
{
    public SampleStatus From { get; }
    public SampleStatus To { get; }
}

public interface IEntityRepository<T> where T : Entity   // krok 08
{
    T Add(T entity);                                  // nadaje Id
    T? GetById(int id);
    IReadOnlyList<T> GetAll();
    void Update(T entity);
}
public class InMemoryRepository<T> : IEntityRepository<T> where T : Entity { }

public static class SampleCode                        // krok 11
{
    public static bool IsValid(string? code);         // ^LAB-\d{4}-\d{6}$
    public static string Create(int year, int sequence);   // LAB-2026-000001
}

public static class Label                             // krok 11 (w krokach 04–10: sklejanie stringów)
{
    public static string Build(Sample sample);
}

public static class Reports                           // krok 12
{
    public static IEnumerable<(string Payer, decimal Revenue)> RevenueByPayer(IEnumerable<Sample> samples);
    public static IEnumerable<Sample> CitoSamples(IEnumerable<Sample> samples);
    public static decimal AveragePrice(IEnumerable<Sample> samples);
}
```

Zasady obowiązujące w całym kodzie:
- `Sample` nie ma publicznych setterów. Zmiana statusu wyłącznie przez `Start`, `Complete`, `Cancel`.
- Przychód to suma cen próbek o statusie `Completed`.

## 3. Reguła ceny

```
cena = Round2(cena_bazowa × (1 − zniżka_płatnika)) + dopłata_za_priorytet
```

- **Zniżka płatnika:** prywatnie 0 %, NFZ 30 %, senior 15 %, firma 10 %.
- **Dopłata za priorytet (kwoty stałe):** rutynowe +0,00, pilne +10,00, cito +30,00.
- **Zaokrąglenie:** `Math.Round(x, 2, MidpointRounding.AwayFromZero)`. Zniżka działa przed dopłatą.

Przykłady (wyliczone i sprawdzone programem):

| Badanie | Cena bazowa | Płatnik | Priorytet | Obliczenie | Cena |
|---|---|---|---|---|---|
| MOR | 30,00 | prywatnie | rutynowe | 30,00 + 0 | **30,00** |
| MOR | 30,00 | NFZ | pilne | 30,00 × 0,70 = 21,00, + 10 | **31,00** |
| LIP | 60,00 | senior | cito | 60,00 × 0,85 = 51,00, + 30 | **81,00** |
| GLU | 12,50 | firma | rutynowe | 12,50 × 0,90 = 11,25 | **11,25** |
| GLU | 12,50 | senior | rutynowe | 12,50 × 0,85 = 10,625 → 10,63 | **10,63** |

Ostatni wiersz pokazuje różnicę między `AwayFromZero` a domyślnym bankers rounding (ten drugi dałby 10,62). Dobre na omówienie w kroku 05.

## 4. Statusy

| Z \ Do | Zarejestrowana | W analizie | Zakończona | Anulowana |
|---|---|---|---|---|
| **Zarejestrowana** | – | `Start()` ✔ | ✘ | `Cancel()` ✔ |
| **W analizie** | ✘ | – | `Complete()` ✔ | `Cancel()` ✔ |
| **Zakończona** | ✘ | ✘ | – | ✘ |
| **Anulowana** | ✘ | ✘ | ✘ | – |

- Stany `Zakończona` i `Anulowana` są końcowe.
- W krokach 03–09 nielegalne przejście jest po prostu ignorowane z komunikatem `if`. Od kroku 11 rzucany jest `InvalidStatusTransitionException`.

## 5. Katalog badań – `workshop/data/tests.csv`

Format: kodowanie UTF-8, separator `;`, cena z kropką. Czytanie w kroku 10 przez `CultureInfo.InvariantCulture`.

```csv
kod;nazwa;cena;probowka
MOR;Morfologia krwi;30.00;EDTA
GLU;Glukoza;12.50;szara
CRP;Białko C-reaktywne;25.00;czerwona
KRE;Kreatynina;18.00;żółta
TSH;TSH;45.00;żółta
LIP;Lipidogram;60.00;żółta
PSA;PSA całkowity;55.00;żółta
WIT;Witamina D;90.00;żółta
```

W krokach 03–10 ten sam katalog jest wpisany w kod (`Dictionary<string, decimal>`, potem `TestCatalog`). Krok 10 importuje go z pliku, a import z błędną ceną pokazuje `FormatException`.

## 6. Pesele do ćwiczeń

Walidacja w projekcie sprawdza tylko długość (11 znaków) i cyfry. Data i płeć są tu tylko informacją dla uczestników, nie są sprawdzane.

| PESEL | Data urodzenia (informacyjnie) | Płeć (informacyjnie) |
|---|---|---|
| `80072103403` | 1980-07-21 | kobieta |
| `95120101214` | 1995-12-01 | mężczyzna |
| `00210100714` | 2000-01-01 | mężczyzna |
| `05230705800` | 2005-03-07 | kobieta |
| `75901402110` | 1875-10-14 | mężczyzna |
| `123` | – | – (za krótki, odrzucany) |

## 7. Dane do raportów (krok 12)

Pięć próbek w jednym przebiegu (kolejność rejestracji):

| # | Badanie | Płatnik | Priorytet | Cena | Status |
|---|---|---|---|---|---|
| 1 | MOR | prywatnie | rutynowe | 30,00 | Zakończona |
| 2 | MOR | NFZ | pilne | 31,00 | Zakończona |
| 3 | LIP | senior | cito | 81,00 | W analizie |
| 4 | GLU | firma | rutynowe | 11,25 | Zakończona |
| 5 | GLU | senior | rutynowe | 10,63 | Anulowana |

Oczekiwany wynik raportów:
- **Przychód wg płatnika** (tylko zakończone): NFZ 31,00; prywatnie 30,00; firma 11,25.
- **Próbki „cito”** (bez anulowanych): LIP, 81,00 zł, status W analizie.
- **Średnia cena** wszystkich próbek: (30 + 31 + 81 + 11,25 + 10,63) / 5 = 32,776, pokazana jako `32,78 zł`.

## 8. Kroki 01–12

Każdy krok ma: cel, co powstaje, sesję-akceptację (kryterium „zrobione”) i to, co celowo zostaje brzydkie. Sesje są przykładowe; numery pozycji menu są takie jak w [plan.md](plan.md), sekcja 3.

Menu od kroku 01 (kolejność stała):

```
=== LABORATORIUM ===
1. Zarejestruj próbkę
2. Lista próbek
3. Zmień status próbki
4. Drukuj etykietę
5. Raporty
6. Import katalogu / zapis danych
0. Wyjście
```

### Krok 01 – menu w `while`, `switch`, `int.TryParse`

- **Cel:** pętla menu z obsługą złego wyboru.
- **Powstaje:** `Program.cs` z `while` i `switch`. Opcje 1–6 wypisują „Nie zaimplementowano”.
- **Sesja:**
  ```
  Wybierz opcję: 9
  Nieprawidłowy wybór.
  Wybierz opcję: abc
  Nieprawidłowy wybór.
  Wybierz opcję: 0
  Do widzenia.
  ```
- **Brzydkie:** wszystko w `Program.cs`, liczby jako literały.

### Krok 02 – metody, `out`

- **Cel:** walidacja PESEL bez klas.
- **Powstaje:** `static bool IsValidPesel(string)`: długość 11 i same cyfry. Menu „Zarejestruj” pyta o PESEL i odrzuca zły.
- **Sesja:**
  ```
  Podaj PESEL: 80072103403
  PESEL poprawny.
  Podaj PESEL: 123
  Błąd: nieprawidłowy PESEL.
  ```
- **Brzydkie:** nic. Pętla sprawdzająca cyfry (`char.IsDigit`) jest prosta i wystarczy.

### Krok 03 – tablice i kolekcje

- **Cel:** rejestracja próbki bez klas.
- **Powstaje:** `Dictionary<string, decimal>` z katalogiem, `List<string>` z próbkami. Rejestracja pyta o kod badania i PESEL (bez priorytetu i płatnika, to dochodzi w kroku 05).
- **Sesja:**
  ```
  Kod badania: MOR
  PESEL: 80072103403
  Zarejestrowano LAB-2026-000001, badanie MOR, cena 30.00
  Lista próbek:
    LAB-2026-000001 MOR 30.00
  ```
- **Brzydkie:** próbka to napis `"LAB-2026-000001|MOR|30.00"`, rozbijany przez `Split('|')`. Celowo.

### Krok 04 – klasy, właściwości, konstruktory, `static`

- **Cel:** refaktoryzacja na klasy. Zachowanie musi być identyczne z krokiem 03.
- **Powstaje:** `Lab.Domain` (biblioteka), `Patient`, `Sample`, `LabTest`, `TestCatalog` jako klasa statyczna. `Patient.Pesel` jest jeszcze `string`.
- **Sesja (akceptacja):** te same wejścia i wyjścia co w kroku 03. Pokazujemy `git diff`-owi uczestników, że wynik się nie zmienił.
- **Brzydkie:** `Sample` nie ma jeszcze priorytetu ani płatnika.
- **Kompozycja:** `Sample` ma pola `Patient` i `LabTest` (próbka *ma* pacjenta i badanie, nie *jest* pacjentem). Nie dziedziczy po nich. Kontrast z dziedziczeniem z modułu 10.

### Krok 05 – `enum`, `switch`

- **Cel:** priorytet i płatnik jako `enum`, cena przez `switch`.
- **Powstaje:** `Priority`, `PayerType`, `SampleStatus`. Dopłata za priorytet i zniżka płatnika w wyrażeniach `switch` w `Sample`.
- **Sesja:**
  ```
  Kod badania: MOR
  PESEL: 80072103403
  Priorytet (0 rutynowe, 1 pilne, 2 cito): 1
  Płatnik (0 prywatnie, 1 NFZ, 2 senior, 3 firma): 1
  Zarejestrowano LAB-2026-000002, cena 31.00
  ```
- **Brzydkie (celowo):** `switch` po `PayerType` w `Sample.CalculatePrice`. Zostaje do kroku 07.

### Krok 06 – `struct`, równość, operatory, `ToString`

- **Cel:** PESEL jako typ wartościowy.
- **Powstaje:** `readonly struct Pesel` z `IsValid`, `TryParse`, `Value`, `Equals`, `==`, `ToString`. `Patient.Pesel` zmienia typ na `Pesel`. Walidacja z kroku 02 przechodzi do struktury.
- **Sesja:**
  ```
  Podaj PESEL: 123
  Błąd: nieprawidłowy PESEL.
  Podaj PESEL: 80072103403
  Te same PESEL-e są równe: True
  ```
- **Brzydkie:** nic. Walidacja jest celowo prosta (długość i cyfry), więc uwaga skupia się na typie wartościowym.

### Krok 07 – interfejsy, polimorfizm

- **Cel:** zastąpić `switch` po płatniku wymienialnymi strategiami rabatu.
- **Powstaje:** `interface IDiscountStrategy` (`Description`, `Apply`) i cztery klasy rabatów: `NoDiscountStrategy`, `NfzDiscountStrategy`, `SeniorDiscountStrategy`, `CompanySubscriptionStrategy`. `DiscountStrategies.For(PayerType)` wybiera strategię z menu. `Sample` przechowuje strategię, nie `PayerType`.
- **Sesja:** wartości jak w kroku 05, bez zmian: `MOR, NFZ, pilne → 31,00`; `LIP, senior, cito → 81,00`.
- **Brzydkie:** nic celowo; to krok, w którym kod wreszcie jest „czysty”. Dodanie nowego płatnika to nowa klasa, bez zmiany `Sample`.

### Krok 08 – typy generyczne

- **Cel:** wspólna baza encji i repozytorium.
- **Powstaje:** `abstract class Entity` (z `Id`), `interface IEntityRepository<T>`, `InMemoryRepository<T> where T : Entity`. `Patient` i `Sample` dziedziczą po `Entity`. Repozytorium nadaje `Id`.
- **Sesja:**
  ```
  Dodano pacjenta Id=1
  Zarejestrowano próbkę Id=1, LAB-2026-000001
  Lista próbek:
    [1] LAB-2026-000001 Morfologia krwi 30,00 zł Zarejestrowana
  ```
- **Brzydkie:** `Id` nadawane przez licznik w repozytorium, bez sprawdzania duplikatów.

### Krok 09 – delegaty, zdarzenia, lambdy

- **Cel:** zmiana statusu rozgłasza zdarzenie.
- **Powstaje:** `Sample.StatusChanged`, `SampleStatusChangedEventArgs`. Menu „Zmień status” wywołuje `Start`, `Complete` lub `Cancel`. Subskrybenci: lambda „SMS” i lambda „dziennik”.
- **Sesja:**
  ```
  Id próbki: 1
  1 rozpocznij analizę, 2 zakończ, 3 anuluj: 1
  [SMS] Pacjent Jan Kowalski: próbka LAB-2026-000001 jest w analizie.
  [Dziennik] Registered -> InAnalysis
  ```
- **Brzydkie:** SMS to `Console.WriteLine`. Nie ma prawdziwej wysyłki, celowo.

### Krok 10 – wejście/wyjście, JSON

- **Cel:** import katalogu, zapis i odczyt próbek.
- **Powstaje:** `TestCatalogLoader` (CSV z `InvariantCulture`), `SampleDto` i zapis/odczyt `data/samples.json`. Mapowanie `PayerType` na strategię przez `DiscountStrategies.For`.
- **Sesja (import i zapis):**
  ```
  Zaimportowano 8 badań z data/tests.csv
  Zapisano 1 próbkę do data/samples.json
  ```
- **Brzydkie:** nic. Błędna cena w `tests.csv` daje `FormatException`, to celowy przykład.

### Krok 11 – wyjątki i etykieta (napisy)

- **Cel:** nielegalne przejścia i złe dane wejściowe jako wyjątki, etykieta próbki i walidacja kodu.
- **Powstaje:** `InvalidStatusTransitionException`, `ArgumentException` przy nieznanym kodzie badania, `try`/`catch` w menu, `finally` przy wyjściu z programu. Także `Label.Build` (`StringBuilder`, `ToString("C", pl-PL)`) i `SampleCode.IsValid` (regex `^LAB-\d{4}-\d{6}$`).
- **Sesja (wyjątki):**
  ```
  Id próbki: 1
  1 rozpocznij analizę, 2 zakończ, 3 anuluj: 2
  Błąd: Nie można przejść ze statusu Registered do Completed.
  Kod badania: XYZ
  Błąd: nieznany kod badania.
  Zamykam aplikację.
  ```
- **Sesja (etykieta):**
  ```
  Id próbki: 1
  ==============================
  LAB-2026-000001
  Pacjent: Jan Kowalski
  Badanie: Morfologia krwi
  Probówka: EDTA
  Cena: 30,00 zł
  ==============================
  ```
- **Brzydkie:** `catch (Exception)` w menu łapie wszystko. Zostaje, bo tu omawiamy, czego nie robić.

### Krok 12 – LINQ, raporty

- **Cel:** raporty na liście próbek.
- **Powstaje:** `Reports.RevenueByPayer`, `Reports.CitoSamples`, `Reports.AveragePrice`. Menu „Raporty” wypisuje trzy sekcje.
- **Sesja (dane z sekcji 7):**
  ```
  Przychód wg płatnika (tylko zakończone):
    NFZ        31,00 zł
    Prywatnie  30,00 zł
    Firma      11,25 zł
  Próbki cito:
    LAB-2026-000003  LIP  81,00 zł  W analizie
  Średnia cena: 32,78 zł
  ```
- **Brzydkie:** nic, poza tym że `AveragePrice` dla pustej listy zwraca 0 (sprawdzamy to w kodzie, nie wyjątkiem).

## 9. Układ plików

```
workshop/
├─ README.md               # historia, lista kroków, PESEL-e z sekcji 6
├─ data/
│  ├─ tests.csv            # sekcja 5
│  └─ samples.json         # powstaje w kroku 10
├─ Lab/
│  ├─ Lab.Domain/          # od kroku 04
│  └─ Lab.ConsoleApp/      # menu, od kroku 01
└─ checkpoints/
   ├─ step-01-menu/
   ├─ step-02-methods/
   ├─ ...
   └─ step-14-linq-reports/
```

Punkty kontrolne to foldery, a nie gałęzie git, zgodnie z planem.

## 10. Czego nie ma (celowo)

- Nazwane wzorce projektowe (Decorator, Visitor, Proxy). Strategia pojawia się jako zwykły interfejs `IDiscountStrategy`. Stateless, mediator, `Channel<T>`, Cofnij/Ponów.
- Drukarka, GUI, DI, asynchroniczność.
- NIP jako zadanie dodatkowe (opisane w `plan.md`, sekcja 6, punkt 2).
- Refleksja, `dynamic`, logowanie, liczniki, P/Invoke, Event Log (poza zakresem kursu).
