using System;
using System.Collections.Generic;
using System.Linq;

namespace Shopping_system_project_using_generics
{
    internal class Program
    {
        // Stack to store actions for undo functionality
        static Stack<string> actions = new Stack<string>();

        // List to store items in the cart
        public static List<string> CartList = new List<string>();

        // Dictionary to store item prices
        static public Dictionary<string, double> ItemPrices = new Dictionary<string, double>()
        {
            {"camera",1500 },
            {"laptop", 3000 },
            {"tv", 2500 },
        };

        static void Main(string[] args)
        {
            while (true)
            {
                // Display the main menu options
                DisplayMenu();

                // Read user's choice
                string choice = Console.ReadLine();
                int intChoice = Convert.ToInt32(choice);

                // Handle the user's choice
                HandleChoice(intChoice);
            }
        }

        // Display the main menu options
        private static void DisplayMenu()
        {
            Console.WriteLine("Welcome to our shop");
            Console.WriteLine("========================");
            Console.WriteLine("1- Add item to cart");
            Console.WriteLine("2- View cart items");
            Console.WriteLine("3- Remove item from cart");
            Console.WriteLine("4- Checkout");
            Console.WriteLine("5- Undo");
            Console.WriteLine("6- Exit");
            Console.WriteLine("Enter your choice number");
        }

        // Handle user's choice from the main menu
        private static void HandleChoice(int choice)
        {
            switch (choice)
            {
                case 1:
                    AddItem();
                    break;
                case 2:
                    ViewCart();
                    break;
                case 3:
                    RemoveItem();
                    break;
                case 4:
                    Checkout();
                    break;
                case 5:
                    Undo();
                    break;
                case 6:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid Choice");
                    break;
            }
        }

        // Add an item to the cart
        private static void AddItem()
        {
            Console.WriteLine("Available Items");
            foreach (var item in ItemPrices)
            {
                Console.WriteLine($"Items: {item.Key} Price: {item.Value}");
            }
            Console.WriteLine("Please enter product name");
            string cartItem = Console.ReadLine();
            if (ItemPrices.ContainsKey(cartItem))
            {
                CartList.Add(cartItem);
                actions.Push($"Item {cartItem} added to cart");
                Console.WriteLine($"item {cartItem} is added to your cart");
            }
            else
            {
                Console.WriteLine("item is out of stock or not available");
            }
        }

        // View items in the cart
        private static void ViewCart()
        {
            var ItemPriceCollection = GetCartPrices();
            Console.WriteLine("Your cart items:");
            if (CartList.Any())
            {
                foreach (var item in ItemPriceCollection)
                {
                    Console.WriteLine($"item :{item.Item1}, Price:{item.Item2} ");
                }
            }
            else
            {
                Console.WriteLine("cart is empty");
            }
        }

        // Get prices of items in the cart
        private static IEnumerable<Tuple<string, double>> GetCartPrices()
        {
            var cartPrices = new List<Tuple<string, double>>();
            foreach (var item in CartList)
            {
                double price = 0;
                bool foundItem = ItemPrices.TryGetValue(item, out price);
                if (foundItem)
                {
                    Tuple<string, double> itemPrice = new Tuple<string, double>(item, price);
                    cartPrices.Add(itemPrice);
                }

            }
            return cartPrices; 
        }

        // Remove an item from the cart
        private static void RemoveItem()
        {
            ViewCart();
            if (CartList.Any())
            {
                Console.WriteLine("Please select item to remove");
                var itemToRemove = Console.ReadLine();
                if (CartList.Contains(itemToRemove))
                {
                    CartList.Remove(itemToRemove);
                    actions.Push($"Item {itemToRemove} removed from cart");

                    Console.WriteLine($"Item {itemToRemove} removed");
                }
                else
                {
                    Console.WriteLine("Item doesnt exist");
                }
            }
        }

        // Checkout and clear the cart
        private static void Checkout()
        {
            if (CartList.Any())
            {
                double totalPrice = 0;
                Console.WriteLine("Your cart items are:");
                IEnumerable<Tuple<string, double>> ItemsInCart = GetCartPrices();
                foreach (var item in ItemsInCart)
                {
                    totalPrice += item.Item2;
                    Console.WriteLine(item.Item1 + " " + item.Item2);
                }
                Console.WriteLine($"total price is {totalPrice} ");
                Console.WriteLine("please preceed to paying, thank you!");
                CartList.Clear();
                actions.Push("checkout");

            }
            else
            {
                Console.WriteLine("Your cart is empty");
            }
        }

        // Undo the last action
        private static void Undo()
        {
            if (actions.Count > 0)
            {
                string LastAction = actions.Pop();
                Console.WriteLine($"your last action is {LastAction} ");
                var actionArray = LastAction.Split();
                if (LastAction.Contains("added"))
                {
                    CartList.Remove(actionArray[1]);
                }
                else if (LastAction.Contains("removed"))
                {
                    CartList.Add(actionArray[1]);
                }
                else
                {
                    Console.WriteLine("checkout cannot be undo ");
                }
            }
        }
    }
}
