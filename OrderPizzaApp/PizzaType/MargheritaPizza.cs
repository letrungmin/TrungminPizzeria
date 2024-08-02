using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderPizzaApp.PizzaType
{
    public class MargheritaPizza : Pizza
    {
        public MargheritaPizza() : base("Margherita", "Small") { }

        public override void CalculatePrice()
        {
            Price = basePrices[Type][Size]; 
            Price += Toppings.Sum(t => t.Price); 
        }
    }

}
