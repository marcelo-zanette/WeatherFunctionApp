using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace WeatherFunctionApp
{
    public static class Function1
    {
        [FunctionName("WeatherFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ExecutionContext context,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var environment = config["Environment"];

            int start;
            int end;
            try
            {
                start = int.Parse(config["scale_start"]);
                end = int.Parse(config["scale_end"]);
            }
            catch
            {
                start = 1000;
                end = 2000;
            }

            string city = req.Query["city"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            city = city ?? data?.name;

            decimal deg = ((decimal)new Random().Next(start, end) / 10);
            decimal temp = Math.Round(deg, 1);

            string responseMessage = 
                environment + 
                "-" + 
                (string.IsNullOrEmpty(city)
                    ? "City name not defined (city=[city-name])"
                    : $"The weather in {city} is around {temp} C."
                );

            log.LogInformation(responseMessage);

            return new OkObjectResult(responseMessage);
        }
    }
}
