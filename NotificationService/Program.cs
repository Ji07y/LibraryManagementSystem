using System;

namespace NotificationService
{
    class Program
    {
        static void Main(string[] args)
        {
            // Puedes obtener estos valores de la configuración o de variables de entorno
            string hostname = "rabbitmq";
            string queueName = "emailQueue";

            var emailService = new EmailService(hostname, queueName);
            emailService.Start();

            Console.WriteLine("Servicio de notificación iniciado. Presiona [enter] para salir.");
            Console.ReadLine();
        }
    }
}
