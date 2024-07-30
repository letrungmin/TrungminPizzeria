using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderPizzaApp.PizzaType
{
    public class PepperoniPizza : Pizza
    {
        public PepperoniPizza() : base("Pepperoni", "Medium") { }

        public override void CalculatePrice()
        {
            // Logic tính giá cho Margherita
            Price = basePrices[Type][Size]; // Giá cơ bản
            Price += Toppings.Sum(t => t.Price); // Cộng thêm giá topping
        }
    }


    //public class PepperoniPizza : Pizza
    //{
    //    public PepperoniPizza(string size) : base("Pepperoni", size) { }
    //    public override void CalculatePrice()
    //    {
    //        // Logic tính giá cho Pepperoni
    //        base.CalculatePrice(); // Gọi phương thức CalculatePrice từ lớp cha
    //        Price += 1.50m; // Cộng thêm giá topping Pepperoni vào giá cơ bản của pizza
    //    }
    //}

}
