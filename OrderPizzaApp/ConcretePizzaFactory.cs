using OrderPizzaApp.PizzaType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderPizzaApp
{
    public class ConcretePizzaFactory : PizzaFactory
    {
        public override Pizza CreatePizza(string type)
        {
            switch (type.ToLower())
            {
                case "margherita": return new MargheritaPizza();
                case "pepperoni": return new PepperoniPizza();
                case "veggie": return new VeggiePizza();
                case "hawaiian": return new HawaiianPizza();
                
                default: throw new ArgumentException("Invalid pizza type.");
            }
        }
    }

}
