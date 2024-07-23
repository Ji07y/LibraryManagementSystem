using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace NotificationService
{
    public class EmailService
    {
        private readonly string _hostname;
        private readonly string _queueName;
        private readonly SmtpClient _smtpClient;

        public EmailService(string hostname, string queueName = "emailQueue")
        {
            _hostname = hostname;
            _queueName = queueName;

            _smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("carlosochoa06.01.12@gmail.com", "pggf drvp ahnk sgzs"),
                EnableSsl = true,
            };
        }

        public void Start()
        {
            try
            {
                var factory = new ConnectionFactory() { HostName = _hostname };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: _queueName,
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        try
                        {
                            var body = ea.Body.ToArray();
                            var message = Encoding.UTF8.GetString(body);
                            var lines = message.Split('\n');
                            if (lines.Length >= 4)
                            {
                                var to = lines[0].Substring(4); // "To: ...".Length = 4
                                var subject = lines[1].Substring(9); // "Subject: ...".Length = 9
                                var bodyMessage = string.Join('\n', lines.Skip(3));

                                using (var mailMessage = new MailMessage("carlosochoa06.01.12@gmail.com", to, subject, bodyMessage))
                                {
                                    _smtpClient.Send(mailMessage);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Message format is incorrect.");
                            }
                        }
                        catch (Exception ex)
                        {
                            // Log error processing the message
                            Console.WriteLine($"Error processing message: {ex.Message}");
                        }
                    };

                    channel.BasicConsume(queue: _queueName,
                                         autoAck: true,
                                         consumer: consumer);

                    // Keep the application running
                    Console.WriteLine("Waiting for messages. Press [enter] to exit.");
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                // Log connection error
                Console.WriteLine($"Error connecting to RabbitMQ: {ex.Message}");
            }
        }
    }
}
