using System.Text;
using RabbitMQ.Client;

var connectionFactory = new ConnectionFactory()
{
    HostName = "localhost",
    UserName = "newadmin",
    Password = "s0m3p4ssw0rd"
};
using var connection = await connectionFactory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(queue: "MyQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);



string message = "Hello World! Producer";
var body = Encoding.UTF8.GetBytes(message);
await channel.BasicPublishAsync(exchange: "", routingKey: "MyQueue", body: body);
Console.WriteLine("Message Has Sent !");
Console.ReadLine();