import pika

connection = pika.BlockingConnection(pika.ConnectionParameters('rabbitmq'))
channel = connection.channel()
channel.queue_declare(queue='emailQueue', durable=False)  # Asegúrate de que 'durable' sea True
channel.basic_publish(exchange='', routing_key='emailQueue', body='To: test@example.com\nSubject: Test Message\n\nThis is a test message.')
connection.close()
