using AutoMapper;
using CarDealer.Data;
using CarDealer.DataTransferObjects.Input;
using CarDealer.DataTransferObjects.Output;
using CarDealer.Models;
using CarDealer.XMLHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        static IMapper mapper;

        public static void Main(string[] args)
        {
            var context = new CarDealerContext();

            //-------------------- IMPORT --------------------------------------------------
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            //var supplierXml = File.ReadAllText("./Datasets/suppliers.xml");
            //var partsXml = File.ReadAllText("./Datasets/parts.xml");
            //var carsXml = File.ReadAllText("./Datasets/cars.xml");
            //var customersXml = File.ReadAllText("./Datasets/customers.xml");
            //var salesXml = File.ReadAllText("./Datasets/sales.xml");

            //ImportSuppliers(context, supplierXml);
            //ImportParts(context, partsXml);
            //ImportCars(context, carsXml);
            //ImportCustomers(context, customersXml);
            //ImportSales(context, salesXml);
            //Console.WriteLine(result);


            //--------------------  EXPORT  --------------------------------------------------

            //GetCarsWithDistance(context);
            //GetCarsFromMakeBmw(context);
            //var result = GetLocalSuppliers(context);
            Console.WriteLine(GetCarsWithTheirListOfParts(context));
        }
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .Select(x => new CarPartOutputModel
                {
                    Make = x.Make,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance,
                    Part = x.PartCars.Select(pc => new CarPartInfoOutputModel
                    {
                        Name = pc.Part.Name,
                        Price = pc.Part.Price
                    })
                    .OrderByDescending(x => x.Price)
                    .ToArray()

                })
                .OrderByDescending(t => t.TravelledDistance)
                .ThenBy(x => x.Model)
                .Take(5)
                .ToArray();

            var result = XmlConverter.Serialize(cars, "cars");
            return result;
        }


        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(x => x.IsImporter != true)
                .Select(x => new SupplierOutputModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    PartCount = x.Parts.Count
                })
                .ToList();

            var result = XmlConverter.Serialize(suppliers, "suppliers");
            return result;

        }


        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var carsBmw = context.Cars
                .Where(x => x.Make == "BMW")
                .Select(x => new BmwOutputModel
                {
                    Id = x.Id,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance
                })
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TravelledDistance)
                .ToArray();

            var result = XmlConverter.Serialize(carsBmw, "cars");
            return result;
        }

        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(c => c.TravelledDistance > 2_000_000)
                .Select(c => new CarOutputModel
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .ToArray();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CarOutputModel[]), new XmlRootAttribute("cars"));

            var textWriter = new StringWriter();

            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            xmlSerializer.Serialize(textWriter, cars, ns);
            var result = textWriter.ToString();

            //var result = XmlConverter.Serialize(cars, "cars");

            return result;
        }


        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            const string root = "Sales";

            var salesDto = XmlConverter.Deserializer<SaleInputModel>(inputXml, root);
            var carsId = context.Cars
                .Select(x => x.Id)
                .ToList();

            var sales = salesDto
                .Where(x => carsId.Contains(x.CarId))
                .Select(x => new Sale
                {
                    CarId = x.CarId,
                    CustomerId = x.CustomerId,
                    Discount = x.Discount
                })
                .ToList();

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}";
        }


        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            const string root = "Customers";
            InitializeAutoMapper();

            var customersDto = XmlConverter.Deserializer<CustomerInputModel>(inputXml, root);
            var customers = mapper.Map<Customer[]>(customersDto);

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Length}";
        }


        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            const string root = "Cars";

            var carsDtos = XmlConverter.Deserializer<CarInputModel>(inputXml, root);

            var allParts = context.Parts
                .Select(x => x.Id)
                .ToList();

            var cars = carsDtos
                .Select(x => new Car
                {
                    Make = x.Make,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance,
                    PartCars = x.CarPartsInputModel
                        .Select(x => x.Id)
                        .Distinct()
                        .Intersect(allParts)
                        .Select(pc => new PartCar
                        {
                            PartId = pc
                        })
                    .ToList()
                })
                .ToList();

            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }


        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            const string root = "Parts";

            var partsDtos = XmlConverter.Deserializer<PartInputModel>(inputXml, root);

            var supplierIds = context.Suppliers
                .Select(x => x.Id)
                .ToList();

            var parts = partsDtos
                .Where(x => supplierIds.Contains(x.SupplierId))
                .Select(x => new Part
                    {
                        Name = x.Name,
                        Price = x.Price,
                        Quantity = x.Quantity,
                        SupplierId = x.SupplierId
                    })
                .ToList();

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count}";
        }


        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            const string root = "Suppliers";
            var suppliersDto = XmlConverter.Deserializer<SupplierInputModel>(inputXml, root);

            var suppliers = suppliersDto.Select(x => new Supplier
            {
                Name = x.Name,
                IsImporter = x.IsImporter
            })
                .ToList();

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();
            
            return $"Successfully imported {suppliers.Count}";
        }

        private static void InitializeAutoMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });

            mapper = config.CreateMapper();
        }
    }
}