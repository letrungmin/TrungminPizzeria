using OrderPizzaApp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TrungminPizzeria
{
    public class UI
    {
        public readonly Menu menu;
        public readonly PizzaRepository repository;
        public readonly List<Order> orders = new List<Order>();

        public UI(Menu menu, PizzaRepository repository)
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
                        Console.WriteLine("Thank you for visiting Trungmin's Pizzeria!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
        public void DisplayMainMenu()
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
            List<MenuItem> filteredMenuItems = menuItemsToDisplay.ToList(); // Copy the original list to preserve it
            string currentTypeFilter = null;
            string currentSizeFilter = null;
            string currentToppingsFilter = null;

            while (true)
            {
                if (filteredMenuItems.Count == 0)
                {
                    Console.WriteLine("\nNo pizzas match your criteria.");
                    Console.WriteLine("Press any key to clear filters and return to the menu...");
                    Console.ReadKey();
                    filteredMenuItems = menuItemsToDisplay.ToList(); // Reset the filtered list
                    currentTypeFilter = null;
                    currentSizeFilter = null;
                    currentToppingsFilter = null;
                    continue;
                }

                Console.Clear();
                Console.WriteLine("\n*** MENU ***");

                // Hiển thị loại filter hiện tại
                if (!string.IsNullOrEmpty(currentTypeFilter))
                {
                    Console.WriteLine($"Current type filter: {currentTypeFilter}");
                }
                if (!string.IsNullOrEmpty(currentSizeFilter))
                {
                    Console.WriteLine($"Current size filter: {currentSizeFilter}");
                }
                if (!string.IsNullOrEmpty(currentToppingsFilter))
                {
                    Console.WriteLine($"Current toppings filter: {currentToppingsFilter}");
                }

                // Hiển thị menu items with categories
                var menuItemsByCategory = filteredMenuItems.GroupBy(mi => mi.Pizza?.Type ?? "Unknown");
                int itemNumber = 1; // Biến đếm số thứ tự của menuItem
                foreach (var category in menuItemsByCategory)
                {
                    Console.WriteLine($"\n{category.Key}:"); // Hiển thị tên loại pizza
                    foreach (var menuItem in category)
                    {
                        Console.WriteLine($"{itemNumber}. {menuItem?.GetDescription() ?? "Unavailable"}"); // Hiển thị mô tả menuItem
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
                        currentTypeFilter = Console.ReadLine();
                        filteredMenuItems = menu.GetMenuItemsByType(currentTypeFilter).ToList(); // Lọc trên filteredMenuItems
                        break;
                    case "2":
                        Console.Write("Enter pizza size (Small, Medium, Large): ");
                        currentSizeFilter = Console.ReadLine();
                        filteredMenuItems = filteredMenuItems.Where(mi => mi.Size.Equals(currentSizeFilter, StringComparison.OrdinalIgnoreCase)).ToList(); // Lọc trên filteredMenuItems
                        break;
                    case "3":
                        Console.Write("Enter toppings (comma-separated): ");
                        currentToppingsFilter = Console.ReadLine();
                        filteredMenuItems = filteredMenuItems.Where(mi => menu.SearchByToppings(currentToppingsFilter).Contains(mi)).ToList(); // Lọc trên filteredMenuItems
                        break;
                    case "4":
                        filteredMenuItems = menu.GetAllMenuItems().ToList(); // Reset filters
                        currentTypeFilter = null; // Reset filters
                        currentSizeFilter = null;
                        currentToppingsFilter = null;
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid filter choice.");
                        break;
                }
            }
        }

        public void PlaceOrderMenu()
        {
            List<MenuItem> menuItemsToDisplay = menu.GetAllMenuItems();
            List<MenuItem> filteredMenuItems = menuItemsToDisplay.ToList(); // Tạo một bản sao để lưu kết quả lọc

            Console.WriteLine("\n*** MENU ***");

            //int itemNumber = 1; // Biến đếm số thứ tự của menuItem
                                // Hiển thị menu items with categories
            var menuItemsByCategory = filteredMenuItems.GroupBy(mi => mi.Pizza?.Type ?? "Unknown");
            int itemNumber = 1; // Biến đếm số thứ tự của menuItem
            foreach (var category in menuItemsByCategory)
            {
                Console.WriteLine($"\n{category.Key}:"); // Hiển thị tên loại pizza
                foreach (var menuItem in category)
                {
                    Console.WriteLine($"{itemNumber}. {menuItem?.GetDescription() ?? "Unavailable"}"); // Hiển thị mô tả menuItem
                    itemNumber++;
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
                for (int i = 0; i < availableToppings.Count; i++) // Đảm bảo duyệt qua tất cả các topping
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
            PizzaFactory pizzaFactory = new ConcretePizzaFactory();

            Order order = new Order(customer, address);

            while (true)
            {
                PlaceOrderMenu();

                Console.WriteLine("\nEnter the number of the pizza you want to order (0 to finish):");
                string pizzaChoiceStr = Console.ReadLine();

                if (pizzaChoiceStr == "0")
                {
                    break; // Exit pizza selection loop
                }

                // Kiểm tra xem lựa chọn có phải là số hợp lệ không
                if (!int.TryParse(pizzaChoiceStr, out int pizzaIndex) || pizzaIndex < 1 || pizzaIndex > menu.GetAllMenuItems().Count)
                {
                    Console.WriteLine("Invalid pizza number. Please enter a valid number from the menu.");
                    continue;
                }

                // Lấy MenuItem dựa trên lựa chọn của người dùng
                MenuItem selectedMenuItem = menu.GetMenuItem(pizzaIndex);

                //// Hiển thị các size pizza có sẵn và yêu cầu người dùng chọn
                //Console.WriteLine($"\nYou selected a {selectedMenuItem.Pizza.Type} pizza. Choose a size:");
                //var sizes = new[] { "Small", "Medium", "Large" };
                //for (int i = 0; i < sizes.Length; i++)
                //{
                //    Console.WriteLine($"{i + 1}. {sizes[i]}");
                //}

                //// Lấy lựa chọn kích thước từ người dùng
                //int sizeChoice;
                //if (!int.TryParse(Console.ReadLine(), out sizeChoice) || sizeChoice < 1 || sizeChoice > sizes.Length)
                //{
                //    Console.WriteLine("Invalid size choice.");
                //    continue;
                //}

                //string selectedSize = sizes[sizeChoice - 1];

                // Tạo một Pizza mới dựa trên MenuItem đã chọn
                Pizza pizza = pizzaFactory.CreatePizza(selectedMenuItem.Pizza.Type); // Sử dụng factory để tạo pizza

                // Cho phép tùy chỉnh topping
                pizza = TakePizzaCustomization(pizza);

                // Thêm pizza vào đơn hàng
                order.AddPizza(pizza);
                Console.WriteLine($"{pizza.GetOrderDetails()} added to your order.");
            }

            // ... (phần còn lại của phương thức PlaceOrder)
            // ... (previous code for taking customer details and adding pizzas to order)

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


        private bool IsValidCustomer(Customer customer)
        {
            return true;
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

            while (true)
            {
                Console.Write("Enter order ID to view details (0 to go back): ");
                string input = Console.ReadLine();

                if (input == "0")
                {
                    return; // Quay lại menu quản lý đơn hàng nếu người dùng nhập 0
                }

                if (int.TryParse(input, out int orderId))
                {
                    // Tìm kiếm đơn hàng theo orderId
                    Order orderToDisplay = orders.FirstOrDefault(o => o.OrderId == orderId);

                    if (orderToDisplay != null)
                    {
                        Console.WriteLine(orderToDisplay.GetOrderDetails());
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        break; // Thoát khỏi vòng lặp sau khi hiển thị chi tiết
                    }
                    else
                    {
                        Console.WriteLine("Order not found.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid order ID. Please enter a number.");
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
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }

                // Update the order in the list
                orders[orders.FindIndex(o => o.OrderId == orderId)] = orderToUpdate;
            }
        }

        

        private void AddPizzaToExistingOrder(Order order)
        {
            PlaceOrderMenu();
            PizzaFactory pizzaFactory = new ConcretePizzaFactory();

            Console.Write("\nEnter the number of the pizza you want to add (0 to cancel): ");
            string pizzaChoice = Console.ReadLine();

            if (pizzaChoice == "0")
                return;

            int pizzaNumber;
            if (int.TryParse(pizzaChoice, out pizzaNumber) && pizzaNumber >= 1 && pizzaNumber <= menu.GetAllMenuItems().Count)
            {
                MenuItem selectedMenuItem = menu.GetMenuItem(pizzaNumber);
   
                Pizza pizza = pizzaFactory.CreatePizza(selectedMenuItem.Pizza.Type);
                pizza = TakePizzaCustomization(pizza);
                order.AddPizza(pizza);
                Console.WriteLine($"{pizza.GetOrderDetails()} added to the order.");
            }
            else
            {
                Console.WriteLine("Invalid pizza number.");
            }
        }


        public void RemovePizzaFromOrder(Order order)
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

        public void UpdateCustomerDetails(Order order)
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

        public void UpdateOrderStatus(Order order)
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


    }
}
