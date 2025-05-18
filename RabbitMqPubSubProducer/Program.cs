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

string exchangeName = "exchange_test_01";
await channel.ExchangeDeclareAsync(exchange: "exchange_test_01", type: ExchangeType.Direct);
string routingKey = "RK01";


string message = "Hello World! Producer";
var body = Encoding.UTF8.GetBytes(message);


await channel.BasicPublishAsync(exchange: exchangeName, routingKey: routingKey, body: body);

Console.WriteLine("Message Has Sent !");
Console.ReadLine();