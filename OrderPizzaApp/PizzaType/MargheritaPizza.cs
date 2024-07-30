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
            // Logic tính giá cho Margherita
            Price = basePrices[Type][Size]; // Giá cơ bản
            Price += Toppings.Sum(t => t.Price); // Cộng thêm giá topping
        }
    }

}
