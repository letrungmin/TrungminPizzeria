using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json; // Make sure you have this NuGet package installed

namespace TrungminPizzeria
{
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
            LoadPizzas();
            LoadToppings();
        }

        private void LoadPizzas()
        {
            lock (_fileLock)
            {
                try
                {
                    string json = File.ReadAllText(PIZZA_FILE);
                    _pizzas = JsonConvert.DeserializeObject<List<Pizza>>(json);
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine($"Error: Pizzas file '{PIZZA_FILE}' not found. Creating a new file with default pizzas.");
                    _pizzas = CreateDefaultPizzas();
                    SavePizzas();
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"Error: Invalid JSON format in pizzas file: {ex.Message}");
                    _pizzas = CreateDefaultPizzas();
                }
            }
        }

        private List<Pizza> CreateDefaultPizzas()
        {
            return new List<Pizza>
        {
            new Pizza("Margherita", "Small"),
            new Pizza("Pepperoni", "Medium"),
            // ... add other default pizza instances
        };
        }

        private void LoadToppings()
        {
            lock (_fileLock)
            {
                try
                {
                    string json = File.ReadAllText(TOPPINGS_FILE);
                    _toppings = JsonConvert.DeserializeObject<List<Topping>>(json);
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine($"Error: Toppings file '{TOPPINGS_FILE}' not found. Creating a new file with default toppings.");
                    _toppings = CreateDefaultToppings();
                    SaveToppings();
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"Error: Invalid JSON format in toppings file: {ex.Message}");
                    _toppings = CreateDefaultToppings();
                }
            }
        }

        private List<Topping> CreateDefaultToppings()
        {
            return new List<Topping>
        {
            new Topping("Extra Cheese", 1.00m),
            new Topping("Pepperoni", 1.50m),
            // ... add other default topping instances
        };
        }

        public void SavePizzas()
        {
            lock (_fileLock)
            {
                string json = JsonConvert.SerializeObject(_pizzas);
                File.WriteAllText(PIZZA_FILE, json);
            }
        }

        public void SaveToppings()
        {
            lock (_fileLock)
            {
                string json = JsonConvert.SerializeObject(_toppings);
                File.WriteAllText(TOPPINGS_FILE, json);
            }
        }

        public List<Pizza> GetAllPizzas() => _pizzas;

        public List<Topping> GetAllToppings() => _toppings;

        public Pizza GetPizza(string type, string size)
        {
            return _pizzas.FirstOrDefault(p => p.Type.Equals(type, StringComparison.OrdinalIgnoreCase) && p.Size.Equals(size, StringComparison.OrdinalIgnoreCase));
        }

        public void LoadPizzasFromFile(string filePath)
        {
            try
            {
                string json = File.ReadAllText(filePath);
                _pizzas = JsonConvert.DeserializeObject<List<Pizza>>(json);
            }
            catch (Exception ex) // Catching a more general Exception for now
            {
                Console.WriteLine($"Error loading pizzas: {ex.Message}");
                // Handle the error (e.g., provide default pizzas)
                _pizzas = CreateDefaultPizzas();
            }
        }

        public void LoadToppingsFromFile(string filePath)
        {
            try
            {
                string json = File.ReadAllText(filePath);
                _toppings = JsonConvert.DeserializeObject<List<Topping>>(json);
            }
            catch (Exception ex) // Catching a more general Exception for now
            {
                Console.WriteLine($"Error loading toppings: {ex.Message}");
                // Handle the error (e.g., provide default toppings)
                _toppings = CreateDefaultToppings();
            }
        }
    }

}
