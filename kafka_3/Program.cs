using System;
using System.Linq;
using kafka_3.DataAccess;
using kafka_3.Services;
using Microsoft.EntityFrameworkCore;

namespace kafka_3
{
    class Program
    {
        static void Main(string[] args)
        {
            // Configure database connection
            var dbContextOptions = new DbContextOptionsBuilder<CRUDDbContext>()
                .UseSqlServer("your_connection_string")
                .Options;

            // Start Kafka producer
            var kafkaProducer = new KafkaProducer("localhost:9092");

            // Start Kafka consumer
            var kafkaConsumer = new KafkaConsumer("localhost:9092", "my-topic");

            // Initialize last primary key value
            int lastPrimaryKey = 0;

            // Continuously fetch and publish new user data to Kafka
            while (true)
            {
                // Fetch new user data from MSSQL database
                using (var dbContext = new CRUDDbContext(dbContextOptions))
                {
                    var newUsers = dbContext.Users.Where(u => u.id > lastPrimaryKey).ToList();

                    if (newUsers.Any())
                    {
                        lastPrimaryKey = newUsers.Max(u => u.id);

                        foreach (var user in newUsers)
                        {
                            kafkaProducer.ProduceAsync("my-topic", $"{{\"Id\":{user.id},\"Name\":\"{user.name}\",\"Salary\":{user.salary}}}");
                        }
                    }
                }

                // Receive messages from Kafka
                kafkaConsumer.Consume("my-topic");

                
                // Wait for some time before fetching new data again (e.g., every 5 seconds)
                System.Threading.Thread.Sleep(5000);
            }
        }
    }
}
