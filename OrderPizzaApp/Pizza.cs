using System.Collections.Generic;
using System;
using TrungminPizzeria;
using System.Linq;
using Newtonsoft.Json;

public abstract class Pizza
{
    public string Type { get; set; }
    public string Size { get; set; }
    public List<Topping> Toppings { get; set; } = new List<Topping>();
    public decimal Price { get; set; }

    public static readonly Dictionary<string, Dictionary<string, decimal>> basePrices = new Dictionary<string, Dictionary<string, decimal>>
    {
        { "margherita", new Dictionary<string, decimal> { { "small", 8.00m }, { "medium", 10.00m }, { "large", 12.00m } } },
        { "pepperoni", new Dictionary<string, decimal> { { "small", 10.00m }, { "medium", 12.00m }, { "large", 14.00m } } },
        { "veggie", new Dictionary<string, decimal> { { "small", 9.50m }, { "medium", 11.50m }, { "large", 13.50m } } },
        { "hawaiian", new Dictionary<string, decimal> { { "small", 10.50m }, { "medium", 13.00m }, { "large", 15.50m } } }
    };

    [JsonConstructor]
    public Pizza(string type, string size)
    {

        Type = type?.ToLower() ?? ""; // Convert type to uppercase and handle null
        Size = size?.ToLower() ?? ""; // Convert size to lowercase and handle null

        // Validate type and size
        if (!basePrices.ContainsKey(Type) || !basePrices[Type].ContainsKey(Size))
        {
            throw new ArgumentException("Invalid pizza type or size.");
        }

        CalculatePrice();
    }
    public Pizza(Pizza pizza) : this(pizza.Type, pizza.Size)
    {
        Toppings = new List<Topping>(pizza.Toppings);
        CalculatePrice();
    }
    public Pizza(MenuItem menuItem) : this(menuItem.Pizza) 
    {
        foreach (var topping in menuItem.DefaultToppings)
        {
            AddTopping(topping);
        }
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

    public virtual void CalculatePrice()
    {
        // Ensure the pizza type and size are valid before accessing base prices
        if (!basePrices.ContainsKey(Type) || !basePrices[Type].ContainsKey(Size))
        {
            throw new ArgumentException("Invalid pizza type or size.");
        }

        Price = basePrices[Type][Size]; // Get base price
        Price += Toppings.Sum(t => t.Price); // Add topping prices
    }
    public string GetOrderDetails()
    {
        string toppingsString = Toppings.Count > 0
            ? string.Join(", ", Toppings.Select(t => t.Name))
            : "no extra toppings";
        return $"{Size} {Type} Pizza with {toppingsString} - ${Price:F2}";
    }
}

