using System;
using System.Text.RegularExpressions;
using Dapper;
using HelloWorld.Models;
using HelloWorld.Data;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
namespace HelloWorld
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
            DataContextDapper dapperContext = new DataContextDapper(config);
            DataContextEF dataContextEF = new(config);
            // string SQLQuery = "select GETDATE()";
            // DateTime currentTime = dapperContext.LoadDataSingle<DateTime>(SQLQuery); // run query using Dapper
            // Console.WriteLine(currentTime);
            // Computer computer = new()
            // {

            //     Motherboard = "AS160",
            //     HasWifi = true,
            //     HasLTE = false,
            //     CPUCores = 4,
            //     ReleaseDate = DateTime.Now,
            //     Price = 1982.23m,
            //     VideoCard = "GTX 1660"
            // };
            // string sql = @"INSERT INTO TutorialAppSchema.Computer (
            //     Motherboard,
            //     HasWifi,
            //     HasLTE,
            //     CPUCores,
            //     VideoCard,
            //     Price
            // ) VALUES (
            //     '" + computer.Motherboard
            //     + "','" + computer.HasWifi
            //     + "','" + computer.HasLTE
            //     + "','" + computer.CPUCores
            //     + "','" + computer.VideoCard
            //     + "','" + computer.Price
            // + "')"; // @ allows to write multiple lines of sql

            // string sqlSelect = @"SELECT * FROM TutorialAppSchema.Computer";
            // IEnumerable<Computer> ans = dapperContext.LoadData<Computer>(sqlSelect);
            // foreach (var s in ans)
            // {
            //     Console.WriteLine($"{s.ComputerId}, {s.Motherboard}, {s.CPUCores}");
            // }
            // int result = dapperContext.ExecuteWithRowCount(sql);
            // bool result = dapperContext.Execute(sql);
            // Console.WriteLine(result);

            // EntityFramework code to Add record to Table
            // dataContextEF.Add(computer);
            // dataContextEF.SaveChanges();
            // IEnumerable<Computer>? resultsEF = dataContextEF.Computer?.ToList();
            // if (resultsEF != null)
            // {
            //     foreach (var res in resultsEF)
            //     {
            //         string formatted = $"ComputerId: {res.ComputerId}, Motherboard: {res.Motherboard}, CPUCores: {res.CPUCores}, ReleaseDate: {res.ReleaseDate}, Price: {res.Price}, HasWifi: {res.HasWifi}, HasLTE: {res.HasLTE}, VideoCard: {res.VideoCard}";
            // Console.WriteLine(formatted);
            // File.AppendAllText("log.txt", "\n" + formatted); 
            // alternative way:
            // using StreamWriter openFileWrite = new("log.txt", append: true);
            // openFileWrite.WriteLine(formatted);

            // overwrites the file (no append)
            // File.WriteAllText("log.txt", "");
            // }
            // }
            // using StreamReader openFileRead = new("log.txt");
            // string readStr = openFileRead.ReadToEnd();
            // Console.WriteLine("Contains AS160? " + readStr.Contains("\r\n"));

            string computersJson = File.ReadAllText("Computers.json");
            // Console.WriteLine(computersJson);

            // tell System.Text.JSON to look for CamelCase object keys
            JsonSerializerOptions options = new()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            // deserializing JSON so we can enumerate over them and save to DB as Computer Object and use Computer (Model)
            // IEnumerable<Computer>? computers = JsonSerializer.Deserialize<IEnumerable<Computer>>(computersJson, options);

            // or use Newtonsoft.Json nuget package (better, doesnt require options to check Camel Case prop name)
            IEnumerable<Computer>? computers = JsonConvert.DeserializeObject<IEnumerable<Computer>>(computersJson);

            // Console.WriteLine(computers?.Count());
            if (computers != null)
            {
                foreach (Computer computer in computers)
                {
                    // Console.WriteLine(computer.VideoCard);
                    string sql = @"INSERT INTO TutorialAppSchema.Computer (
                        Motherboard,
                        HasWifi,
                        HasLTE,
                        ReleaseDate,
                        Price,
                        VideoCard
                    ) VALUES (
                        '" + computer.Motherboard
                        + "','" + computer.HasWifi
                        + "','" + computer.HasLTE
                        + "','" + computer.ReleaseDate
                        + "','" + computer.Price
                        + "','" + computer.VideoCard
                    + "')"; // @ allows to write multiple lines of sql

                    dapperContext.Execute(sql);
                }
            }

            // switch to camelCase
            DefaultContractResolver contractResolver = new()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            string computersCopyNewtonsoft = JsonConvert.SerializeObject(computers, new JsonSerializerSettings()
            {
                ContractResolver = contractResolver
            });
            File.WriteAllText("computersCopyNewtonsoft.txt", computersCopyNewtonsoft);

            string computersCopySystemJson = System.Text.Json.JsonSerializer.Serialize(computers, options);
            File.WriteAllText("computersCopySystemJson.txt", computersCopySystemJson);
        }
    }
}
