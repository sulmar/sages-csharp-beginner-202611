using Lab.Domain;

Console.WriteLine("Hello, Lab!");


Sample sample = new Sample("ABC");

Console.WriteLine(sample);

// sample.Start();
Console.WriteLine(sample);
// sample.Complete();
Console.WriteLine(sample);

Console.WriteLine(sample.Status); // Get


return;



LabTest labTest1 = new LabTest("MOR", "Morfologia krwi", 50m); // tworzy obiekt klasy LabTest
LabTest labTest2 = new LabTest("GLU", "Glukoza", 30m); // tworzy obiekt klasy LabTest

Console.WriteLine(labTest1.Code);
Console.WriteLine(labTest1.Price);

Console.WriteLine(labTest2.Code);
Console.WriteLine(labTest2.Price);




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
    4. Wyświetl katalog badań z cenami    
    0. Wyjście
    Wybierz opcję:
    """;

// Tablica kodów badań
/// string[] testCodes = { "MOR", "GLU", "CRP", "KRE", "LIP" };

// Słownik: klucz - wartosc słownik nazw badań (kod badania - nazwa badania)
Dictionary<string, LabTest> labTests = new()   // new() - konstruktor
{
    ["MOR"] = new LabTest("MOR", "Morfologia krwi", 50m),
    ["GLU"] = new LabTest("GLU", "Glukoza", 30m),
    ["CRP"] = new LabTest("CRP", "Białko C-reaktywne", 40m),
    ["KRE"] = new LabTest("KRE", "Kreatynina", 35m),
    ["LIP"] =  new LabTest("LIP", "Lipidogram", 60m)
};

// Lista wybranych badań (kodów badań) - przechowuje kody badań wybrane przez użytkownika
List<LabTest> selectedTests = new();


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
        case "4":
            PrintLabTests();
            break;        
        case "0":
            PrintOrder();
            break;
        default:
            Console.WriteLine("Nieprawidłowa opcja. Spróbuj ponownie.");
            break;
    }
}


void PrintOrder()
{
    Console.WriteLine("Wybrane badania:");

    foreach (var labTest in selectedTests)
    {
        Console.WriteLine($"Kod badania: {labTest.Code} Nazwa: {labTest.Name}");
    }
}


//void PrintStringDictionary(Dictionary<string, string> dictionary)
//{
//    foreach (var item in dictionary)
//    {
//        Console.WriteLine($"{item.Key}: {item.Value}");
//    }
//}

//void PrintDecimalDictionary(Dictionary<string, decimal> dictionary)
//{
//    foreach (var item in dictionary)
//    {
//        Console.WriteLine($"{item.Key}: {item.Value}");
//    }
//}


void PrintLabTests()
{
    foreach (var labTest in labTests)
    {
        Console.WriteLine($"{labTest.Value.Code} {labTest.Value.Name} {labTest.Value.Price}");
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

    string testCode = Ask("Kod badania: ");

    LabTest labTest = null;

    if (labTests.ContainsKey(testCode)) // Sprawdzenie czy słownik zawiera klucz testCode
    {
        labTest = labTests[testCode];        

        Console.WriteLine($"Wybrano badanie: {labTest.Name} (Kod: {labTest.Code}) Cena: {labTest.Price}");
    }
    else
    {
        Console.WriteLine("Nieznany kod badania.");
    }

    selectedTests.Add(labTest); // Dodanie badania do listy wybranych badań

    if (priority == Priority.Cito)
    {
        Console.WriteLine("Uwaga: Priorytet CITO wymaga natychmiastowej obsługi!");
    }

    decimal cost = 0m;

    if (payerType == PayerType.Private)
    {
        // cost = CalculateCost(100m, 7); // Cena netto 100 zł, stawka VAT 7%

        cost = CalculateCost(vatRate: 7, netPrice: labTest.Price); // Nazwane parmetry - pozwala na przekazanie argumentów w dowolnej kolejności, ale wymaga podania nazwy parametru.

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
string Ask(string prompt, string defaultValue = "01234567891")
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write(prompt);
    string? response = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(response))
    {
        response = defaultValue;
    }

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