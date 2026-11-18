using System;
using System.Collections.Generic;
using System.Text;

namespace Demo
{
    internal class Shipment
    {

        public int Number { get; set; }
        public string Recipient { get; set; }
        public decimal Weight { get; set; }

        private bool isSent = false;

        public Shipment(int number, string recipient, decimal weight)
        {
            if (number <= 0)
                throw new ArgumentOutOfRangeException(nameof(number));

            if (string.IsNullOrWhiteSpace(recipient))
                throw new ArgumentException("Odbiorca nie może być pusty", nameof(recipient));

            if (weight <= 0 || weight > 20)
                throw new ArgumentOutOfRangeException(nameof(weight), "Waga musi być większa od 0 i nie większa od 20");


            this.Number = number;
            this.Recipient = recipient;
            this.Weight = weight;
        }

        public void Send()
        {
            throw new NotImplementedException();
        }
        
    }


    internal class ShipmentCalculator
    {
        public decimal Calculate(Shipment shipment, decimal costPerKg, decimal ratio)
        {
            if (shipment == null)
                throw new ArgumentNullException(nameof(shipment));

            if (costPerKg < 0)
                throw new ArgumentException(nameof(costPerKg), "Koszt nie może ujemny.");

            // algorytm
            return shipment.Weight * costPerKg / ratio;
        }
    }
}
