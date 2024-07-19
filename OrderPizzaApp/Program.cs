using System;
using System.Threading.Tasks; // For Task.Delay() if you want to add a delay between menu options
using Newtonsoft.Json;

namespace TrungminPizzeria
{
    class Program
    {
        static Task Main(string[] args)
        {
            PizzaRepository repository = new PizzaRepository();
            ConsoleUI ui = new ConsoleUI(new Menu(repository), repository);
            ui.Start();
            return Task.CompletedTask;
        }
    }
}
