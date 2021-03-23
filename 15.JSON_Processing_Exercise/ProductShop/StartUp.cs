using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DataTransferObjects;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        static IMapper mapper;
        public static void Main(string[] args)
        {
            var productShopContext = new ProductShopContext();

            //productShopContext.Database.EnsureDeleted();
            //productShopContext.Database.EnsureCreated();

            // ---------------- 2 ---------------------------
            //string inputJson = File.ReadAllText("../../../Datasets/users.json");
            //var result = ImportUsers(productShopContext, inputJson);

            // ---------------- 3 ---------------------------
            //string usersJson = File.ReadAllText("../../../Datasets/users.json");
            //string productsJson = File.ReadAllText("../../../Datasets/products.json");
            //ImportUsers(productShopContext, usersJson);
            //var result = ImportProducts(productShopContext, productsJson);


            // ---------------- 4 ---------------------------
            //string usersJson = File.ReadAllText("../../../Datasets/users.json");
            //string productsJson = File.ReadAllText("../../../Datasets/products.json");
            //string categoriesJson = File.ReadAllText("../../../Datasets/categories.json");
            //ImportUsers(productShopContext, usersJson);
            //ImportProducts(productShopContext, productsJson);
            //var result = ImportCategories(productShopContext, categoriesJson);

            // -------------- Read and Import Data -------------------------------------------------------------------
            //string usersJson = File.ReadAllText("../../../Datasets/users.json");
            //string productsJson = File.ReadAllText("../../../Datasets/products.json");
            //string categoriesJson = File.ReadAllText("../../../Datasets/categories.json");
            //string categoriesProductsJson = File.ReadAllText("../../../Datasets/categories-products.json");
            //ImportUsers(productShopContext, usersJson);
            //ImportProducts(productShopContext, productsJson);
            //ImportCategories(productShopContext, categoriesJson);
            //var result = ImportCategoryProducts(productShopContext, categoriesProductsJson);

            //Console.WriteLine(result);




            // -------------- Read and Export Data -------------------------------------------------------------------

            //var result = GetProductsInRange(productShopContext);
            //var result = GetSoldProducts(productShopContext);
            //var result = GetCategoriesByProductsCount(productShopContext);
            var result = GetUsersWithProducts(productShopContext);

            Console.WriteLine(result);

        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Include(x => x.ProductsSold)
                .ToList()
                .Where(x => x.ProductsSold.Any(b => b.BuyerId != null))
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    age = u.Age,
                    soldProducts = new
                    {
                        count = u.ProductsSold.Where(x => x.BuyerId != null).Count(),
                        products = u.ProductsSold.Where(x => x.BuyerId != null).Select(p => new
                        {
                            name = p.Name,
                            price = p.Price
                        })

                    }
                })
                .OrderByDescending(x => x.soldProducts.products.Count())
                .ToList();

            var resultObject = new
            {
                usersCount = users.Count(),
                users = users
            };

            var jsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var resultJson = JsonConvert.SerializeObject(resultObject, Formatting.Indented, jsonSerializerSettings);

            return resultJson;
        }


        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Select(c => new
                {
                    category = c.Name,
                    productsCount = c.CategoryProducts.Count,
                    averagePrice = c.CategoryProducts
                        .Average(p => p.Product.Price).ToString("F2"),
                    totalRevenue = c.CategoryProducts
                        .Sum(p => p.Product.Price).ToString("F2")
                })
                .OrderByDescending(x => x.productsCount)
                .ToList();

            var result = JsonConvert.SerializeObject(categories, Formatting.Indented);

            return result;
        }


        public static string GetSoldProducts(ProductShopContext context)
        {
            var user = context.Users
                .Where(x => x.ProductsSold.Any(p => p.BuyerId != null))
                .Select(x => new
                {
                    firstName = x.FirstName,
                    lastName = x.LastName,
                    soldProducts = x.ProductsSold.Where(p => p.BuyerId != null)
                        .Select(b => new
                        {
                            name = b.Name,
                            price = b.Price,
                            buyerFirstName = b.Buyer.FirstName,
                            buyerLastName = b.Buyer.LastName
                        })
                        .ToList()
                })
                .OrderBy(x => x.lastName)
                .ThenBy(x => x.firstName)
                .ToList();

            var result = JsonConvert.SerializeObject(user, Formatting.Indented);

            return result;
        }


        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .Select(x => new
                {
                    name = x.Name,
                    price = x.Price,
                    seller = x.Seller.FirstName + " " + x.Seller.LastName
                })
                .OrderBy(x => x.price)
                .ToList();

            var result = JsonConvert.SerializeObject(products, Formatting.Indented);

            return result;

            //return string.Join(Environment.NewLine, products.Select(x => x.name));
        }


        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            InitializeAutomapper();

            var dtoCategoryProducts = JsonConvert
                .DeserializeObject<IEnumerable<CategoryProductsInputModel>>(inputJson);

            var categoryProducts = mapper.Map<IEnumerable<CategoryProduct>>(dtoCategoryProducts);

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count()}";
        }


        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            InitializeAutomapper();

            var dtoCategories = JsonConvert
                .DeserializeObject<IEnumerable<CategoryInputModel>>(inputJson)
                .Where(c => c.Name != null)
                .ToList();

            var categories = mapper.Map<IEnumerable<Category>>(dtoCategories);

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count()}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {

            InitializeAutomapper();

            var dtoProducts = JsonConvert.DeserializeObject<IEnumerable<ProductInputModel>>(inputJson);

            var products = mapper.Map<IEnumerable<Product>>(dtoProducts);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count()}";
        }


        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            InitializeAutomapper();

            var dtoUsers = JsonConvert.DeserializeObject<IEnumerable<UserInputModel>>(inputJson);

            var users = mapper.Map<IEnumerable<User>>(dtoUsers);

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count()}";
        }

        private static void InitializeAutomapper()
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });

            mapper = config.CreateMapper();
        }
    }
}