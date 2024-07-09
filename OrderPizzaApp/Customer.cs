using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrungminPizzeria
{
    public class Customer
    {
        public string Name { get; }
        public string Address { get; }
        public string PhoneNumber { get; }

        public Customer(string name, string address, string phoneNumber)
        {
            Name = name;
            Address = address;
            PhoneNumber = phoneNumber;
        }
    }

}
