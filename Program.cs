using System;
using System.Linq;
using DataAnalyse;
using System.Threading;
using Confluent.Kafka;
using DataAnalyse.Infrastructure;

namespace DataAnalyse
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new telecomContext())
            {
                Console.WriteLine("Inserting a new blog");
                db.Locations.Take(100000).ToList().ForEach(x => Console.WriteLine(x.City));
            }
            KafkaConsumers.start();
        }
    }
}