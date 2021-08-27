using ETender.Products.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETender.Products.Data
{
    public class ProductContextSeed
    {
        public static void SeedData(IMongoCollection<Product> productCollection)
        {
            var existsProduct = productCollection.Find(x => true).Any();
            if (!existsProduct)
            {
                productCollection.InsertManyAsync(GetConfigureProducts());
            }
        }

        private static IEnumerable<Product> GetConfigureProducts()
        {
            return new List<Product>()
            {
                new Product()
                {
                    Name = "Test - 1",
                    Summary = "Test 1",
                    Description = "Lorem",
                    ImageFile = "product-2.png",
                    Price = 1576m,
                    Category = "White"
                },
                new Product()
                {
                    Name = "Test - 2",
                    Summary = "Test 2",
                    Description = "Lorem",
                    ImageFile = "product-2.png",
                    Price = 25m,
                    Category = "White"
                },
                new Product()
                {
                    Name = "Test - 3",
                    Summary = "Test 3",
                    Description = "Lorem",
                    ImageFile = "product-2.png",
                    Price = 12m,
                    Category = "Black"
                },
                new Product()
                {
                    Name = "Test - 4",
                    Summary = "Test 4",
                    Description = "Lorem",
                    ImageFile = "product-2.png",
                    Price = 54m,
                    Category = "White"
                },
                new Product()
                {
                    Name = "Test - 5",
                    Summary = "Test 5",
                    Description = "Lorem",
                    ImageFile = "product-2.png",
                    Price = 20m,
                    Category = "Black"
                },
            };
        }
    }
}
