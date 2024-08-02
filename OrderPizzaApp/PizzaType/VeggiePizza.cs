using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderPizzaApp.PizzaType
{
    public class VeggiePizza : Pizza
    {
        public VeggiePizza() : base("Veggie", "Large") { }

        public override void CalculatePrice()
        {
            Price = basePrices[Type][Size]; 
            Price += Toppings.Sum(t => t.Price);
        }
    }
}
