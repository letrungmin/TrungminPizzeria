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

        public void AddDefaultTopping(Topping topping)
        {
            DefaultToppings.Add(topping);
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
