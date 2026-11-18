# Ćwiczenie: Rejestr przesyłek

**Czas:** około 45–60 minut.

Utwórz od zera nowy projekt aplikacji konsolowej w C#. Program ma przechowywać przesyłki, obliczać koszty ich wysyłki i umożliwiać oznaczanie ich jako nadane.

To rozwinięcie kalkulatora z pierwszego dnia — tym razem wykorzystaj kolekcję obiektów i programowanie obiektowe.

## Wymagania

1. Wyświetl menu:

   ```text
   === REJESTR PRZESYŁEK ===
   1. Dodaj przesyłkę
   2. Wyświetl wszystkie przesyłki
   3. Nadaj przesyłkę
   4. Wyświetl podsumowanie
   0. Zakończ
   ```

2. Utwórz klasę `Shipment`, która przechowuje:

   - numer przesyłki,
   - nazwę odbiorcy,
   - wagę w kilogramach,
   - informację o dostawie ekspresowej,
   - informację, czy przesyłka została nadana.

   Dane początkowe przekaż przez konstruktor. Nowa przesyłka ma status „oczekująca”. Numer i status nie mogą być dowolnie zmieniane poza klasą.

3. Dodaj do klasy metody:

   - `CalculateCost()` — zwraca koszt wysyłki jako `decimal`,
   - `Send()` — oznacza przesyłkę jako nadaną; zwraca `false`, jeśli była już nadana, a `true` po udanej zmianie statusu.

   Cena podstawowa zależy od wagi:

   | Waga | Cena |
   |---|---:|
   | Do 1 kg włącznie | 10 zł |
   | Powyżej 1 kg, do 5 kg włącznie | 15 zł |
   | Powyżej 5 kg, do 20 kg włącznie | 25 zł |

   Dostawa ekspresowa kosztuje dodatkowo 10 zł.

4. Przechowuj przesyłki w kolekcji `List<Shipment>`. Przy dodawaniu:

   - zapytaj o odbiorcę, wagę i sposób dostawy,
   - nadaj automatycznie kolejny numer, zaczynając od `1`,
   - utwórz obiekt i dodaj go do kolekcji dopiero po sprawdzeniu wszystkich danych.

   Odbiorca nie może być pusty. Waga musi być większa od zera i nie większa niż 20 kg. Sposób dostawy wybierz jako `1` — standardowa lub `2` — ekspresowa.

5. Wyświetlając przesyłki, pokaż numer, odbiorcę, wagę, sposób dostawy, koszt i status. Jeśli kolekcja jest pusta, wyświetl odpowiedni komunikat.

6. Przy nadawaniu zapytaj o numer przesyłki i odszukaj obiekt w kolekcji. Obsłuż brak takiego numeru oraz próbę ponownego nadania. Nadanej przesyłki nie usuwaj z listy.

7. W podsumowaniu wyświetl:

   - liczbę wszystkich przesyłek,
   - liczbę przesyłek oczekujących i nadanych,
   - łączny koszt wysyłki wszystkich przesyłek.

   Dla pustej kolekcji wyświetl zera.

8. Po wykonaniu operacji wróć do menu. Obsłuż błędne dane komunikatem, bez zmieniania kolekcji ani stanu przesyłek. Opcja `0` kończy program.

## Wskazówki

- Wykorzystaj `List<Shipment>`, pętlę `foreach` oraz `TryParse`.
- Kwoty przechowuj jako `decimal`.
- Obliczanie ceny i zmiana statusu należą do klasy `Shipment`.
- Wczytywanie danych i wyświetlanie menu pozostaw w `Program`.
- Do ograniczenia możliwości zmiany właściwości wykorzystaj `get` lub `private set`.
- Zadanie wykonaj bez LINQ i zapisu do pliku — dane są przechowywane tylko podczas działania programu.

## Przykład działania

```text
Wybierz opcję: 1
Odbiorca: Anna Kowalska
Waga w kg: 3
Sposób dostawy (1 — standardowa, 2 — ekspresowa): 2

Dodano przesyłkę nr 1. Koszt wysyłki: 25,00 zł.

Wybierz opcję: 2
Nr 1 | Anna Kowalska | 3 kg | ekspresowa | 25,00 zł | oczekująca

Wybierz opcję: 3
Numer przesyłki: 1
Przesyłka nr 1 została nadana.

Wybierz opcję: 4
Wszystkie przesyłki: 1
Oczekujące: 0
Nadane: 1
Łączny koszt wysyłki: 25,00 zł
```

## Sprawdź swoje rozwiązanie

| Scenariusz | Oczekiwany wynik |
|---|---|
| Wyświetlenie pustej listy | Komunikat o braku przesyłek |
| Dodanie 1 kg, standardowa | Koszt 10 zł |
| Dodanie 5 kg, ekspresowa | Koszt 25 zł |
| Dodanie 20 kg, standardowa | Koszt 25 zł |
| Waga `0`, `-2`, `21` lub `abc` | Błąd, bez dodania przesyłki |
| Pusty odbiorca lub dostawa `9` | Błąd, bez dodania przesyłki |
| Nadanie istniejącej przesyłki | Zmiana statusu na „nadana” |
| Ponowne nadanie tej samej przesyłki | Komunikat, bez zmiany stanu |
| Nadanie nieistniejącego numeru | Komunikat o braku przesyłki |

Dodaj przesyłki za 10 zł i 25 zł, a następnie nadaj jedną z nich. Podsumowanie powinno pokazać: **2 przesyłki, 1 oczekująca, 1 nadana, łączny koszt 35 zł**.

## Dla chętnych

Zastąp flagę dostawy ekspresowej dziedziczeniem:

- utwórz klasy `StandardShipment` i `ExpressShipment` dziedziczące po `Shipment`,
- zastosuj `virtual` i `override` przy obliczaniu kosztu,
- przechowuj oba rodzaje przesyłek w jednej kolekcji `List<Shipment>`.

Sprawdź, czy wywołanie `CalculateCost()` w pętli zwraca właściwą cenę dla każdego rodzaju przesyłki, bez sprawdzania jej typu w `Program`.
