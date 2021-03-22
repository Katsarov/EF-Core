using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;


namespace JsonDemoProjectTwo
{
    class Program
    {
        static void Main(string[] args)
        {
            var car = new Car
            {
                Extras = new List<string> { "Klimatronik", "4x4", "Farove" },
                ManufacturedOn = DateTime.Now,
                Model = "Golf",
                Vendor = "VW",
                Price = 12345.56M,
                Engine = new Engine { Volume = 1.6M, HorsePower = 80 }
            };

            //Console.WriteLine(JsonConvert.SerializeObject(car, Formatting.Indented));

            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                DateFormatString = "yyyy-MM-dd"
            };




            Console.WriteLine(JsonConvert.SerializeObject(car, settings));



            //var json = File.ReadAllText("myCar.json");
            //var a = new
            //{
            //    Model = "",
            //    Vendor = "",
            //    Price = 0.0M,
            //};
            //Console.WriteLine(JsonConvert.DeserializeAnonymousType(json, a, settings));










            //File.WriteAllText("myCar.json", JsonSerializer.Serialize(car));

            //Console.WriteLine(JsonSerializer.Serialize(car));

            //var json = File.ReadAllText("myCar.json");
            //Car car = JsonSerializer.Deserialize<Car>(json);

            //var json = File.ReadAllText("myCar.json");

            // -------------------- Serialization ---------------------------------------------------------
            //Console.WriteLine(JsonConvert.SerializeObject(new {Name = "Niki", Course = "EF Core" }));

            // -------------------- Deserialization ---------------------------------------------------------
            //var car = JsonConvert.DeserializeObject<Car>(json);

        }
    }
}
