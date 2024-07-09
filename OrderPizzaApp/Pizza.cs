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

        private static readonly Dictionary<string, Dictionary<string, decimal>> basePrices = new Dictionary<string, Dictionary<string, decimal>>
    {
        { "margherita", new Dictionary<string, decimal> { { "small", 8.00m }, { "medium", 10.00m }, { "large", 12.00m } } },
        { "pepperoni", new Dictionary<string, decimal> { { "small", 10.00m }, { "medium", 12.00m }, { "large", 14.00m } } },
        { "veggie", new Dictionary<string, decimal> { { "small", 9.50m }, { "medium", 11.50m }, { "large", 13.50m } } }
        // Add more pizza types and their base prices here
    };

        // Constructor for creating a new pizza from scratch
        public Pizza(string type, string size)
        {
            Type = type;
            Size = size;
            CalculatePrice();
        }

        // Constructor for creating a pizza from existing pizza object
        public Pizza(Pizza pizza)
        {
            Type = pizza.Type;
            Size = pizza.Size;
            Toppings = pizza.Toppings;
            CalculatePrice();
        }

        // Constructor for creating a pizza from a MenuItem
        public Pizza(MenuItem menuItem)
        {
            Type = menuItem.Pizza.Type;
            Size = menuItem.Size;
            // Add the default toppings from MenuItem
            foreach (var topping in menuItem.DefaultToppings)
            {
                AddTopping(topping);
            }
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

        private void CalculatePrice()
        {
            // Ensure the pizza type and size are valid before accessing base prices
            if (!basePrices.ContainsKey(Type.ToLower()) || !basePrices[Type.ToLower()].ContainsKey(Size.ToLower()))
            {
                throw new ArgumentException("Invalid pizza type or size.");
            }

            Price = basePrices[Type.ToLower()][Size.ToLower()]; // Get base price based on type and size
            Price += Toppings.Sum(t => t.Price); // Add topping prices
        }

        public string GetOrderDetails()
        {
            string toppingsString = Toppings.Count > 0
                ? string.Join(", ", Toppings.Select(t => t.Name))
                : "no toppings";
            return $"{Size} {Type} Pizza with {toppingsString} - ${Price:F2}";
        }
    }


}
