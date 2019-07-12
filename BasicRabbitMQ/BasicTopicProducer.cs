using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace BasicRabbitMQ
{
    public class BasicTopicProducer : IDisposable
    {
        readonly string exchangeName;
        IConnection connection;
        IModel channel;

        public BasicTopicProducer(string rabbitMQUrl, string exchangeName)
        {
            this.exchangeName = exchangeName;
            var factory = new ConnectionFactory()
            {
                Uri = new Uri(rabbitMQUrl)
            };
            this.connection = factory.CreateConnection();
            this.channel = connection.CreateModel();
            channel.ExchangeDeclare(exchangeName, "topic", autoDelete: false);
        }

        public void Publish(Object body, string routingKey)
        {
            var jsonBody = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(body));

            channel.BasicPublish(exchange: exchangeName,
                                 routingKey: routingKey,
                                 body: jsonBody);
        }

        public void Dispose()
        {
            connection.Dispose();
            channel.Dispose();
        }
    }
}
