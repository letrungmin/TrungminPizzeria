using System;
using System.Collections.Generic;
using System.Linq;

namespace TrungminPizzeria
{
    public class Menu
    {
        private readonly List<MenuItem> menuItems = new List<MenuItem>();
        private readonly PizzaRepository pizzaRepository;
        public Menu(PizzaRepository pizzaRepository)
        {
            this.pizzaRepository = pizzaRepository;
            GenerateMenuItems(); // Initialize menuItems
        }

        private void GenerateMenuItems()
        {
            var pizzas = pizzaRepository.GetAllPizzas();
            var toppings = pizzaRepository.GetAllToppings();

            foreach (var pizza in pizzas)
            {
                foreach (var size in new[] { "Small", "Medium", "Large" })
                {
                    var menuItem = new MenuItem(pizza, size);
                    menuItem.AddDefaultToppings(toppings, pizza.Type);
                    menuItems.Add(menuItem);
                }
            }
        }

        public List<MenuItem> GetMenuItemsByType(string type)
        {
            return menuItems
                .Where(mi => mi.Pizza.Type.Equals(type, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public List<MenuItem> GetMenuItemsBySize(string size)
        {
            return menuItems
                .Where(mi => mi.Size.Equals(size, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public List<MenuItem> GetAllMenuItems()
        {
            return menuItems;
        }

        public MenuItem GetMenuItem(int itemNumber)
        {
            if (itemNumber >= 1 && itemNumber <= menuItems.Count)
            {
                return menuItems[itemNumber - 1];
            }
            else
            {
                throw new ArgumentException("Invalid menu item number.");
            }
        }

        public List<MenuItem> SearchByToppings(string toppingsCriteria)
        {
            string[] searchToppings = toppingsCriteria.Split(',').Select(t => t.Trim().ToLower()).ToArray();
            return menuItems
                .Where(mi => searchToppings.All(t => mi.DefaultToppings.Any(dt => dt.Name.ToLower() == t)))
                .ToList();
        }
    }
}

