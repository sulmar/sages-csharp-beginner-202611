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
        Console.WriteLine("Wybrano opcję 1: Zarejestruj próbkę");
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









