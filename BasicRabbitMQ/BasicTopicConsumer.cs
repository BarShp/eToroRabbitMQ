using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BasicRabbitMQ
{
    public class BasicTopicConsumer : IDisposable
    {
        readonly string queueName;
        IConnection connection;
        IModel channel;

        public BasicTopicConsumer(string rabbitMQUrl, string queueName, string exchangeName, string bindingKey)
        {
            this.queueName = queueName;
            var factory = new ConnectionFactory()
            {
                Uri = new Uri(rabbitMQUrl)
            };
            this.connection = factory.CreateConnection();
            this.channel = connection.CreateModel();
            channel.ExchangeDeclare(exchangeName, "topic", autoDelete: false);
            channel.QueueDeclare(queueName, exclusive: false, autoDelete: false);
            channel.QueueBind(queueName, exchangeName, bindingKey);
        }

        public void Consume<T>(EventHandler<T> onReceived)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var jsonBody = Encoding.UTF8.GetString(body);
                T deserializedObject = JsonConvert.DeserializeObject<T>(jsonBody);
                onReceived(this, deserializedObject);
            };
            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }

        public void Dispose()
        {
            connection.Dispose();
            channel.Dispose();
        }
    }
}
