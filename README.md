# Programowanie .NET – kurs podstawowy

Materiały do trzydniowego szkolenia z C# i .NET 10 (LTS). To repo startowe: każdy moduł ma opis i puste foldery na demo i ćwiczenia, które wypełniamy na zajęciach.

## Agenda

| Dzień | Temat |
|---|---|
| 1 | Platforma .NET, Visual Studio, składnia, typy proste, metody, tablice i kolekcje, napisy |
| 2 | Programowanie obiektowe: struktury, klasy, dziedziczenie, interfejsy, generyki, delegaty i zdarzenia |
| 3 | Wejście/wyjście, wyjątki, LINQ |

## Struktura

| Katalog | Zawartość |
|---|---|
| `src/day-1-fundamentals/` | Moduły z pierwszego dnia |
| `src/day-2-oop/` | Moduły z programowania obiektowego |
| `src/day-3-libraries/` | Moduły z bibliotek (I/O, wyjątki, LINQ) |
| `workshop/Lab/` | Projekt warsztatowy `Lab`, rośnie przez trzy dni |
| `docs/lab.md` | Opis kroków projektu `Lab` |
| `docs/dotnet-cli-cheatsheet.md` | Ściąga z poleceń `dotnet` CLI |

## Wymagania

- .NET 10 SDK
- Visual Studio 2026 lub Rider (albo edytor i `dotnet` w terminalu)

## Przygotowanie środowiska (Windows)

Instalacja .NET 10 SDK:

```powershell
winget install --id Microsoft.DotNet.SDK.10 --exact
```

Instalacja Git:

```powershell
winget install --id Git.Git -e --source winget
```

Konfiguracja Git (wpisz swoje imię, nazwisko i adres e-mail):

```powershell
git config --global user.name "Jan Kowalski"
git config --global user.email "jan@example.com"
```

Sprawdzenie instalacji (otwórz nowe okno terminala, żeby wczytać zmienną `PATH`):

```powershell
dotnet --version
git --version
```

`dotnet --version` powinno pokazać wersję zaczynającą się od `10.`.

Przypięcie wersji SDK dla repozytorium (uruchom w głównym katalogu repo):

```powershell
dotnet new globaljson --sdk-version 10.0.404
```

Polecenie tworzy plik `global.json`, dzięki któremu wszystkie polecenia `dotnet` w tym katalogu i podkatalogach używają SDK 10.0.404 (lub nowszej poprawki z serii 10.0.4xx). Jeśli takiego SDK nie ma na komputerze, `dotnet` zgłosi błąd. Wtedy zainstaluj aktualne SDK poleceniem `winget upgrade --id Microsoft.DotNet.SDK.10`.
