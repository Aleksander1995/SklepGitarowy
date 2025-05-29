// Aleksander Czajkowski – Sklep gitarowy
// Aplikacja konsolowa .NET 8 z bazą danych (ocena 4,5)

using System;
using System.Collections.Generic;
using System.Linq;

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
                        int id = int.Parse(Console.ReadLine());
                        Console.Write("Podaj ilość: ");
                        int qty = int.Parse(Console.ReadLine());
                        store.AddToCart(id, qty, cart);
                        break;
                    case "3":
                        cart.DisplayCart();
                        Console.Write("Podaj ID produktu do usunięcia: ");
                        int removeId = int.Parse(Console.ReadLine());
                        Console.Write("Podaj ilość do usunięcia: ");
                        int removeQty = int.Parse(Console.ReadLine());
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

    class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

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
        private List<Product> products;

        public Store()
        {
            products = new List<Product>
            {
                new Product(1, "Gitara akustyczna Yamaha", 999.99m, 5),
                new Product(2, "Gitara elektryczna Ibanez", 1499.99m, 3),
                new Product(3, "Wzmacniacz Fender", 799.99m, 4),
                new Product(4, "Struny D'Addario", 39.99m, 10),
                new Product(5, "Pasek gitarowy Ernie Ball", 59.99m, 7)
            };
        }

        public void DisplayProducts()
        {
            Console.WriteLine("\n=== Lista produktów ===");
            foreach (var p in products)
            {
                Console.WriteLine($"{p.Id}. {p.Name} - {p.Price:C} (Dostępne: {p.Quantity})");
            }
        }

        public void AddToCart(int id, int qty, Cart cart)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
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
                var product = products.First(p => p.Id == item.Product.Id);
                product.Quantity -= item.Quantity;
            }

            Console.WriteLine("Dziękujemy za zakupy!");
            cart.Clear();
        }
    }
}
