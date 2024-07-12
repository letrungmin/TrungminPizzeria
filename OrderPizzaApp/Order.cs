using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrungminPizzeria
{
    public class Order
    {
        private static int _nextOrderId = 1;

        public int OrderId { get; }
        public Customer Customer { get; set; }
        public string Address { get; set; }
        public List<Pizza> Pizzas { get; } = new List<Pizza>();
        public DateTime OrderTime { get; } = DateTime.Now;
        public OrderStatus Status { get; set; } = OrderStatus.PLACED;

        public Order(Customer customer, string address)
        {
            OrderId = _nextOrderId++;
            Customer = customer;
            Address = address;
        }

        public void AddPizza(Pizza pizza)
        {
            Pizzas.Add(pizza);
        }

        public void RemovePizza(Pizza pizza)
        {
            Pizzas.Remove(pizza);
        }

        public decimal CalculateTotalPrice()
        {
            return Pizzas.Sum(p => p.Price);
        }

        public string GetOrderDetails()
        {
            var details = new StringBuilder();
            details.AppendLine($"Order ID: {OrderId}");
            details.AppendLine($"Customer: {Customer.Name} ({Customer.PhoneNumber})");
            details.AppendLine($"Address: {Address}");
            details.AppendLine($"Order Time: {OrderTime}");
            details.AppendLine($"Status: {Status}");
            details.AppendLine("Pizzas:");

            foreach (var pizza in Pizzas)
            {
                details.AppendLine(pizza.GetOrderDetails());
            }

            details.AppendLine($"Total: ${CalculateTotalPrice():F2}");
            return details.ToString();
        }

        public void UpdateStatus(OrderStatus newStatus)
        {
            Status = newStatus;
        }

        public void UpdateCustomerDetails(string name, string address, string phoneNumber)
        {
            // Create a new Customer object with the updated details
            Customer = new Customer(name, address, phoneNumber);
        }

        public void CancelOrder()
        {
            UpdateStatus(OrderStatus.CANCELLED);
        }
    }


}
