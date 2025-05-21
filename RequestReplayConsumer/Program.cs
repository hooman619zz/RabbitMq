using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

Console.WriteLine("Consumer :");
Console.WriteLine("-------------------------------------");

var connectionFactory = new ConnectionFactory()
{
    HostName = "localhost",
    UserName = "newadmin",
    Password = "s0m3p4ssw0rd"
};
using var connection = await connectionFactory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

var replyQueue = await channel.QueueDeclareAsync(queue: "", durable: true, exclusive: true);
await channel.QueueDeclareAsync("request-queue", exclusive: false);


var consumerEvent = new AsyncEventingBasicConsumer(channel);

consumerEvent.ReceivedAsync += async (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Reply message : {message}");
};

await channel.BasicConsumeAsync(replyQueue.QueueName, true, consumerEvent);

var properties = new BasicProperties();
properties.ReplyTo = replyQueue.QueueName;
properties.CorrelationId = Guid.NewGuid().ToString();

string message = "send request consumer ";
var body = Encoding.UTF8.GetBytes(message);

await channel.BasicPublishAsync(addr: new PublicationAddress(null, null, "request-queue"), basicProperties: properties, body);

Console.WriteLine("Consumer Finished");
Console.ReadLine();