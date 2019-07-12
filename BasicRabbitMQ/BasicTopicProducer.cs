using System;
using System.Text;
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

        public void PublishMessage(string message, string routingKey)
        {
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: exchangeName,
                                 routingKey: routingKey,
                                 body: body);
        }

        public void Dispose()
        {
            connection.Dispose();
            channel.Dispose();
        }
    }
}
