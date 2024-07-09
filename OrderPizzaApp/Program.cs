using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrungminPizzeria
{
    class Program
    {
        static void Main(string[] args)
        {
            PizzaRepository repository = new PizzaRepository();
            repository.LoadPizzasFromFile("pizzas.json"); // Or use a different data source
            repository.LoadToppingsFromFile("toppings.json");

            Menu menu = new Menu(repository.GetAllPizzas(), repository.GetAllToppings());

            ConsoleUI ui = new ConsoleUI(menu, repository);
            ui.Start();
        }
    }

}
