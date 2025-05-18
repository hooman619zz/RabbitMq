using System.Text;
using RabbitMQ.Client;

Console.WriteLine("Hello, World!");
var connectionFactory = new ConnectionFactory()
{
    HostName = "localhost",
    UserName = "newadmin",
    Password = "s0m3p4ssw0rd"
};
using var connection = await connectionFactory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

string exchangeName = "TopicEx";
await channel.ExchangeDeclareAsync(exchange: exchangeName, type: ExchangeType.Topic);

bool isRepeat = true;
while (isRepeat)
{
    Console.WriteLine("Enter Command:");
    var command = Console.ReadLine();
    switch (command)
    {
        case "p":
            Console.WriteLine("Enter Routing Key:");
            string routingKey = Console.ReadLine();
            
            Console.WriteLine("Enter Message:");
            string message = Console.ReadLine();
            
            var body = Encoding.UTF8.GetBytes(message!);

            await channel.BasicPublishAsync(exchange: exchangeName, routingKey: routingKey, body: body);
            Console.WriteLine("-------------------------------------");
            break;
        case "e":
            isRepeat = false;
            break;
    }
}


Console.WriteLine("Message Has Sent !");
Console.ReadLine();