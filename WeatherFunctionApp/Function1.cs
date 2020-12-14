using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WeatherFunctionApp
{
    public static class Function1
    {
        [FunctionName("WeatherFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string city = req.Query["city"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            city = city ?? data?.name;

            decimal deg = new Random().Next(100, 200) / 10;
            decimal temp = Math.Round(deg, 1);

            string responseMessage = string.IsNullOrEmpty(city)
                ? "City name not defined (city=[city-name])"
                : $"The weather in {city} is around {temp} C.";

            return new OkObjectResult(responseMessage);
        }
    }
}
