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
                // Logic tính giá cho Margherita
                Price = basePrices[Type][Size]; // Giá cơ bản
                Price += Toppings.Sum(t => t.Price); // Cộng thêm giá topping
            }
        }
}