using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderPizzaApp.PizzaType
{
        public class HawaiianPizza : Pizza
        {
            public HawaiianPizza() : base("Hawaiian", "Large") { }

            public override void CalculatePrice()
            {
                // Price logic for Pizza Type
                Price = basePrices[Type][Size]; // base price
                Price += Toppings.Sum(t => t.Price); // Add ons toppings
            }
        }
}