using System;
using System.Linq;
using DataAnalyse;
using System.Threading;
using Confluent.Kafka;

namespace DataAnalyse
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new telecomContext())
            {
                Console.WriteLine("Inserting a new blog");
                var locations = db.Locations.Select(x => x).Take(5).ToList();
                foreach (var location in locations)
                {
                    System.Console.WriteLine(location.City);
                }
                var conf = new ConsumerConfig
                {
                    GroupId = "'consumer'",
                    EnableAutoCommit = false,
                    BootstrapServers = "10.132.221.111:6667,10.132.221.112:6667," +
    "10.132.221.113:6667,10.132.221.114:6667,10.132.221.116:6667," +
    "10.132.221.117:6667,10.132.221.118:6667,10.132.221.119:6667," +
    "10.132.221.120:6667,10.132.221.121:6667,10.132.221.123:6667," +
    "10.132.221.124:6667,10.132.221.125:6667,10.132.221.126:6667," +
    "10.132.221.127:6667,10.132.221.128:6667,10.132.221.129:6667," +
    "10.132.221.130:6667,10.132.221.132:6667",
            // Note: The AutoOffsetReset property determines the start offset in the event
            // there are not yet any committed offsets for the consumer group for the
            // topic/partitions of interest. By default, offsets are committed
            // automatically, so in this example, consumption will only start from the
            // earliest message in the topic "my-topic" the first time you run the program.
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                    AllowAutoCreateTopics=true
                };

                using (var c = new ConsumerBuilder<Ignore, string>(conf).Build())
                {
                    c.Subscribe("'consumer'");
                
                    CancellationTokenSource cts = new CancellationTokenSource();
                    Console.CancelKeyPress += (_, e) =>
                    {
                        e.Cancel = true; // prevent the process from terminating.
                        cts.Cancel();
                    };

                    try
                    {
                        while (true)
                        {
                            try
                            {
                                var cr = c.Consume(cts.Token);
                                Console.WriteLine($"Consumed message '{cr.Value}' at: '{cr.TopicPartitionOffset}'.");
                            }
                            catch (ConsumeException e)
                            {
                                Console.WriteLine($"Error occured: {e.Error.Reason}");
                            }
                        }
                    }
                    catch (OperationCanceledException e)
                    {
                        // Ensure the consumer leaves the group cleanly and final offsets are committed.
                        Console.WriteLine($"Error occured: {e.ToString()}");
                        c.Close();
                    }
                }
            }
        }
    }
}
