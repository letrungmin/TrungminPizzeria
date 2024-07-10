using System;
using System.Collections.Generic;
using System.Linq;

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
                    break; // Exit the display loop
                }

                Console.Clear();
                Console.WriteLine("\n*** MENU ***");

                var menuItemsByCategory = menuItemsToDisplay.GroupBy(mi => mi.Pizza?.Type ?? "Unknown"); // Handle potential null Pizza
                foreach (var category in menuItemsByCategory)
                {
                    Console.WriteLine($"\n{category.Key}:"); // Display category header
                    int itemNumber = 1;
                    foreach (var menuItem in category)
                    {
                        Console.WriteLine($"{itemNumber}. {menuItem?.GetDescription() ?? "Unavailable"}"); // Handle potential null menuItem
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
                        if (menuItemsToDisplay == null || menuItemsToDisplay.Count == 0)
                        {
                            Console.WriteLine("No pizzas found with that type.");
                            continue; // Stay in the filtering loop to allow another search
                        }
                        break;
                    case "2":
                        Console.Write("Enter pizza size (Small, Medium, Large): ");
                        string size = Console.ReadLine();
                        menuItemsToDisplay = menu.GetMenuItemsBySize(size);
                        if (menuItemsToDisplay == null || menuItemsToDisplay.Count == 0)
                        {
                            Console.WriteLine("No pizzas found with that size.");
                            continue; // Stay in the filtering loop to allow another search
                        }
                        break;
                    case "3":
                        Console.Write("Enter toppings (comma-separated): ");
                        string toppingsCriteria = Console.ReadLine();
                        menuItemsToDisplay = menu.SearchByToppings(toppingsCriteria);
                        if (menuItemsToDisplay == null || menuItemsToDisplay.Count == 0)
                        {
                            Console.WriteLine("No pizzas found with those toppings.");
                            continue; // Stay in the filtering loop to allow another search
                        }
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

                Console.WriteLine("\nEnter the number of the pizza you want to order (0 to finish):");
                string pizzaChoiceStr = Console.ReadLine();

                if (pizzaChoiceStr == "0")
                {
                    break; // Thoát vòng lặp nếu người dùng nhập 0
                }

                // Kiểm tra xem lựa chọn có phải là số hợp lệ không
                if (!int.TryParse(pizzaChoiceStr, out int pizzaIndex) || pizzaIndex < 1 || pizzaIndex > menu.GetAllMenuItems().Count)
                {
                    Console.WriteLine("Invalid pizza number. Please enter a valid number from the menu.");
                    continue; // Tiếp tục vòng lặp nếu lựa chọn không hợp lệ
                }

                // Lấy MenuItem dựa trên lựa chọn của người dùng
                MenuItem selectedMenuItem = menu.GetMenuItem(pizzaIndex);

                // Hiển thị các size pizza có sẵn và yêu cầu người dùng chọn
                Console.WriteLine($"\nYou selected a {selectedMenuItem.Pizza.Type} pizza. Choose a size:");
                var sizes = new[] { "Small", "Medium", "Large" };
                for (int i = 0; i < sizes.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {sizes[i]}");
                }

                // Lấy lựa chọn kích thước từ người dùng
                int sizeChoice;
                if (!int.TryParse(Console.ReadLine(), out sizeChoice) || sizeChoice < 1 || sizeChoice > sizes.Length)
                {
                    Console.WriteLine("Invalid size choice.");
                    continue; // Tiếp tục vòng lặp nếu lựa chọn không hợp lệ
                }

                string selectedSize = sizes[sizeChoice - 1];

                // Tìm kiếm MenuItem chính xác dựa trên loại và kích thước pizza
                selectedMenuItem = menu.GetAllMenuItems().FirstOrDefault(item =>
                    item.Pizza.Type == selectedMenuItem.Pizza.Type && item.Size == selectedSize);

                if (selectedMenuItem == null)
                {
                    Console.WriteLine("Invalid pizza and size combination.");
                    continue;
                }

                // Tạo một Pizza mới dựa trên MenuItem đã chọn
                Pizza pizza = new Pizza(selectedMenuItem);

                // Cho phép tùy chỉnh topping
                pizza = TakePizzaCustomization(pizza);

                // Thêm pizza vào đơn hàng
                order.AddPizza(pizza);
                Console.WriteLine($"{pizza.GetOrderDetails()} added to your order.");
            }

            // Kiểm tra xem đơn hàng có trống không
            if (order.Pizzas.Count == 0)
            {
                Console.WriteLine("Your order is empty.");
            }
            else
            {
                // Hiển thị tóm tắt đơn hàng và yêu cầu xác nhận
                Console.WriteLine("\nOrder Summary:");
                Console.WriteLine(order.GetOrderDetails());
                Console.Write("\nConfirm order? (yes/no): ");
                string confirm = Console.ReadLine()?.ToLower();

                // Kiểm tra xác nhận
                if (confirm == "yes")
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

        public void UpdateOrder()
        {
            Console.Write("Enter order ID to update: ");
            int orderId;
            if (!int.TryParse(Console.ReadLine(), out orderId) || !orders.Any(o => o.OrderId == orderId))
            {
                Console.WriteLine("Order not found.");
                return;
            }

            Order orderToUpdate = orders.First(o => o.OrderId == orderId);
            while (true)
            {
                Console.WriteLine(orderToUpdate.GetOrderDetails());
                Console.WriteLine("\nUpdate Options:");
                Console.WriteLine("1. Add pizza");
                Console.WriteLine("2. Remove pizza");
                Console.WriteLine("3. Update customer details");
                Console.WriteLine("4. Update order status");
                Console.WriteLine("0. Back to order management menu");
                Console.Write("Enter your choice: ");
                string updateChoice = Console.ReadLine();

                switch (updateChoice)
                {
                    case "1":
                        AddPizzaToExistingOrder(orderToUpdate);
                        break;
                    case "2":
                        RemovePizzaFromOrder(orderToUpdate);
                        break;
                    case "3":
                        UpdateCustomerDetails(orderToUpdate);
                        break;
                    case "4":
                        UpdateOrderStatus(orderToUpdate);
                        break;
                    case "0": return;
                    default: Console.WriteLine("Invalid choice."); break;
                }

                // Update the order in the list
                orders[orders.FindIndex(o => o.OrderId == orderId)] = orderToUpdate;
            }
        }

        private void AddPizzaToExistingOrder(Order order)
        {
            DisplayMenu();
            Console.Write("\nEnter the number of the pizza you want to add (0 to cancel): ");
            string pizzaChoice = Console.ReadLine();
            if (pizzaChoice == "0")
                return;

            int pizzaNumber;
            if (int.TryParse(pizzaChoice, out pizzaNumber) && pizzaNumber >= 1 && pizzaNumber <= menu.GetAllMenuItems().Count)
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

            Console.Write("Enter the number of the pizza you want to remove (0 to cancel): ");
            string pizzaChoice = Console.ReadLine();

            if (pizzaChoice == "0")
                return;

            int pizzaNumber;
            if (int.TryParse(pizzaChoice, out pizzaNumber) && pizzaNumber >= 1 && pizzaNumber <= order.Pizzas.Count)
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

        private void UpdateOrderStatus(Order order)
        {
            Console.WriteLine("\nCurrent order status: " + order.Status);
            Console.WriteLine("Enter new status (Placed, Preparing, Baking, Out for Delivery, Delivered, Cancelled):");
            string newStatus = Console.ReadLine();

            if (Enum.TryParse(newStatus, true, out OrderStatus parsedStatus))
            {
                order.UpdateStatus(parsedStatus);
                Console.WriteLine("Order status updated to: " + order.Status);
            }
            else
            {
                Console.WriteLine("Invalid order status.");
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
