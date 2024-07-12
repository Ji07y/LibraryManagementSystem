using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

[Route("api/[controller]")]
[ApiController]
public class NotificationsController : ControllerBase
{
	private readonly IConnection _connection;
	private readonly IModel _channel;

	public NotificationsController()
	{
		var factory = new ConnectionFactory() { HostName = "localhost" };
		_connection = factory.CreateConnection();
		_channel = _connection.CreateModel();

		_channel.QueueDeclare(queue: "notifications",
							 durable: false,
							 exclusive: false,
							 autoDelete: false,
							 arguments: null);

		var consumer = new EventingBasicConsumer(_channel);
		consumer.Received += (model, ea) =>
		{
			var body = ea.Body.ToArray();
			var message = Encoding.UTF8.GetString(body);
			// Procesar el mensaje recibido (enviar correo, etc.)
		};
		_channel.BasicConsume(queue: "notifications",
							 autoAck: true,
							 consumer: consumer);
	}

	// Método para enviar notificaciones (simulación)
	[HttpPost]
	public IActionResult SendNotification(string message)
	{
		var body = Encoding.UTF8.GetBytes(message);
		_channel.BasicPublish(exchange: "",
							 routingKey: "notifications",
							 basicProperties: null,
							 body: body);

		return Ok("Notification sent");
	}
}
