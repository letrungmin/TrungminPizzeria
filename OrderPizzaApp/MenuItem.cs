using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            // Ensure allToppings is not null and contains items
            if (allToppings == null || allToppings.Count == 0)
            {
                Console.WriteLine("Error: No toppings available to add to the menu item.");
                return;
            }

            switch (pizzaType.ToLower())
            {
                case "margherita":
                    DefaultToppings.AddRange(allToppings.Where(t =>
                        t.Name.Equals("Mozzarella", StringComparison.OrdinalIgnoreCase) ||
                        t.Name.Equals("Tomato Sauce", StringComparison.OrdinalIgnoreCase)));
                    break;
                case "pepperoni":
                    DefaultToppings.AddRange(allToppings.Where(t =>
                        t.Name.Equals("Mozzarella", StringComparison.OrdinalIgnoreCase) ||
                        t.Name.Equals("Tomato Sauce", StringComparison.OrdinalIgnoreCase) ||
                        t.Name.Equals("Pepperoni", StringComparison.OrdinalIgnoreCase)));
                    break;
                case "veggie":
                    DefaultToppings.AddRange(allToppings.Where(t =>
                        t.Name.Equals("Mozzarella", StringComparison.OrdinalIgnoreCase) ||
                        t.Name.Equals("Tomato Sauce", StringComparison.OrdinalIgnoreCase) ||
                        t.Name.Equals("Mushrooms", StringComparison.OrdinalIgnoreCase) ||
                        t.Name.Equals("Onions", StringComparison.OrdinalIgnoreCase) ||
                        t.Name.Equals("Bell Peppers", StringComparison.OrdinalIgnoreCase)));
                    break;
                case "hawaiian":
                    DefaultToppings.AddRange(allToppings.Where(t =>
                        t.Name.Equals("Mozzarella", StringComparison.OrdinalIgnoreCase) ||
                        t.Name.Equals("Tomato Sauce", StringComparison.OrdinalIgnoreCase) ||
                        t.Name.Equals("Mushrooms", StringComparison.OrdinalIgnoreCase) ||
                        t.Name.Equals("Onions", StringComparison.OrdinalIgnoreCase) ||
                        t.Name.Equals("Bell Peppers", StringComparison.OrdinalIgnoreCase)));
                    break;
                default:
                    throw new ArgumentException("Invalid pizza type.");
            }
        }

        public decimal GetPrice()
        {
            // Lấy giá cơ bản từ lớp Pizza dựa trên loại và kích thước
            decimal price = Pizza.basePrices[Pizza.Type.ToLower()][Size.ToLower()];

            // Cộng thêm giá của các topping mặc định
            foreach (var topping in DefaultToppings)
            {
                price += topping.Price;
            }

            return price;
        }

        public string GetDescription()
        {
            string toppingsString = DefaultToppings.Count > 0
                ? string.Join(", ", DefaultToppings.Select(t => t.Name))
                : "no extra toppings";
            return $"{Size} {Pizza.Type} Pizza with {toppingsString} - ${GetPrice():F2}";
        }
    }
}
