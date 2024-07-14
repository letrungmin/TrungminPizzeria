using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TrungminPizzeria;

public class PizzaRepository
{
    private List<Pizza> _pizzas = new List<Pizza>();
    private List<Topping> _toppings = new List<Topping>();
    private const string PIZZA_FILE = "pizzas.json";
    private const string TOPPINGS_FILE = "toppings.json";
    private static readonly object _fileLock = new object();

    public PizzaRepository()
    {
        LoadData();
    }

    private void LoadData()
    {
        LoadPizzasFromFile(PIZZA_FILE); // Load pizzas from file
        LoadToppingsFromFile(TOPPINGS_FILE); // Load toppings from file
    }

    public List<Pizza> GetAllPizzas() => _pizzas;

    public List<Topping> GetAllToppings() => _toppings;

    public Pizza GetPizza(string type, string size)
    {
        return _pizzas.FirstOrDefault(p => p.Type.Equals(type, StringComparison.OrdinalIgnoreCase) && p.Size.Equals(size, StringComparison.OrdinalIgnoreCase));
    }

    private void LoadPizzasFromFile(string filePath)
    {
        lock (_fileLock)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    _pizzas = JsonConvert.DeserializeObject<List<Pizza>>(json);
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"Error: Invalid JSON format in pizzas file: {ex.Message}");
                    // Handle the error (e.g., create default pizzas)
                    _pizzas = CreateDefaultPizzas();
                }
            }
            else
            {
                Console.WriteLine($"Error: Pizzas file '{filePath}' not found. Using default pizzas.");
                _pizzas = CreateDefaultPizzas();
            }
        }
    }

    // Trong PizzaRepository.cs
    private List<Pizza> CreateDefaultPizzas()
    {
        var toppings = GetAllToppings();
        return new List<Pizza>()
    {
        new Pizza(new MenuItem(new Pizza("Margherita", "Small"), "Small")), // Sử dụng hàm tạo từ MenuItem
        new Pizza(new MenuItem(new Pizza("Pepperoni", "Medium"), "Medium")), // Sử dụng hàm tạo từ MenuItem
        new Pizza(new MenuItem(new Pizza("Veggie", "Large"), "Large")), // Sử dụng hàm tạo từ MenuItem
        new Pizza(new MenuItem(new Pizza("Hawaiian", "Large"), "Large")), // Sử dụng hàm tạo từ MenuItem
        // ... add other default pizza instances
    };
    }


    private void LoadToppingsFromFile(string filePath)
    {
        lock (_fileLock)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    _toppings = JsonConvert.DeserializeObject<List<Topping>>(json);
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"Error: Invalid JSON format in toppings file: {ex.Message}");
                    // Handle the error (e.g., create default toppings)
                    _toppings = CreateDefaultToppings();
                }
            }
            else
            {
                Console.WriteLine($"Error: Toppings file '{filePath}' not found. Using default toppings.");
                _toppings = CreateDefaultToppings();
            }
        }
    }

    private List<Topping> CreateDefaultToppings()
    {
        return new List<Topping>
        {
            new Topping("Extra Cheese", 1.00m),
            //new Topping("Pepperoni", 1.50m)
            //new Topping("Onion", 0.50m)
            // ... add other default topping instances
        };
    }
}

