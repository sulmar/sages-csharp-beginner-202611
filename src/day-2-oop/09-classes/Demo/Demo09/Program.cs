using System.ComponentModel;
using System.Diagnostics.SymbolStore;
using System.Net.Http.Headers;
using System.Numerics;

Console.WriteLine("Hello, World!");

// ----- Programowanie strukturalne

string number = "FV/2026/001";
decimal net = 100m;
decimal vatRate = 23m;
string customerName = "ACME";
string customerNIP = "1231235545";

PrintInvoice(number, customerName, customerNIP, net, vatRate);

string number2 = "FV/2026/002";
decimal net2 = 200m;
decimal vatRate2 = 23m;
string customerName2 = "ACME";
string customerNIP2 = "1231235545";

PrintInvoice(number2, customerName2, customerNIP2, net2, vatRate2);

void PrintInvoice(string number, string customerName, string customerNIP, decimal net, decimal vatRate)
{
    Console.WriteLine($"FV: Numer {number} Klient: {customerName} NIP: {customerNIP} Kwota netto: {net} Stawka VAT: {vatRate}");
}

// ---- Programowanie zorientowanie obiektowo  OOP (Object Oriented Programming)

Address address = new Address { Street = "Dworcowa", PostCode = "01-001", BuildingNumber = "1A", FlatNumber = "2" };

Customer customer = new Customer { CustomerName = "ACME", CustomerNIP = "1231235545", InvoiceAddress = address, ShippingAddress = address };

List<Invoice> invoices = new List<Invoice>();
Invoice invoice1 = new Invoice { Number = "FV/2026/001", Net = 100m, VatRate = 23m, Customer = customer };
Invoice invoice2 = new Invoice { Number = "FV/2026/002", Net = 200m, VatRate = 23m, Customer = customer };

invoices.Add(invoice1);
invoices.Add(invoice2);

Item product1 = new Product { Name = "Komputer", Price = 1000, Color = "black" };
Item product2 = new Product { Name = "Myszka", Price = 100, Color = "silver" };
Item service1 = new Service { Name = "Wymiana dysku", Price = 200, Duration = TimeSpan.Parse("03:00") };

invoice1.AddLine(product1, product1.Price, 2);
invoice1.AddLine(product2, 90);
invoice1.AddLine(service1, 300);

invoice1.Confirm();


KsefService ksefService = new KsefService();

foreach (Invoice invoice in invoices)
{
    Console.WriteLine(invoice.GetFull());
    ksefService.Send(invoice);
}


class Customer
{
    public string CustomerName;
    public string CustomerNIP;
    public Address InvoiceAddress;
    public Address ShippingAddress;

    public string GetFull()
    {
        return $"{CustomerName} {CustomerNIP} InvoiceAddress: {InvoiceAddress.GetFullAddress()} ShippingAddress: {ShippingAddress.GetFullAddress()}";        
    }
}

class Address
{
    public string Street;
    public string BuildingNumber;
    public string FlatNumber;
    public string PostCode;

    public string GetFullAddress()
    {
        return $"{PostCode} {Street} {BuildingNumber}/{FlatNumber}";
    }
}


// Klasa bazowa
abstract class Item
{
    public string Name;
    public decimal Price;

    public virtual string GetFull()  // Polimorfizm (Wielopostaciowość)
    {
        return $"Name: {Name} Price: {Price}";
    }

}

// Klata potomna
class Product : Item   // Dziedziczenie (Inheritance)
{
    public string Color;
    public float Weight;

    public override string GetFull()
    {
        return $"{base.GetFull()} Color: {Color} Weight: {Weight}";
    }
}

// Klata potomna
class Service : Item // Dziedziczenie
{
    public TimeSpan Duration;

    public override string GetFull()
    {
        return $"{base.GetFull()} Duration: {Duration}";
    }
}

class InvoiceLine
{
    public int Order;
    public Item Item;
    public decimal Price;
    public int Quantity;
    public decimal GetAmount()
    {
        return Price * Quantity;
    }

    public string GetFullLine()
    {
        return Item.GetFull();
    }
}

class Invoice
{
    public string Number;
    public decimal Net;
    public decimal VatRate;
    public Customer Customer; // Agregacja  
    public DateTime DueDate { get; set; }
    public InvoiceStatus Status;

    // Hermetyzacja
    private List<InvoiceLine> Lines = new();

    public Invoice()
    {
        Status = InvoiceStatus.Registered;
    }

    // Fasada
    public void AddLine(Item item, decimal price, int quantity = 1)
    {
        int lastOrder = 0;

        if (Lines.Any())
        {
            lastOrder = Lines.Max(line => line.Order);
        }

        InvoiceLine invoiceLine = new InvoiceLine { Item = item, Price = price, Quantity = quantity, Order = ++lastOrder };

        this.Lines.Add(invoiceLine);
    }



    public string GetFull()
    {
        string header = $"FV: Numer {this.Number} Klient: {this.Customer.GetFull()} Kwota netto: {this.Net} Stawka VAT: {this.VatRate}";

        string content = "";

        foreach (var line in Lines)
        {
            content += line.GetFullLine();
        }

        return header + content;
    }

    public void Confirm()
    {
        if (Status == InvoiceStatus.Registered)
        {
            Status = InvoiceStatus.Accounted;
        }
    }

    public void Cancel()
    {
        Status = InvoiceStatus.Cancelled;
    }
}


enum InvoiceStatus
{
    Registered,
    Accounted,
    Cancelled
}

class KsefService
{
    public void Send(Invoice invoice)
    {
        if (invoice.Status == InvoiceStatus.Accounted)
        {

            Console.WriteLine($"Wysyłanie faktury {invoice.Number} do KSeF...");
        }
        else
        {
            Console.WriteLine("Pominięto");
        }
    }
}