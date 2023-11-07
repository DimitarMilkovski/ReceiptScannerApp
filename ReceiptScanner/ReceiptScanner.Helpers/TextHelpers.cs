using ReceiptScanner.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptScanner.Helpers
{
    public class TextHelpers
    {
        public static void WriteLineInColor(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void PrintProduct(Product product)
        {
            Console.WriteLine($"... {product.Name}");
            Console.WriteLine($"    Price: ${product.Price}");
            Console.WriteLine($"    {product.Description.Substring(0, 10)}...");
            Console.WriteLine((product.Weight != null) ? $"    Weight: {product.Weight}g" : "    Weight: N/A");
        }
    }
}
