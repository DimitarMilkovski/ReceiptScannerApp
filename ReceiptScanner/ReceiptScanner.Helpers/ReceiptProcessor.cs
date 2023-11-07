using ReceiptScanner.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptScanner.Helpers
{
    public class ReceiptProcessor
    {
        private async Task<List<Product>> GetProducts()
        {
            string apiUrl = "https://interview-task-api.mca.dev/qr-scanner-codes/alpha-qr-gFpwhsQ8fkY1";

            using(HttpResponseMessage response = await ApiHelper.Client.GetAsync(apiUrl))
            {
                if(response.IsSuccessStatusCode)
                {
                    List<Product> Products = await response.Content.ReadAsAsync<List<Product>>();
                    return Products;
                }

                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        private List<Product> GroupAndSortProductsByOrigin(List<Product> products, bool domestic) 
        {
            if(domestic)
            {
                List<Product> domesticProducts = products.Where(x=>x.Domestic == true)
                    .OrderBy(x=>x.Name)
                    .ToList();
                return domesticProducts;
            }
            else
            {
                List<Product> importedProducts = products.Where(x => x.Domestic == false)
                    .OrderBy(x=>x.Name)
                    .ToList();
                return importedProducts;
            }
        }

        public async Task GenerateReceipt ()
        {
            var products = await GetProducts();

            if(products.Count == 0)
            {
                throw new Exception("No products were obtained!");
            }

            List<Product> domesticProducts = GroupAndSortProductsByOrigin(products, true);
            List<Product> importedProducts = GroupAndSortProductsByOrigin(products, false);

            decimal domesticProductsTotalPrice = domesticProducts.Aggregate(0m, (sum, product) => sum + product.Price);
            decimal importedProductsTotalPrice = importedProducts.Aggregate(0m, (sum, product) => sum + product.Price);

            TextHelpers.WriteLineInColor(". Domestic", ConsoleColor.Green);
            foreach(Product product in domesticProducts)
            {
                TextHelpers.PrintProduct(product);
            }

            TextHelpers.WriteLineInColor(". Imported", ConsoleColor.Red);
            foreach (Product product in importedProducts)
            {
                TextHelpers.PrintProduct(product);
            }

            TextHelpers.WriteLineInColor($"Domestic cost: ${domesticProductsTotalPrice}", ConsoleColor.Green);
            TextHelpers.WriteLineInColor($"Imported cost: ${importedProductsTotalPrice}", ConsoleColor.Red);
            Console.WriteLine($"Domestic count: {domesticProducts.Count}");
            Console.WriteLine($"Imported count: {importedProducts.Count}");



        }

    }
}
