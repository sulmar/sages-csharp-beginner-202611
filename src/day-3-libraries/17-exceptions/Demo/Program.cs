using Demo;

Console.WriteLine("Hello, Exceptions!");

try
{
    Shipment shipment = new Shipment(1, "a", 21);
}
catch(Exception e)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(e.Message);
    Console.ResetColor();

    File.AppendAllText("log.txt", $"{DateTime.Now}|Error|{e.Message}\n");
}

return;

const string TemporaryFileName = "temp.tmp";

try
{
    File.Create(TemporaryFileName).Dispose(); // Dispose - zamyka plik

    decimal amount = decimal.Parse("-1"); // FormatException : Exception

    if (amount <= 0)
        throw new ArgumentOutOfRangeException("amount", "Kwota nie może być ujemna"); // Wygenerowanie wyjątku

    File.WriteAllText(TemporaryFileName, $"amount:{amount}");
      
    File.ReadAllText("plik1.txt");
    File.ReadAllText(TemporaryFileName);

    Console.WriteLine($"Kwota {amount}"); // FileNotFoundException : Exception
}

catch(FormatException) // blok, który wykona się gdy wystąpi wyjątek FormatException
{
    Console.WriteLine("Błąd: nieprawidłowa kwota.");
}

catch(FileNotFoundException e) // blok, który wykona się gdy wystąpi wyjątek FileNotFoundException
{    
    Console.WriteLine($"Błąd: plik {e.FileName} nie istnieje.");
}
catch(ArgumentOutOfRangeException e)
{
    Console.WriteLine($"{e.ParamName}: {e.Message}");
}
catch(Exception e)
{
    Console.WriteLine($"Nieobsłużony wyjątek: {e.Message}");
}

finally // blok, który wykona się zawsze niezależnie od tego czy wystąpi wyjątek (wspólnych dla wszystkich wyjątków)
{
    File.Delete(TemporaryFileName); // Czyscimy zasoby
}


