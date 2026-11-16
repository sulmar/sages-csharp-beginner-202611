# Ściąga: dotnet CLI

Krótka lista poleceń przydatnych podczas tworzenia projektów .NET i dodawania ich do solution. Przykłady używają projektu warsztatowego `Lab` z katalogu `workshop/Lab`.

## Informacje o środowisku

Wersja SDK używana w bieżącym katalogu:

```bash
dotnet --version
```

Zainstalowane SDK i środowiska uruchomieniowe:

```bash
dotnet --list-sdks
dotnet --list-runtimes
```

Szczegółowe informacje o środowisku:

```bash
dotnet --info
```

## Solution

Utworzenie nowego pliku solution (w .NET 10 domyślnie w formacie `.slnx`):

```bash
dotnet new sln -n Lab -o workshop/Lab
```

Dodanie istniejącego projektu do solution:

```bash
dotnet sln workshop/Lab/Lab.slnx add workshop/Lab/Lab.ConsoleApp/Lab.ConsoleApp.csproj
```

Dodanie wielu projektów do solution:

```bash
dotnet sln workshop/Lab/Lab.slnx add \
  workshop/Lab/Lab.ConsoleApp/Lab.ConsoleApp.csproj \
  workshop/Lab/Lab.Domain/Lab.Domain.csproj
```

Wyświetlenie projektów dodanych do solution:

```bash
dotnet sln workshop/Lab/Lab.slnx list
```

Usunięcie projektu z solution:

```bash
dotnet sln workshop/Lab/Lab.slnx remove workshop/Lab/Lab.Domain/Lab.Domain.csproj
```

## Tworzenie projektów

Aplikacja konsolowa:

```bash
dotnet new console -n Lab.ConsoleApp -o workshop/Lab/Lab.ConsoleApp
```

Biblioteka klas (Class Library):

```bash
dotnet new classlib -n Lab.Domain -o workshop/Lab/Lab.Domain
```

Projekt testów jednostkowych (xUnit):

```bash
dotnet new xunit -n Lab.Domain.Tests -o workshop/Lab/Lab.Domain.Tests
```

Demo do modułu (tworzy `Demo.csproj` w podanym katalogu):

```bash
dotnet new console -n Demo -o src/day-1-fundamentals/05-methods/Demo
```

## Referencje między projektami

Dodanie referencji projektu:

```bash
dotnet add workshop/Lab/Lab.ConsoleApp/Lab.ConsoleApp.csproj reference \
  workshop/Lab/Lab.Domain/Lab.Domain.csproj
```

Dodanie kilku referencji:

```bash
dotnet add workshop/Lab/Lab.Domain.Tests/Lab.Domain.Tests.csproj reference \
  workshop/Lab/Lab.Domain/Lab.Domain.csproj \
  workshop/Lab/Lab.ConsoleApp/Lab.ConsoleApp.csproj
```

Wyświetlenie referencji projektu:

```bash
dotnet list workshop/Lab/Lab.ConsoleApp/Lab.ConsoleApp.csproj reference
```

Usunięcie referencji:

```bash
dotnet remove workshop/Lab/Lab.ConsoleApp/Lab.ConsoleApp.csproj reference \
  workshop/Lab/Lab.Domain/Lab.Domain.csproj
```

## Pakiety NuGet

Dodanie pakietu:

```bash
dotnet add workshop/Lab/Lab.ConsoleApp/Lab.ConsoleApp.csproj package Spectre.Console
```

Dodanie pakietu w konkretnej wersji:

```bash
dotnet add workshop/Lab/Lab.ConsoleApp/Lab.ConsoleApp.csproj package Spectre.Console --version 0.49.1
```

Wyświetlenie pakietów projektu (z `--outdated` pokazuje dostępne aktualizacje):

```bash
dotnet list workshop/Lab/Lab.ConsoleApp/Lab.ConsoleApp.csproj package
dotnet list workshop/Lab/Lab.ConsoleApp/Lab.ConsoleApp.csproj package --outdated
```

Usunięcie pakietu:

```bash
dotnet remove workshop/Lab/Lab.ConsoleApp/Lab.ConsoleApp.csproj package Spectre.Console
```

> W .NET 10 działa też składnia „rzeczownik, potem czasownik”, np. `dotnet reference add`, `dotnet package add`, `dotnet package list`. Obie formy robią to samo.

## Przydatne komendy

Przywrócenie zależności:

```bash
dotnet restore
```

Budowanie projektu lub solution:

```bash
dotnet build
dotnet build workshop/Lab/Lab.slnx
```

Uruchomienie projektu:

```bash
dotnet run --project workshop/Lab/Lab.ConsoleApp/Lab.ConsoleApp.csproj
```

Uruchomienie z argumentami (wszystko po `--` trafia do `args`):

```bash
dotnet run --project workshop/Lab/Lab.ConsoleApp/Lab.ConsoleApp.csproj -- data/samples.csv
```

Uruchomienie z automatycznym przebudowaniem po zapisaniu pliku:

```bash
dotnet watch --project workshop/Lab/Lab.ConsoleApp/Lab.ConsoleApp.csproj
```

Uruchomienie pojedynczego pliku `.cs` bez projektu (.NET 10):

```bash
dotnet run hello.cs
```

Uruchomienie testów:

```bash
dotnet test
dotnet test workshop/Lab/Lab.Domain.Tests/Lab.Domain.Tests.csproj
```

Usunięcie wyników kompilacji (`bin/`, `obj/`):

```bash
dotnet clean
```

Formatowanie kodu według reguł `.editorconfig`:

```bash
dotnet format
```

Publikacja aplikacji do katalogu:

```bash
dotnet publish workshop/Lab/Lab.ConsoleApp/Lab.ConsoleApp.csproj -c Release -o publish
```

Lista dostępnych szablonów:

```bash
dotnet new list
```

Pomoc dla konkretnego szablonu:

```bash
dotnet new console --help
```

Pomoc dla dowolnego polecenia:

```bash
dotnet --help
dotnet sln --help
```
