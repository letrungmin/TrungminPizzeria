using System;
using System.Collections.Generic;
using System.Linq;
using TrungminPizzeria;

public class Menu
{
    private readonly List<MenuItem> menuItems = new List<MenuItem>();
    private readonly PizzaRepository pizzaRepository;

    public Menu(PizzaRepository pizzaRepository)
    {
        this.pizzaRepository = pizzaRepository;
        GenerateMenuItems();
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
                //menuItem.AddDefaultToppings(toppings, pizza.Type);
                menuItems.Add(menuItem);
            }
        }
    }

    // Added an indexer to get a MenuItem by index
    public MenuItem this[int index]
    {
        get
        {
            if (index >= 0 && index < menuItems.Count)
            {
                return menuItems[index];
            }
            else
            {
                throw new IndexOutOfRangeException("Invalid menu item index.");
            }
        }
    }

    // Added GetPizzaTypes() to get unique pizza types
    public List<string> GetPizzaTypes()
    {
        return menuItems.Select(mi => mi.Pizza.Type).Distinct().ToList();
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

    public List<MenuItem> SearchByToppings(string toppingsCriteria)
    {
        var searchToppings = toppingsCriteria.Split(',').Select(t => t.Trim().ToLower()).ToList();
        return menuItems
            .Where(mi => searchToppings.All(t => mi.DefaultToppings.Any(dt => dt.Name.ToLower() == t)))
            .ToList();
    }

    public List<MenuItem> GetAllMenuItems()
    {
        return menuItems; // Assuming you have a list called menuItems in your Menu class
    }

    // In Menu.cs
    public MenuItem GetMenuItem(int itemNumber)
    {
        if (itemNumber >= 1 && itemNumber <= menuItems.Count)
        {
            return menuItems[itemNumber - 1]; // Adjust for zero-based indexing
        }
        else
        {
            throw new ArgumentException("Invalid menu item number.");
        }
    }

    
}
