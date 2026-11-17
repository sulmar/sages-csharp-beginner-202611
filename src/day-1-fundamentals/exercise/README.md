# Ćwiczenie: Kalkulator przesyłek

**Czas:** około 30–45 minut.

Utwórz od zera nowy projekt aplikacji konsolowej w C#. Program ma obliczać koszt wysyłki paczki na podstawie jej wagi i wybranego sposobu dostawy.

## Wymagania

1. Wyświetl menu:

   ```text
   === KALKULATOR PRZESYŁEK ===
   1. Oblicz koszt wysyłki
   0. Zakończ
   ```

2. Po wybraniu opcji `1` zapytaj o wagę paczki w kilogramach. Cena podstawowa wynosi:

   | Waga | Cena |
   |---|---:|
   | Do 1 kg włącznie | 10 zł |
   | Powyżej 1 kg, do 5 kg włącznie | 15 zł |
   | Powyżej 5 kg, do 20 kg włącznie | 25 zł |

   Waga musi być większa od zera. Paczki powyżej 20 kg nie są obsługiwane.

3. Zapytaj o sposób dostawy:
   - `1` — standardowa: bez dopłaty,
   - `2` — ekspresowa: dopłata 10 zł.

4. Wyświetl cenę podstawową, dopłatę i łączny koszt wysyłki.
5. Po obliczeniu wróć do menu. Opcja `0` kończy program.
6. Obsłuż błędne dane, np. tekst zamiast wagi, ujemną wagę i nieistniejącą opcję menu lub dostawy. Wyświetl komunikat i wróć do menu, bez wykonywania wyceny.

## Wskazówki

- Wykorzystaj zmienne, instrukcje `if`, `switch` i pętlę.
- Kwoty przechowuj jako `decimal`.
- Do sprawdzania danych liczbowych możesz użyć `TryParse`.
- Wydziel przynajmniej jedną metodę obliczającą cenę.
- Zadanie wykonaj bez kolekcji.

## Przykład działania

```text
=== KALKULATOR PRZESYŁEK ===
1. Oblicz koszt wysyłki
0. Zakończ

Wybierz opcję: 1
Podaj wagę paczki w kg: 3
Sposób dostawy (1 — standardowa, 2 — ekspresowa): 2

Cena podstawowa: 15,00 zł
Dopłata: 10,00 zł
Łączny koszt: 25,00 zł
```

## Sprawdź swoje rozwiązanie

| Waga | Dostawa | Oczekiwany wynik |
|---|---|---|
| 1 kg | Standardowa | 10 zł |
| 5 kg | Ekspresowa | 25 zł |
| 20 kg | Standardowa | 25 zł |
| 0 kg | — | Komunikat o błędzie |
| -2 kg | — | Komunikat o błędzie |
| 21 kg | — | Komunikat o błędzie |
| `abc` | — | Komunikat o błędzie |
| 3 kg | Nieistniejąca opcja `9` | Komunikat o błędzie, bez wyceny |

Sprawdź także, czy można wykonać kilka kolejnych wycen bez ponownego uruchamiania programu oraz czy opcja `0` kończy jego działanie.

## Dla chętnych

Przy zakończeniu programu wyświetl liczbę poprawnie wykonanych wycen i ich łączną wartość.

- Zwiększaj licznik i sumę tylko po poprawnym wykonaniu wyceny.
- Błędne dane nie mogą zmieniać licznika ani sumy.
- Jeśli nie wykonano żadnej wyceny, wyświetl liczbę `0` i wartość `0,00 zł`.

Przykład po wykonaniu wycen na 10 zł i 25 zł:

```text
Liczba poprawnie wykonanych wycen: 2
Łączna wartość wycen: 35,00 zł
Do widzenia!
```
