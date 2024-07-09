using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrungminPizzeria;

namespace TrungminPizzeria
{
    public class MenuItem
    {
        public Pizza Pizza { get; }
        public string Size { get; }
        public List<Topping> DefaultToppings { get; } = new List<Topping>();

        public MenuItem(Pizza pizza, string size)
        {
            Pizza = pizza;
            Size = size;
        }

        public void AddDefaultToppings(List<Topping> allToppings, string pizzaType)
        {
            // Logic to add default toppings based on pizzaType
            switch (pizzaType.ToLower())
            {
                case "margherita":
                    DefaultToppings.AddRange(allToppings.Where(t => t.Name.ToLower() == "mozzarella" || t.Name.ToLower() == "tomato sauce"));
                    break;
                case "pepperoni":
                    DefaultToppings.AddRange(allToppings.Where(t => t.Name.ToLower() == "mozzarella" || t.Name.ToLower() == "tomato sauce" || t.Name.ToLower() == "pepperoni"));
                    break;
                case "veggie":
                    DefaultToppings.AddRange(allToppings.Where(t => t.Name.ToLower() == "mozzarella" || t.Name.ToLower() == "tomato sauce" || t.Name.ToLower() == "mushrooms" || t.Name.ToLower() == "onions" || t.Name.ToLower() == "bell peppers"));
                    break;
                // ... (Add default toppings for other pizza types)
                default:
                    throw new ArgumentException("Invalid pizza type.");
            }
        }

        public decimal GetPrice()
        {
            decimal price = Pizza.Price;
            foreach (var topping in DefaultToppings)
            {
                price += topping.Price;
            }
            return price;
        }

        public string GetDescription()
        {
            string toppingsString = string.Join(", ", DefaultToppings.Select(t => t.Name));
            return $"{Size} {Pizza.Type} Pizza with {toppingsString} - ${GetPrice():F2}";
        }
    }

}
