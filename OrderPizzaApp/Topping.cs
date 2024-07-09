using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrungminPizzeria
{
    public class Topping
    {
        public string Name { get; }
        public decimal Price { get; }

        public Topping(string name, decimal price)
        {
            Name = name;
            Price = price;
        }
    }

}
