using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using BasicRabbitMQ;
using Common;

namespace GOOGConsumer
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
                consumer.Consume<StockInfo>((c, stock) =>
                {
                    Console.WriteLine($"StockId: {stock.ID}, BuyRate: {stock.BuyRate}, SellRate: {stock.SellRate}, Time: {stock.Time}");
                });

                Console.WriteLine("Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}