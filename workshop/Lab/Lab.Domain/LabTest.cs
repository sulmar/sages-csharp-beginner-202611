using System;
using System.Collections.Generic;
using System.Text;

namespace Lab.Domain
{
    // Klasa reprezentująca badanie laboratoryjne
    public class LabTest
    {
        public string Code;
        public string Name;
        public decimal Price;
        public string Description;

        // Konstruktor bezparametryczny
        //public LabTest() {
        //}

        // Konstruktor - specjalna metoda wywoływana przez operator new()
        // Kontruktor ma identyczną nazwę jak nazwa klasy
        // Konstruktor nic nie zwraca - on tworzy obiekt
        public LabTest(string code, string name, decimal price) // w Pythonie init
        {
            this.Code = code;
            this.Name = name;
            this.Price = price;
        }       
    }

    /*
    
    Pyhon class przykład:

    class Dog:

    def __init__(self, name, age): // konstruktor

        self.name = name

        self.age = age

    */


    public class Sample
    {
        public Guid Id;
        public string Code;
        public DateTime RegisteredAt;
        public SampleStatus Status;

        public Sample(string code)
        {
            RegisteredAt = DateTime.Now;
            Status = SampleStatus.Registered;

            this.Id = Guid.NewGuid();
            this.Code = code;
        }

        public void Start()
        {
            Status = SampleStatus.InProgress;
        }

        public void Complete()
        {
            Status = SampleStatus.Completed;
        }
    }

    public enum SampleStatus
    {
        Registered,
        InProgress,
        Completed,
        Cancelled
    }


}
