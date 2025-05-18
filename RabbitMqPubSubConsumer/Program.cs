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


string exchangeName = "exchange_test_01";
await channel.ExchangeDeclareAsync(exchange: "exchange_test_01", type: ExchangeType.Direct);
string routingKey = "RK01";
string queueName = (await channel.QueueDeclareAsync()).QueueName;
await channel.QueueBindAsync(queue: queueName, exchange: exchangeName, routingKey: routingKey);

var consumerEvent = new AsyncEventingBasicConsumer(channel);

consumerEvent.ReceivedAsync += async (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine(message);
};

await channel.BasicConsumeAsync(queueName, true, consumerEvent);
Console.WriteLine(" [x] Received message");
Console.ReadLine(); 