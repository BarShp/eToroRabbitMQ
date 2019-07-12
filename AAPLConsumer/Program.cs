using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using BasicRabbitMQ;

namespace AAPLConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfigurationRoot configuration = builder.Build();

            using (var consumer =
                new BasicTopicConsumer(
                    configuration["RabbitMQUrl"],
                    configuration["QueueName"],
                    configuration["ExchangeName"],
                    configuration["BindingKey"]))
            {
                consumer.Consume((c, msg) =>
                {
                    Console.WriteLine(msg);
                });

                Console.WriteLine("Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
