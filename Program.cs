using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SklepGitarowy
{
    class Program
    {
        static void Main(string[] args)
        {
            Store store = new Store();
            Cart cart = new Cart();

            while (true)
            {
                Console.WriteLine("\n=== SKLEP GITAROWY ===");
                Console.WriteLine("1. Wyświetl produkty");
                Console.WriteLine("2. Dodaj produkt do koszyka");
                Console.WriteLine("3. Usuń produkt z koszyka");
                Console.WriteLine("4. Pokaż koszyk");
                Console.WriteLine("5. Finalizuj zakup");
                Console.WriteLine("0. Wyjdź");

                Console.Write("\nWybierz opcję: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        store.DisplayProducts();
                        break;
                    case "2":
                        store.DisplayProducts();
                        Console.Write("Podaj ID produktu: ");
                        if (!int.TryParse(Console.ReadLine(), out int id))
                        {
                            Console.WriteLine("Nieprawidłowe ID.");
                            break;
                        }
                        Console.Write("Podaj ilość: ");
                        if (!int.TryParse(Console.ReadLine(), out int qty))
                        {
                            Console.WriteLine("Nieprawidłowa ilość.");
                            break;
                        }
                        store.AddToCart(id, qty, cart);
                        break;
                    case "3":
                        cart.DisplayCart();
                        Console.Write("Podaj ID produktu do usunięcia: ");
                        if (!int.TryParse(Console.ReadLine(), out int removeId))
                        {
                            Console.WriteLine("Nieprawidłowe ID.");
                            break;
                        }
                        Console.Write("Podaj ilość do usunięcia: ");
                        if (!int.TryParse(Console.ReadLine(), out int removeQty))
                        {
                            Console.WriteLine("Nieprawidłowa ilość.");
                            break;
                        }
                        cart.RemoveItem(removeId, removeQty);
                        break;
                    case "4":
                        cart.DisplayCart();
                        break;
                    case "5":
                        store.FinalizePurchase(cart);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Nieprawidłowa opcja.");
                        break;
                }
            }
        }
    }

    public class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }
        public List<OrderItem> Items { get; set; } = new();
    }

    public class OrderItem
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int OrderId { get; set; }
        public Order? Order { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public Product() {}

        public Product(int id, string name, decimal price, int quantity)
        {
            Id = id;
            Name = name;
            Price = price;
            Quantity = quantity;
        }
    }

    class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }

        public CartItem(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }
    }

    class Cart
    {
        private List<CartItem> items = new List<CartItem>();

        public void AddItem(Product product, int quantity)
        {
            var item = items.FirstOrDefault(i => i.Product.Id == product.Id);
            if (item != null)
            {
                item.Quantity += quantity;
            }
            else
            {
                items.Add(new CartItem(product, quantity));
            }
        }

        public void RemoveItem(int productId, int quantity)
        {
            var item = items.FirstOrDefault(i => i.Product.Id == productId);
            if (item != null)
            {
                item.Quantity -= quantity;
                if (item.Quantity <= 0)
                {
                    items.Remove(item);
                }
            }
        }

        public void DisplayCart()
        {
            Console.WriteLine("\n=== Zawartość koszyka ===");
            if (!items.Any())
            {
                Console.WriteLine("Koszyk jest pusty.");
                return;
            }

            foreach (var item in items)
            {
                Console.WriteLine($"ID: {item.Product.Id} | {item.Product.Name} - {item.Quantity} x {item.Product.Price:C} = {(item.Quantity * item.Product.Price):C}");
            }
            Console.WriteLine($"\nŁącznie: {GetTotalPrice():C}");
        }

        public decimal GetTotalPrice()
        {
            return items.Sum(i => i.Product.Price * i.Quantity);
        }

        public List<CartItem> GetItems() => items;
        public void Clear() => items.Clear();
    }

    class Store
    {
        private readonly StoreDbContext _context = new StoreDbContext();

        public void DisplayProducts()
        {
            Console.WriteLine("\n=== Lista produktów ===");
            foreach (var p in _context.Products.ToList())
            {
                Console.WriteLine($"ID: {p.Id} | {p.Name} - {p.Price:C} (Dostępne: {p.Quantity})");
            }
        }

        public void AddToCart(int id, int qty, Cart cart)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                Console.WriteLine("Nie znaleziono produktu.");
                return;
            }
            if (qty > product.Quantity)
            {
                Console.WriteLine("Brak wystarczającej ilości na stanie.");
                return;
            }
            cart.AddItem(product, qty);
            Console.WriteLine("Dodano do koszyka.");
        }

        public void FinalizePurchase(Cart cart)
        {
            Console.WriteLine("\n=== Finalizacja zakupu ===");
            cart.DisplayCart();

            foreach (var item in cart.GetItems())
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == item.Product.Id);
                if (product != null)
                {
                    product.Quantity -= item.Quantity;
                }
            }

            _context.SaveChanges();

            var order = new Order
            {
                TotalAmount = cart.GetTotalPrice(),
                Items = cart.GetItems().Select(item => new OrderItem
                {
                    ProductName = item.Product.Name,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product.Price
                }).ToList()
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            Console.WriteLine("Dziękujemy za zakupy!");
            Console.WriteLine("Zamówienie zapisane.");
            cart.Clear();
        }
    }
} 
