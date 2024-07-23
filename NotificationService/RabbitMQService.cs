using RabbitMQ.Client;
using System;
using System.Text;

namespace NotificationService
{
    public class RabbitMQService
    {
        private readonly string _hostname;
        private readonly string _queueName;

        public RabbitMQService(string hostname, string queueName = "emailQueue")
        {
            _hostname = hostname;
            _queueName = queueName;
        }

        public void SendEmail(string to, string subject, string body)
        {
            try
            {
                var factory = new ConnectionFactory() { HostName = _hostname };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: _queueName,
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    string message = $"To: {to}\nSubject: {subject}\n\n{body}";
                    var bodyBytes = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                                         routingKey: _queueName,
                                         basicProperties: null,
                                         body: bodyBytes);
                }
            }
            catch (Exception ex)
            {
                // Log error sending the email
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
    }
}
