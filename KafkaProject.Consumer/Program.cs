// See https://aka.ms/new-console-template for more information

using Confluent.Kafka;
using Newtonsoft.Json;

var config = new ConsumerConfig
{
    GroupId = "kafka-consumer-group",
    BootstrapServers = "localhost:9092",
    AutoOffsetReset = AutoOffsetReset.Earliest
};

using var consumer = new ConsumerBuilder<Null, string>(config).Build();

consumer.Subscribe("kafka-topic");

CancellationTokenSource token = new();

try
{

    while (true)
    {
        var response = consumer.Consume(token.Token);
        if (response.Message != null)
        {
            var weather = JsonConvert.DeserializeObject<Weather>(response.Message.Value);
            Console.WriteLine($"State: {weather.State}" + $"Temp: {weather.Temperature}F");
        }
    }
}
catch(ConsumeException exception)
{
    throw;
}


public record Weather(string State, int Temperature);