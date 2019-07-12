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

            using (var bp = new BasicTopicProducer(configuration["RabbitMQUrl"], "MyExchange"))
            {
                for (int i = 0; i < 5; i++)
                {
                    bp.PublishMessage("Hello World!", "badRoutingKey");
                    bp.PublishMessage("Hello World!", "goodRoutingKey");
                }
            }
        }
    }
}