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
        public Guid Id { get; private set; }
        public string Code { get; private set; }
        public DateTime RegisteredAt { get; private set; } // Pola

        /* 

        // prywatna zmienna (back field)
        private SampleStatus _status; // Pola

        public SampleStatus Status  // Właściwość (Property)
        {
            // Getter
            get
            {
                return _status;
            }

            // Setter
            private set
            {
                // walidacja ... 

                _status = value;
            }
        }

        */


        // Skrócona definicja właściwosci
        public SampleStatus Status { get; private set; }


       /*
        // Get
        public SampleStatus GetStatus()
        {
            return _status;
        }     

        // Set
        public void SetStatus(SampleStatus status)
        {
            // walidacja ...
            _status = status;
        }

        */

        public Sample(string code)
        {
            RegisteredAt = DateTime.Now;
            Status = SampleStatus.Registered;

            this.Id = Guid.NewGuid();
            this.Code = code;
        }

        public void Start()
        {
            if (Status == SampleStatus.Registered)
            {
                Status = SampleStatus.InProgress;
            }

            // TODO: zglos blad
        }

        public void Complete()
        {
            Status = SampleStatus.Completed;
        }

        public override string ToString()
        {
            return $"Id: {Id} Code: {Code} RegisteredAt: {RegisteredAt} Status: {Status}";
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
