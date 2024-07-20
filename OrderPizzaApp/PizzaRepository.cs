using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace TrungminPizzeria
{
    public class PizzaRepository
    {
        public List<Pizza> _pizzas = new List<Pizza>();
        public List<Topping> _toppings = new List<Topping>();
        public const string PIZZA_FILE = "pizzas.json";
        public const string TOPPINGS_FILE = "toppings.json";
        public static readonly object _fileLock = new object();


        public PizzaRepository()
        {
            LoadData();
        }

        public void LoadData()
        {
            LoadPizzasFromFile(PIZZA_FILE); // Load pizzas from file
            LoadToppingsFromFile(TOPPINGS_FILE);
        }

        public List<Pizza> GetAllPizzas() => _pizzas;

        public List<Topping> GetAllToppings() => _toppings;

        public Pizza GetPizza(string type, string size)
        {
            return _pizzas.FirstOrDefault(p => p.Type.Equals(type, StringComparison.OrdinalIgnoreCase) && p.Size.Equals(size, StringComparison.OrdinalIgnoreCase));
        }

        public void LoadPizzasFromFile(string filePath)
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
        public List<Pizza> CreateDefaultPizzas()
        {
            var toppings = GetAllToppings();
            return new List<Pizza>()
    {
        new Pizza(new MenuItem(new Pizza("Margherita", "Small"), "Small")),
        new Pizza(new MenuItem(new Pizza("Pepperoni", "Medium"), "Medium")),
        new Pizza(new MenuItem(new Pizza("Veggie", "Large"), "Large")),
        new Pizza(new MenuItem(new Pizza("Hawaiian", "Large"), "Large")),
    };
        }


        public void LoadToppingsFromFile(string filePath)
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

        public List<Topping> CreateDefaultToppings()
        {
            return new List<Topping>
        {
            new Topping("Extra Cheese", 1.00m),
            new Topping("Pepperoni", 1.50m),
            new Topping("Onion", 0.50m)
            // ... add other default topping instances
        };
        }
    }
}

