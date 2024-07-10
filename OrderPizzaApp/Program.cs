using System;

namespace TrungminPizzeria
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a new PizzaRepository and load data
            PizzaRepository repository = new PizzaRepository();
            repository.LoadPizzasFromFile("pizzas.json"); // Or your preferred data source
            repository.LoadToppingsFromFile("toppings.json");

            // Create a new Menu (passing only the repository)
            Menu menu = new Menu(repository); // Correct constructor call

            // Create a new ConsoleUI and start the application
            ConsoleUI ui = new ConsoleUI(menu, repository);
            ui.Start();
        }
    }
}
