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
            // Logic tính giá cho Margherita
            Price = basePrices[Type][Size]; // Giá cơ bản
            Price += Toppings.Sum(t => t.Price); // Cộng thêm giá topping
        }
    }
}
