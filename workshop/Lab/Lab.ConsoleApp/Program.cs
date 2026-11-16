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
    0. Wyjście
    Wybierz opcję:
    """;

Console.Write(Menu);


string input = Console.ReadLine();


// Wybór opcji za pomocą instrukcji if-else

/*
    if (input == "1")
    {
        Console.WriteLine("Wybrano opcję 1: Zarejestruj próbkę");
    }
    else if (input == "2")
    {
        Console.WriteLine("Wybrano opcję 2: Lista próbek");
    }
    else if (input == "0")
    {
        Console.WriteLine("Wybrano opcję 0: Wyjście");
    }
    else
    {
        Console.WriteLine("Nieprawidłowa opcja. Spróbuj ponownie.");
    }
*/

// Wybór opcji za pomocą instrukcji switch
switch (input)
{
    case "1":
        RegisterSample();
        break;
    case "2":
        Console.WriteLine("Wybrano opcję 2: Lista próbek");
        break;
    case "0":
        Console.WriteLine("Wybrano opcję 0: Wyjście");
        break;
    default:
        Console.WriteLine("Nieprawidłowa opcja. Spróbuj ponownie.");
        break;
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

    // TODO: Dodaj zapis do bazy danych lub innej logiki rejestracji próbki tutaj

    Console.WriteLine($"Zarejestrowano próbkę dla pacjenta: {firstName} {lastName} (PESEL: {pesel})");
}

// DRY - Don't Repeat Yourself (nie powtarzaj się)
// - zasada programowania, która mówi, że nie należy powtarzać tego samego kodu w wielu miejscach.
// Zamiast tego, należy tworzyć funkcje lub metody, które można wielokrotnie wykorzystywać.
// W tym przypadku metoda Ask() jest przykładem zastosowania tej zasady, ponieważ pozwala na wielokrotne pobieranie danych od użytkownika bez powtarzania kodu.

// Metoda do wyświetlania komunikatu i pobierania danych od użytkownika. Przyjmuje parametr tekstowu. Zwraca wprowadzony tekst.
string Ask(string prompt)
{
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.Write(prompt);
    string result = Console.ReadLine() ?? "";
    Console.ResetColor();

    return result;
}

// Logika walidacji PESEL - sprawdzenie długości. Zwraca true jeśli PESEL jest poprawny, false w przeciwnym wypadku.
bool IsValidPesel(string pesel)
{
    if (pesel.Length != 11)
        return false;
    else
        return true;
}


