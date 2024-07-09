using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrungminPizzeria
{
    public class Pizza
    {
        public string Type { get; }
        public string Size { get; }
        public List<Topping> Toppings { get; } = new List<Topping>();
        public decimal Price { get; private set; } // Calculated dynamically

        public Pizza(string type, string size)
        {
            Type = type;
            Size = size;
            CalculatePrice();
        }

        public void AddTopping(Topping topping)
        {
            Toppings.Add(topping);
            CalculatePrice();
        }

        public void RemoveTopping(Topping topping)
        {
            Toppings.Remove(topping);
            CalculatePrice();
        }

        public void CalculatePrice()
        {
            Price = 0; // Base price (you'll need to define your base prices)
            switch (Size.ToLower())
            {
                case "small": Price += 8.00m; break;
                case "medium": Price += 10.00m; break;
                case "large": Price += 12.00m; break;
            }
            Price += Toppings.Sum(t => t.Price);
        }

        public string GetOrderDetails()
        {
            string toppingsString = string.Join(", ", Toppings.Select(t => t.Name));
            return $"{Size} {Type} Pizza with {toppingsString} - ${Price:F2}";
        }
    }

}
