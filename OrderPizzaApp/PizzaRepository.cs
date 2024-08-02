using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using OrderPizzaApp;

namespace TrungminPizzeria
{
    public class PizzaRepository
    {
        public const string PIZZA_FILE = "pizzas.json";
        public const string TOPPINGS_FILE = "toppings.json";
        public static readonly object _fileLock = new object();
        public List<Pizza> _pizzas = new List<Pizza>();
        public List<Topping> _toppings = new List<Topping>();
        public PizzaRepository()
        {
            LoadData();
        }

        
        private List<Pizza> CreateDefaultPizzas()
        {
            var factory = new ConcretePizzaFactory();
            return new List<Pizza>()
    {
        factory.CreatePizza("Margherita"),
        factory.CreatePizza("Pepperoni"),
        factory.CreatePizza("Veggie"),
        factory.CreatePizza("Hawaiian")
    };
        }

        public List<Topping> CreateDefaultToppings()
        {
            return new List<Topping>
        {
            new Topping("Extra Cheese", 1.00m),
            new Topping("Pepperoni", 1.50m),
            new Topping("Onion", 0.50m)
            
        };
        }

        public List<Pizza> GetAllPizzas() => _pizzas;

        public List<Topping> GetAllToppings() => _toppings;

        public Pizza GetPizza(string type, string size)
        {
            return _pizzas.FirstOrDefault(p => p.Type.Equals(type, StringComparison.OrdinalIgnoreCase) && p.Size.Equals(size, StringComparison.OrdinalIgnoreCase));
        }

        public void LoadData()
        {
            LoadPizzasFromFile(PIZZA_FILE); // Load pizzas from file
            LoadToppingsFromFile(TOPPINGS_FILE);
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
    }


}

