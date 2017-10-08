using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using RedisService.Services;

namespace RedisService
{
    public class Consumer
    {
        private static RabbitMq _rabbitMQService;

        public Consumer()
        {
            if (_rabbitMQService == null)
            {
                _rabbitMQService = new RabbitMq();
            }
        }

        public void Consume(string queueName)
        {
            _rabbitMQService = new RabbitMq();

            using (var connection = _rabbitMQService.GetRabbitMQConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var jsonString = Encoding.UTF8.GetString(body);

                        SetStringFromJson(jsonString);
                    };

                    channel.BasicConsume(queueName, true, consumer);

                    ExitProgram();
                }
            }
        }

        private void SetStringFromJson(string jsonString)
        {
            Dictionary<string, object> userCreatedCommand = JsonConvert.DeserializeObject<Dictionary<string, object>>(JToken.Parse(jsonString).ToString());

            var redisConnection = new RedisConnection().GetRedisConnection;

            var db = redisConnection.GetDatabase();

            if (userCreatedCommand.ContainsKey("keys") && userCreatedCommand.ContainsKey("value"))
            {
                IEnumerable enumerable = userCreatedCommand["keys"] as IEnumerable;
                foreach (var keyObj in enumerable)
                {
                    db.StringSet(keyObj.ToString(), JsonConvert.SerializeObject(userCreatedCommand["value"]));
                }
            }
        }

        private void ExitProgram()
        {
            Console.WriteLine("Programı sonlandırmak için exit yaz");
            string input = "";
            do
            {
                input = Console.ReadLine();
            } while (input != "exit");
        }
    }
}
