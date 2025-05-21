using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var connectionFactory = new ConnectionFactory()
{
    HostName = "localhost",
    UserName = "newadmin",
    Password = "s0m3p4ssw0rd"
};
using var connection = await connectionFactory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync("request-queue", exclusive: false);


var consumerEvent = new AsyncEventingBasicConsumer(channel);

consumerEvent.ReceivedAsync += async (model, ea) =>
{
    Console.WriteLine($"CorrelationId : {ea.BasicProperties.CorrelationId}");
    Console.WriteLine($"ReplyTo : {ea.BasicProperties.ReplyTo}");


    var message2 = $"this message is response for : {ea.BasicProperties.CorrelationId}";
    var body2 = Encoding.UTF8.GetBytes(message2);
    await channel.BasicPublishAsync("", routingKey: ea.BasicProperties.ReplyTo, body2);
};
await channel.BasicConsumeAsync("request-queue", true, consumerEvent);
Console.ReadLine();