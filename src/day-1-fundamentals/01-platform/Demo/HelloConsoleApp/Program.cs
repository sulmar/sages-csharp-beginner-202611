Console.WriteLine("Hello, World!"); 
Console.Write("Podaj imię: ");

string name = Console.ReadLine(); // Odczyt konsoli

Console.WriteLine($"Witaj, {name}");

Console.WriteLine(7 / 2); // Python 3.5 

Console.WriteLine(7 / 2.0);
Console.WriteLine(7 / 2d); // (d)ecimal
Console.WriteLine(7d / 2); // (d)ecimal

Console.WriteLine(7 / 2f); // (f)loat
Console.WriteLine(7 / 2m); // double

int age = 30;
// age = "osiemnascie"; //  Error CS0029 : Nie można niejawnie przekonwertować typu „string” na „int”.

Console.WriteLine($"Wiek: {age}");

int x; // Warning CS8600 : Konwertowanie literału null lub możliwej wartości null na nienullowalny typ.
// Console.WriteLine(x);  // Error CS0165 : Użyto nieprzypisanej zmiennej lokalnej „x”

var y = 5; // var w c# nie oznacza dowolnego typu. Typ jest wnioskowany przed kompilator na podstawie przypisanej wartości
// y = "a"; // Error CS0029 : Nie można niejawnie przekonwertować typu „string” na „int”.

Console.WriteLine(y);





