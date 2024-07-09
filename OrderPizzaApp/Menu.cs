using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrungminPizzeria
{
    public class Menu
    {
        private List<MenuItem> menuItems = new List<MenuItem>();

        public Menu(List<Pizza> pizzas, List<Topping> toppings)
        {
            // Create menu items based on pizza and topping combinations
            foreach (var pizza in pizzas)
            {
                foreach (var size in new[] { "Small", "Medium", "Large" })
                {
                    var menuItem = new MenuItem(pizza, size);

                    // Add default toppings based on pizza type (you'll need to define these rules)
                    if (pizza.Type == "Margherita")
                    {
                        menuItem.AddDefaultTopping(toppings.Find(t => t.Name == "Mozzarella"));
                        menuItem.AddDefaultTopping(toppings.Find(t => t.Name == "Tomato Sauce"));
                    }
                    // ... (Add default toppings for other pizza types)

                    menuItems.Add(menuItem);
                }
            }
        }

        public List<MenuItem> GetMenuItemsByType(string type)
        {
            return menuItems.Where(mi => mi.Pizza.Type.Equals(type, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<MenuItem> GetMenuItemsBySize(string size)
        {
            return menuItems.Where(mi => mi.Size.Equals(size, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<MenuItem> GetAllMenuItems()
        {
            return menuItems;
        }
    }


}
