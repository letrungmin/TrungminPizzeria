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
        private Menu menu;
        private PizzaRepository repository;

        public ConsoleUI(Menu menu, PizzaRepository repository)
        {
            this.menu = menu;
            this.repository = repository;
        }

        public void Start()
        {
            while (true)
            {
                Console.WriteLine("Welcome to Trungmin's Pizzeria!");
                Console.WriteLine("1. View Menu");
                Console.WriteLine("2. Place Order");
                Console.WriteLine("3. Manage Orders"); // Add option to manage orders
                Console.WriteLine("4. Exit");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        DisplayMenu();
                        break;
                    case "2":
                        PlaceOrder();
                        break;
                    case "3":
                        ManageOrders();
                        break;
                    case "4":
                        Environment.Exit(0); // Exit the application
                        break;
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
                Console.WriteLine("\nMenu:");
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

                string filterChoice = Console.ReadLine();

                switch (filterChoice)
                {
                    case "1":
                        Console.Write("Enter pizza type: ");
                        string type = Console.ReadLine();
                        menuItemsToDisplay = menu.GetMenuItemsByType(type);
                        break;
                    // ... (Similar cases for filtering by size and toppings)
                    case "4":
                        menuItemsToDisplay = menu.GetAllMenuItems();
                        break;
                    case "0":
                        return; // Return to the main menu
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

        // ... Add more methods for managing orders, searching for pizzas, etc.

        public void PlaceOrder()
        {
            Customer customer = TakeCustomerDetails();
            Console.Write("Enter delivery address: ");
            string address = Console.ReadLine();

            Order order = new Order(customer, address);

            while (true)
            {
                DisplayMenu();

                Console.Write("Enter the number of the pizza you want to order (0 to finish): ");
                string pizzaChoice = Console.ReadLine();

                if (pizzaChoice == "0")
                {
                    break; // Exit pizza selection loop
                }
                // ... (rest of the order placement logic)
            }
        }

        // Other methods for managing orders, searching for pizzas, etc.
        // ...
    }


}
