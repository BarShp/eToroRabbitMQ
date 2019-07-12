using System;
using System.IO;
using BasicRabbitMQ;
using Common;
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

            using (var producer = new BasicTopicProducer(configuration["RabbitMQUrl"], configuration["ExchangeName"]))
            {
                for (int i = 0; i < 5; i++)
                {
                    StockInfo stock = new StockInfo()
                    {
                        BuyRate = 4.1,
                        SellRate = 3.2,
                        Time = DateTime.Now
                    };

                    stock.ID = "GOOG";
                    producer.Publish(stock, stock.ID);
                    stock.ID = "AAPL";
                    producer.Publish(stock, stock.ID);
                }
            }
        }
    }
}