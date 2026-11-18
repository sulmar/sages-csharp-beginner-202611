using System.Xml.Linq;

Console.WriteLine("Hello, World!");


List<Product> products =
[
    new Product { Name = "a", Color = "red", Price = 100 },
    new Product { Name = "b", Color = "blue", Price = 40 },
    new Product { Name = "c", Color = "green", Price = 500 },
    new Product { Name = "d", Color = "red", Price = 80 },
    new Product { Name = "e", Color = "green", Price = 110 },
];




// TODO: Wyszukaj produkt o nazwie c
Product foundProduct = FindProduct("c");

Console.WriteLine(foundProduct);

// LINQ (Language Integrated Query)

List<Product> redProducts = FindByColor("red");

Console.WriteLine("Znalezione produkty: ");

foreach (var product in redProducts)
{
    Console.WriteLine(product);
}

Product FindProduct(string name)
{
    Product foundProduct = null;

    foreach (Product product in products)
    {
        if (product.Name == name) // Warunek (Predykat)
        {
            foundProduct = product;
            break;
        }
    }

    return foundProduct;
}


Product FindProductLinq(string name) => products.Single(product => product.Name == name);    // Warunek(Predykat) - wyrażenie lambda


// TODO: wyszukaj produkty w kolorze czerwonym
List<Product> FindByColor(string color)
{
    List<Product> foundProducts = new();

    foreach (Product product in products)
    {
        if (product.Color == color) // Warunek (Predykat)
        {
            foundProducts.Add(product);            
        }
    }

    return foundProducts;
}


// LINQ
List<Product> FindByColorLinq(string color) => products // zbiór
    .Where(product => product.Color == color) // Predykat (zapisany za pomocą wyrażenia lambda)
    .ToList();

// TODO: Oblicz sumę produktów


class Product
{
    public string Name { get; set; }
    public string Color { get; set; }
    public decimal Price { get; set; }

    public override string ToString()
    {
        return $"Name: {Name} Color: {Color} Price: {Price}";
    }
}