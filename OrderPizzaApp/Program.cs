using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TrungminPizzeria
{
    class Program
    {
        static Task Main(string[] args)
        {
            PizzaRepository repository = new PizzaRepository();
            UI ui = new UI(new Menu(repository), repository);
            ui.Start();
            return Task.CompletedTask;
        }
    }
}
