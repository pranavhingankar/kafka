using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kafka_3.Services
{
    public class KafkaConsumer
    {
        private readonly ConsumerConfig _config;

        public KafkaConsumer(string bootstrapServers, string groupId)
        {
            _config = new ConsumerConfig
            {
                BootstrapServers = bootstrapServers,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
        }

        public void Consume(string topic)
        {
            using (var consumer = new ConsumerBuilder<string, string>(_config).Build())
            {
                consumer.Subscribe(topic);

                while (true)
                {
                    try
                    {
                        var message = consumer.Consume();
                        Console.WriteLine($"Consumed message: {message.Value} from topic {message.Topic}");
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine($"Error occurred: {e.Error.Reason}");
                    }
                }
            }
        }
    }
}
