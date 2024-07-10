using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TrungminPizzeria
{
    public class PizzaRepository
    {
        private List<Pizza> pizzas = new List<Pizza>();
        private List<Topping> toppings = new List<Topping>();

        public PizzaRepository()
        {
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


        // Methods for loading and saving pizzas and toppings from JSON files
        // ... (Use JsonConvert.DeserializeObject and JsonConvert.SerializeObject)
    }

}
