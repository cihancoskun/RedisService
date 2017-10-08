using System.Configuration;

namespace RedisService
{
    class Program
    {
        static void Main(string[] args)
        {
            Consumer consumer = new Consumer();

            consumer.Consume(ConfigurationSettings.AppSettings["QueueName"]);
        }
    }
}
