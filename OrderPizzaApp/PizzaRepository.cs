using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace TrungminPizzeria
{
    public class PizzaRepository
    {
        private List<Pizza> pizzas = new List<Pizza>();
        private List<Topping> toppings = new List<Topping>();

        public PizzaRepository()
        {
            // Load initial data from JSON files
            LoadPizzasFromFile("pizzas.json");
            LoadToppingsFromFile("toppings.json");
        }

        public List<Pizza> GetAllPizzas() => pizzas;

        public List<Topping> GetAllToppings() => toppings;

        public Pizza GetPizza(string type, string size)
        {
            return pizzas.FirstOrDefault(p =>
                p.Type.Equals(type, StringComparison.OrdinalIgnoreCase) &&
                p.Size.Equals(size, StringComparison.OrdinalIgnoreCase)
            );
        }

        public void LoadPizzasFromFile(string filePath)
        {
            try
            {
                string json = File.ReadAllText(filePath);
                pizzas = JsonConvert.DeserializeObject<List<Pizza>>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading pizzas: {ex.Message}");
                // Handle the error appropriately (e.g., provide default pizzas)
            }
        }

        public void LoadToppingsFromFile(string filePath)
        {
            try
            {
                string json = File.ReadAllText(filePath);
                toppings = JsonConvert.DeserializeObject<List<Topping>>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading toppings: {ex.Message}");
                // Handle the error appropriately (e.g., provide default toppings)
            }
        }
    }

}
