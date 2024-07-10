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
                DisplayMainMenu();
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

        private void DisplayMainMenu()
        {
            Console.WriteLine("\nWelcome to Trungmin's Pizzeria!");
            Console.WriteLine("----------------------------");
            Console.WriteLine("1. View Menu");
            Console.WriteLine("2. Place Order");
            Console.WriteLine("3. Manage Orders");
            Console.WriteLine("4. Exit");
            Console.WriteLine("----------------------------");
        }

        public void DisplayMenu()
        {
            List<MenuItem> menuItemsToDisplay = menu.GetAllMenuItems();

            while (true)
            {
                if (menuItemsToDisplay.Count == 0)
                {
                    Console.WriteLine("\nNo pizzas match your criteria or the menu is empty.");
                    break;
                }

                Console.Clear();
                Console.WriteLine("\n*** MENU ***");

                var menuItemsByCategory = menuItemsToDisplay.GroupBy(mi => mi.Pizza.Type);
                foreach (var category in menuItemsByCategory)
                {
                    Console.WriteLine($"\n{category.Key}:");
                    int itemNumber = 1;
                    foreach (var menuItem in category)
                    {
                        Console.WriteLine($"{itemNumber}. {menuItem.GetDescription()}");
                        itemNumber++;
                    }
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

        public Customer TakeCustomerDetails()
        {
            Console.WriteLine("\nEnter customer details:");
            Console.Write("Name: ");
            string name = Console.ReadLine();
            Console.Write("Address: ");
            string address = Console.ReadLine();
            Console.Write("Phone Number: ");
            string phoneNumber = Console.ReadLine();

            return new Customer(name, address, phoneNumber);
        }

        public Pizza TakePizzaCustomization(Pizza pizza)
        {
            List<Topping> availableToppings = repository.GetAllToppings();

            while (true)
            {
                Console.WriteLine("\nAvailable toppings:");
                for (int i = 0; i < availableToppings.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {availableToppings[i].Name} (+${availableToppings[i].Price:F2})");
                }
                Console.WriteLine("0. Done adding toppings");

                Console.Write("Enter your choice: ");
                string toppingChoice = Console.ReadLine();

                if (toppingChoice == "0")
                    break;

                int toppingIndex;
                if (int.TryParse(toppingChoice, out toppingIndex) && toppingIndex >= 1 && toppingIndex <= availableToppings.Count)
                {
                    pizza.AddTopping(availableToppings[toppingIndex - 1]);
                    Console.WriteLine($"Added {availableToppings[toppingIndex - 1].Name} to your pizza.");
                }
                else
                {
                    Console.WriteLine("Invalid topping choice.");
                }
            }

            return pizza;
        }
        public void PlaceOrder()
        {
            Customer customer = TakeCustomerDetails();
            Console.Write("Enter delivery address: ");
            string address = Console.ReadLine();

            Order order = new Order(customer, address);
            while (true)
            {
                DisplayMenu();
                Console.Write("\nEnter the number of the pizza you want to order (0 to finish): ");
                string pizzaChoice = Console.ReadLine();
                if (pizzaChoice == "0")
                {
                    break;
                }

                int pizzaNumber;
                if (int.TryParse(pizzaChoice, out pizzaNumber) && pizzaNumber >= 1 && pizzaNumber <= menu.GetAllMenuItems().Count)
                {
                    MenuItem selectedMenuItem = menu.GetMenuItem(pizzaNumber);
                    Pizza pizza = new Pizza(selectedMenuItem);
                    pizza = TakePizzaCustomization(pizza);
                    order.AddPizza(pizza);
                    Console.WriteLine($"{pizza.GetOrderDetails()} added to your order.");
                }
                else
                {
                    Console.WriteLine("Invalid pizza number.");
                }
            }
            if (order.Pizzas.Count == 0)
            {
                Console.WriteLine("Your order is empty.");
            }
            else
            {
                Console.WriteLine(order.GetOrderDetails());
                Console.Write("Confirm order? (y/n): ");
                string confirm = Console.ReadLine();
                if (confirm.ToLower() == "y")
                {
                    orders.Add(order);
                    Console.WriteLine("Order placed successfully! Your order ID is: " + order.OrderId);
                }
                else
                {
                    Console.WriteLine("Order cancelled.");
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
                        return;
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
                Console.WriteLine("No orders found.");
                return;
            }

            Console.WriteLine("\nOrders:");
            for (int i = 0; i < orders.Count; i++)
            {
                Console.WriteLine($"{i + 1}. Order ID: {orders[i].OrderId}, Status: {orders[i].Status}");
            }

            Console.Write("Enter order number to view details (0 to go back): ");
            int orderNumber;
            if (int.TryParse(Console.ReadLine(), out orderNumber) && orderNumber >= 1 && orderNumber <= orders.Count)
            {
                Console.WriteLine(orders[orderNumber - 1].GetOrderDetails());
            }
            else if (orderNumber != 0)
            {
                Console.WriteLine("Invalid order number.");
            }
        }

        public void UpdateOrder()
        {
            Console.Write("Enter order ID to update: ");
            int orderId;
            if (int.TryParse(Console.ReadLine(), out orderId))
            {
                Order orderToUpdate = orders.FirstOrDefault(o => o.OrderId == orderId);
                if (orderToUpdate != null)
                {
                    // ... (Logic for updating the order: add/remove pizzas, update customer details)
                }
                else
                {
                    Console.WriteLine("Order not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid order ID.");
            }
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