using BikesApplication.Model;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace BikesApplication
{
    class Program
    {
        // Declaring file path for input json file and output json file
        private const string filePath = @"./../../App_Data/{0}";

        static void Main(string[] args)
        {
            try
            {
                using (StreamReader readfile = File.OpenText(String.Format(filePath, "bikes.json")))
                {
                    // Reading from the JSON file
                    JsonSerializer serializer = new JsonSerializer();

                    // Deserializing the data from JSON file
                    Garage[] garage = (Garage[])serializer.Deserialize(readfile, typeof(Garage[]));

                    // Applying LINQ query to filter bike names and their families counts (number of responses)
                    var result = garage.SelectMany(g => g.bikes)
                        .GroupBy(s => s) // Grouping by bike names
                        .Select(b => new { bikeName = b.Key, numberOfFamilies = b.Count() }) // Mapping bike name and counts relevant to the bike response
                        .OrderByDescending(r => r.numberOfFamilies) // Arranging in descending order i.e. The top responses first
                        .Take(20); // Take only top 20 results

                    // Writing Output to a file "output.json"
                    File.WriteAllText(String.Format(filePath, "output.json"), JsonConvert.SerializeObject(result));

                    // Showing output on the console as Top Bike Names used by number of families
                    foreach (var item in result)
                    {
                        Console.WriteLine("Bike Name: {0}, Number of families: {1}", item.bikeName, item.numberOfFamilies);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey(); // Holding console screen till a key is pressed
        }
    }
}
