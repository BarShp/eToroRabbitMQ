using System;
using System.IO;
using BasicRabbitMQ;
using Microsoft.Extensions.Configuration;

namespace StocksPublisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");

            IConfigurationRoot configuration = builder.Build();

            using (var bp = new BasicTopicProducer(configuration["RabbitMQUrl"], configuration["ExchangeName"]))
            {
                for (int i = 0; i < 5; i++)
                {
                    bp.PublishMessage("Hello GOOG!", "GOOG");
                    bp.PublishMessage("Hello AAPL!", "AAPL");
                }
            }
        }
    }
}