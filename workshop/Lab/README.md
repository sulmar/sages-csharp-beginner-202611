# Projekt Lab

Konsolowy system laboratorium. Kroki 01–12 i sygnatury: [docs/lab.md](../../docs/lab.md).





1. Utworzenie Lab.ConsoleApp

```
dotnet new console --name Lab.ConsoleApp

```



2\. Utworzenie Lab.Domain 



```

dotnet new classlib --name Lab.Domain

```





3\. Utworzenie solution

```

dotnet new sln --name Lab

```



4\. Dodanie projektów do solution

```
dotnet sln add Lab.ConsoleApp

dotnet sln add Lab.Domain


```



