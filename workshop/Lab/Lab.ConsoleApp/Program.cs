using System.Collections.Concurrent;

Console.WriteLine("Hello, Lab!");

/*
Console.WriteLine("=== LABORATORIUM ===");
Console.WriteLine("1. Zarejestruj próbkę");
Console.WriteLine("2. Lista próbek");
Console.WriteLine("0. Wyjście");
Console.WriteLine("Wybierz opcję:");
*/

const string Menu = """
    === LABORATORIUM ===
    1. Zarejestruj próbkę
    2. Lista próbek
    3. Ustal lokalizację pacjenta
    0. Wyjście
    Wybierz opcję:
    """;

while (true)
{

    Console.Write(Menu);

    string input = Console.ReadLine();

    Handle(input);

}


// Wybór opcji za pomocą instrukcji switch
void Handle(string choice)
{
    switch (choice)
    {
        case "1":
            RegisterSample();
            break;
        case "2":
            Console.WriteLine("Wybrano opcję 2: Lista próbek");
            break;
        case "3":
            var location = GetLocation();
            location.lon += 0.1f; // Przesunięcie lokalizacji o 0.1 stopnia w prawo (na wschód)
            Console.WriteLine($"Wybrano opcję 3: Ustal lokalizację pacjenta. Lokalizacja: ({location.lat}, {location.lon})");
            break;
        case "0":
            Console.WriteLine("Wybrano opcję 0: Wyjście");
            break;
        default:
            Console.WriteLine("Nieprawidłowa opcja. Spróbuj ponownie.");
            break;
    }
}


// Metoda do rejestracji próbki. void oznacza ze metoda (funkcja) nie zwraca żadnej wartości.
void RegisterSample()
{
    Console.WriteLine("Wybrano opcję 1: Zarejestruj próbkę");

    var pesel = Ask("Podaj PESEL pacjenta: ");

    // TODO: Dodaj walidację PESEL tutaj (np. sprawdzenie długości 11 znaków i czy zawiera tylko cyfry itp.)

    // SOLID - Single Responsibility Principle (zasada pojedynczej odpowiedzialności) - metoda powinna mieć jedną odpowiedzialność.
    // W tym przypadku metoda RegisterSample() zajmuje się rejestracją próbki.
    if (!IsValidPesel(pesel))
    {
        Console.WriteLine("Nieprawidłowy format PESEL. Proszę wprowadzić 11 cyfr.");
        return; // Zakończ metodę, jeśli PESEL jest nieprawidłowy
    }

    var firstName = Ask("Imię pacjenta: ");
    var lastName = Ask("Nazwisko pacjenta: ");

    Priority priority = AskPrority("Priorytet (0) rutynowy, (1) pilny, (2) cito: ");

    PayerType payerType = AskPayerType("Typ płatnika (0) prywatny, (1) NFZ, (2) ubezpieczenie: ");

    if (priority == Priority.Cito)
    {
        Console.WriteLine("Uwaga: Priorytet CITO wymaga natychmiastowej obsługi!");
    }

    decimal cost = 0m;

    if (payerType == PayerType.Private)
    {
        // cost = CalculateCost(100m, 7); // Cena netto 100 zł, stawka VAT 7%

        cost = CalculateCost(netPrice: 100m); // Nazwane parmetry - pozwala na przekazanie argumentów w dowolnej kolejności, ale wymaga podania nazwy parametru.

    }

    // TODO: Dodaj zapis do bazy danych lub innej logiki rejestracji próbki tutaj

    Console.WriteLine($"Zarejestrowano próbkę dla pacjenta: {firstName} {lastName} (PESEL: {pesel}) Priorytet: {priority} Typ płatnika: {payerType} Koszt: {cost}");
}


decimal CalculateCost(decimal netPrice, int vatRate = 23) // vatRate ma domyślną wartość 23, jeśli nie zostanie podana inna wartość
{
    return netPrice * (1 + vatRate / 100m); // Obliczanie ceny brutto na podstawie ceny netto i stawki VAT
}

// DRY - Don't Repeat Yourself (nie powtarzaj się)
// - zasada programowania, która mówi, że nie należy powtarzać tego samego kodu w wielu miejscach.
// Zamiast tego, należy tworzyć funkcje lub metody, które można wielokrotnie wykorzystywać.
// W tym przypadku metoda Ask() jest przykładem zastosowania tej zasady, ponieważ pozwala na wielokrotne pobieranie danych od użytkownika bez powtarzania kodu.

// Metoda do wyświetlania komunikatu i pobierania danych od użytkownika. Przyjmuje parametr tekstowu. Zwraca wprowadzony tekst.
string Ask(string prompt)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write(prompt);
    string response = Console.ReadLine() ?? "";
    Console.ResetColor();

    return response;
}


// Nazwa funkcji nie może sie powtarzać
/*
int AskInt(string prompt)
{
    string response = Ask(prompt);

    int result = int.Parse(response);

    return result;
}
*/

/*
int AskInt(string prompt)
{    
    return int.Parse(Ask(prompt));     // Złożenie funkcji - f(x) = g(h(x)) - funkcja złożona. W tym przypadku funkcja AskInt() wywołuje funkcję Ask() i przekazuje jej wynik do funkcji int.Parse().
}
*/

int AskInt(string prompt) => int.Parse(Ask(prompt)); // Wyrażenie lambda (funkcja anonimowa) - skrócony zapis metody

Priority AskPrority(string prompt)
{
    Priority priority;

    do
    {
        priority = (Priority)AskInt(prompt);

        if (!Enum.IsDefined(priority))
        {
            Console.WriteLine("Nieprawidłowy priorytet. Spróbuj ponownie.");
        }
    }
    while (!Enum.IsDefined(priority));

    return priority;
}

PayerType AskPayerType(string prompt)
{
    PayerType payerType;

    do
    {
        payerType = (PayerType)AskInt(prompt);

        if (!Enum.IsDefined(payerType))
        {
            Console.WriteLine("Nieprawidłowy typ płatnika. Spróbuj ponownie.");
        }
    }
    while (!Enum.IsDefined(payerType));

    return payerType;
}


// Logika walidacji PESEL - sprawdzenie długości . Zwraca true jeśli PESEL jest poprawny, false w przeciwnym wypadku.
bool IsValidPesel(string pesel) => pesel.Length == 11 && pesel.All(char.IsAsciiDigit); // Wyrażenie lambda (funkcja anonimowa) - skrócony zapis metody

// Zwraca Struct
Location GetLocation2()
{
    return new Location(21.90f, 51.10f, 0f); // Zwraca obiekt struktury Location z trzema wartościami typu float
}


// Zwraca Tuple 
(float lat, float lon, float alt) GetLocation()
{
    return (21.90f, 51.10f, 0f); // Zwraca krotkę (tuple) z trzema wartościami typu float
}


// Struct - struktura danych, która może przechowywać różne typy danych. W tym przypadku struktura Location przechowuje trzy wartości typu float: lat, lon i alt.
struct Location
{
    public float lat;
    public float lon;
    public float alt;
    public Location(float lat, float lon, float alt)
    {
        this.lat = lat;
        this.lon = lon;
        this.alt = alt;
    }
}

// Typ wyliczeniowy
enum Priority
{
    Routine = 0,
    Urgent = 1,
    Cito = 2
}

// Typ wyliczeniowy
enum PayerType
{
    Private,
    NFZ,
    Insurance
}