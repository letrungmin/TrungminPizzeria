using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TrungminPizzeria
{
    public class ConsoleUI
    {
        private readonly Menu menu;
        private readonly PizzaRepository repository;
        private readonly List<Order> orders = new List<Order>();

        public ConsoleUI(Menu menu, PizzaRepository repository)
        {
            this.menu = menu;
            this.repository = repository;
        }

        public void Start()
        {
            while (true)
            {
                Console.WriteLine("\nWelcome to Trungmin's Pizzeria!");
                Console.WriteLine("----------------------------");
                Console.WriteLine("1. View Menu");
                Console.WriteLine("2. Place Order");
                Console.WriteLine("3. Manage Orders");
                Console.WriteLine("4. Exit");
                Console.WriteLine("----------------------------");

                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": DisplayMenu(); break;
                    case "2": PlaceOrder(); break;
                    case "3": ManageOrders(); break;
                    case "4":
                        Console.WriteLine("Thank you for visiting Trungmin's Pizzeria!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        public void DisplayMenu()
        {
            List<MenuItem> menuItemsToDisplay = menu.GetAllMenuItems();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n*** MENU ***");

                int itemNumber = 1;
                foreach (var menuItem in menuItemsToDisplay)
                {
                    Console.WriteLine($"{itemNumber}. {menuItem.GetDescription()}");
                    itemNumber++;
                }

                Console.WriteLine("\nFilter Options:");
                Console.WriteLine("1. Filter by Type");
                Console.WriteLine("2. Filter by Size");
                Console.WriteLine("3. Search by Toppings");
                Console.WriteLine("4. Clear Filters");
                Console.WriteLine("0. Back to Main Menu");

                Console.Write("Enter your choice: ");
                string filterChoice = Console.ReadLine();

                switch (filterChoice)
                {
                    case "1":
                        Console.Write("Enter pizza type: ");
                        string type = Console.ReadLine();
                        menuItemsToDisplay = menu.GetMenuItemsByType(type);
                        break;
                    case "2":
                        Console.Write("Enter pizza size (Small, Medium, Large): ");
                        string size = Console.ReadLine();
                        menuItemsToDisplay = menu.GetMenuItemsBySize(size);
                        break;
                    case "3":
                        Console.Write("Enter toppings (comma-separated): ");
                        string toppingsCriteria = Console.ReadLine();
                        menuItemsToDisplay = menu.SearchByToppings(toppingsCriteria);
                        break;
                    case "4":
                        menuItemsToDisplay = menu.GetAllMenuItems();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid filter choice.");
                        break;
                }
            }
        }
        public void ManageOrders()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\nOrder Management:");
                Console.WriteLine("----------------");
                Console.WriteLine("1. View Orders");
                Console.WriteLine("2. Update Order");
                Console.WriteLine("3. Cancel Order");
                Console.WriteLine("0. Back to Main Menu");

                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewOrders();
                        break;
                    case "2":
                        UpdateOrder();
                        break;
                    case "3":
                        CancelOrder();
                        break;
                    case "0":
                        return; // Go back to main menu
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }
        public void ViewOrders()
        {
            if (orders.Count == 0)
            {
                Console.WriteLine("\nNo orders placed yet.");
                return;
            }

            Console.WriteLine("\nYour Orders:");
            for (int i = 0; i < orders.Count; i++)
            {
                Console.WriteLine($"{i + 1}. Order ID: {orders[i].OrderId}, Status: {orders[i].Status}");
            }

            Console.Write("\nEnter order number to view details (0 to go back): ");
            if (int.TryParse(Console.ReadLine(), out int orderNumber) && orderNumber >= 1 && orderNumber <= orders.Count)
            {
                Console.WriteLine(orders[orderNumber - 1].GetOrderDetails());
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
            else if (orderNumber != 0)
            {
                Console.WriteLine("Invalid order number.");
            }
        }

        public void UpdateOrder()
        {
            Console.Write("\nEnter order ID to update: ");
            if (!int.TryParse(Console.ReadLine(), out int orderId) || !orders.Any(o => o.OrderId == orderId))
            {
                Console.WriteLine("Order not found.");
                return;
            }

            Order order = orders.First(o => o.OrderId == orderId);
            Console.WriteLine(order.GetOrderDetails());

            while (true)
            {
                Console.WriteLine("\nUpdate options:");
                Console.WriteLine("1. Add Pizza");
                Console.WriteLine("2. Remove Pizza");
                Console.WriteLine("3. Update Customer Details");
                Console.WriteLine("0. Back to Manage Orders");

                Console.Write("Enter your choice: ");
                string updateChoice = Console.ReadLine();

                switch (updateChoice)
                {
                    case "1":
                        AddPizzaToExistingOrder(order);
                        break;
                    case "2":
                        RemovePizzaFromOrder(order);
                        break;
                    case "3":
                        UpdateCustomerDetails(order);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }

                Console.WriteLine("\nUpdated Order Details:");
                Console.WriteLine(order.GetOrderDetails());

                Console.Write("Confirm changes? (y/n): ");
                string confirm = Console.ReadLine();
                if (confirm.ToLower() != "y")
                {
                    Console.WriteLine("Changes discarded.");
                    return;
                }

                // Save the updated order (if using persistent storage)
            }
        }
        private void AddPizzaToExistingOrder(Order order)
        {
            DisplayMenu();
            Console.Write("\nEnter the number of the pizza you want to add: ");
            if (int.TryParse(Console.ReadLine(), out int pizzaNumber) && pizzaNumber >= 1 && pizzaNumber <= menu.GetAllMenuItems().Count)
            {
                MenuItem selectedMenuItem = menu.GetMenuItem(pizzaNumber);
                Pizza pizza = new Pizza(selectedMenuItem);
                pizza = TakePizzaCustomization(pizza);
                order.AddPizza(pizza);
                Console.WriteLine($"{pizza.GetOrderDetails()} added to the order.");
            }
            else
            {
                Console.WriteLine("Invalid pizza number.");
            }
        }

        private void RemovePizzaFromOrder(Order order)
        {
            Console.WriteLine("\nPizzas in the order:");
            for (int i = 0; i < order.Pizzas.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {order.Pizzas[i].GetOrderDetails()}");
            }
            Console.Write("Enter the number of the pizza you want to remove: ");
            if (int.TryParse(Console.ReadLine(), out int pizzaNumber) && pizzaNumber >= 1 && pizzaNumber <= order.Pizzas.Count)
            {
                order.RemovePizza(order.Pizzas[pizzaNumber - 1]);
                Console.WriteLine("Pizza removed from the order.");
            }
            else
            {
                Console.WriteLine("Invalid pizza number.");
            }
        }
        private void UpdateCustomerDetails(Order order)
        {
            Console.WriteLine("\nEnter new customer details:");
            Console.Write("Name: ");
            string name = Console.ReadLine();
            Console.Write("Address: ");
            string address = Console.ReadLine();
            Console.Write("Phone Number: ");
            string phoneNumber = Console.ReadLine();
            order.UpdateCustomerDetails(name, address, phoneNumber);
            Console.WriteLine("Customer details updated!");
        }

        public void CancelOrder()
        {
            Console.Write("Enter order ID to cancel: ");
            if (int.TryParse(Console.ReadLine(), out int orderId) && orders.Any(o => o.OrderId == orderId))
            {
                Order orderToCancel = orders.First(o => o.OrderId == orderId);
                Console.WriteLine(orderToCancel.GetOrderDetails());

                Console.Write("Are you sure you want to cancel this order? (y/n): ");
                string confirm = Console.ReadLine();
                if (confirm.ToLower() == "y")
                {
                    orderToCancel.UpdateStatus(OrderStatus.CANCELLED);
                    Console.WriteLine("Order cancelled successfully.");
                }
                else
                {
                    Console.WriteLine("Cancellation aborted.");
                }
            }
            else
            {
                Console.WriteLine("Order not found.");
            }
        }

        public void SearchPizzas()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\nSearch Pizzas:");
                Console.WriteLine("----------------");
                Console.WriteLine("1. Search by Type");
                Console.WriteLine("2. Search by Size");
                Console.WriteLine("3. Search by Toppings");
                Console.WriteLine("0. Back to Main Menu");
                Console.Write("Enter your choice: ");

                string searchChoice = Console.ReadLine();
                switch (searchChoice)
                {
                    case "1":
                        SearchPizzasByType();
                        break;
                    case "2":
                        SearchPizzasBySize();
                        break;
                    case "3":
                        SearchPizzasByToppings();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }
        private void SearchPizzasByType()
        {
            Console.Write("Enter pizza type: ");
            string type = Console.ReadLine();
            List<MenuItem> filteredItems = menu.GetMenuItemsByType(type);
            DisplayMenuItems(filteredItems, "Pizzas matching type:");
        }

        private void SearchPizzasBySize()
        {
            Console.Write("Enter pizza size (Small, Medium, Large): ");
            string size = Console.ReadLine();
            List<MenuItem> filteredItems = menu.GetMenuItemsBySize(size);
            DisplayMenuItems(filteredItems, "Pizzas matching size:");
        }

        private void SearchPizzasByToppings()
        {
            Console.Write("Enter toppings (comma-separated): ");
            string toppingsCriteria = Console.ReadLine();
            List<MenuItem> filteredItems = menu.SearchByToppings(toppingsCriteria);
            DisplayMenuItems(filteredItems, "Pizzas matching toppings:");
        }

        private void DisplayMenuItems(List<MenuItem> items, string heading)
        {
            if (items.Count == 0)
            {
                Console.WriteLine("\nNo pizzas found matching your criteria.");
            }
            else
            {
                Console.WriteLine($"\n{heading}");
                for (int i = 0; i < items.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {items[i].GetDescription()}");
                }
            }
        }
    }
}
