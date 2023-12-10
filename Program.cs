using System;
using System.Text.RegularExpressions;
using Dapper;
using HelloWorld.Models;
using HelloWorld.Data;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using AutoMapper;
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
                    computer.Motherboard = Utils.EscapeUnwantedChars(computer.Motherboard);
                    computer.VideoCard = Utils.EscapeUnwantedChars(computer.VideoCard);
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
            // DefaultContractResolver contractResolver = new()
            // {
            //     NamingStrategy = new CamelCaseNamingStrategy()
            // };

            // string computersCopyNewtonsoft = JsonConvert.SerializeObject(computers, new JsonSerializerSettings()
            // {
            //     ContractResolver = contractResolver
            // });
            // File.WriteAllText("computersCopyNewtonsoft.txt", computersCopyNewtonsoft);

            // string computersCopySystemJson = System.Text.Json.JsonSerializer.Serialize(computers, options);
            // File.WriteAllText("computersCopySystemJson.txt", computersCopySystemJson);


            // Automapper Model mapping from snake case to JSON
            string computersSnakeJson = File.ReadAllText("ComputersSnake.json");
            Mapper mapper = new(new MapperConfiguration((cfg) =>
            {
                // mapping from ComputerSnake.cs model mapping to Computer.cs model
                cfg.CreateMap<ComputerSnake, Computer>()
                    .ForMember(destination => destination.ComputerId,
                    options => options.MapFrom(source => source.computer_id))
                    .ForMember(destination => destination.Motherboard,
                    options => options.MapFrom(source => source.motherboard))
                    .ForMember(destination => destination.HasWifi,
                    options => options.MapFrom(source => source.has_wifi))
                    .ForMember(destination => destination.HasLTE,
                    options => options.MapFrom(source => source.has_lte))
                    .ForMember(destination => destination.VideoCard,
                    options => options.MapFrom(source => source.video_card))
                    .ForMember(destination => destination.ReleaseDate,
                    options => options.MapFrom(source => source.release_date))
                    .ForMember(destination => destination.Price,
                    options => options.MapFrom(source => source.price));
            }));
            IEnumerable<ComputerSnake>? computerSnakes = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<ComputerSnake>>(computersSnakeJson);
            if (computerSnakes != null)
            {
                // execute the mapping with the above MappingConfigurations (from source to destination)
                IEnumerable<Computer> computersResult = mapper.Map<IEnumerable<Computer>>(computerSnakes);
                foreach (Computer computer in computersResult)
                {
                    Console.WriteLine(computer.Motherboard);
                }
            }

        }
    }
}
