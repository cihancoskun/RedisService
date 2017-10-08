using System.Configuration;

using RabbitMQ.Client;

namespace RedisService.Services
{
    public class RabbitMq
    {  
        public IConnection GetRabbitMQConnection()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory()
            {
                HostName = ConfigurationSettings.AppSettings["rabbitHost"]
                //UserName = ConfigurationSettings.AppSettings["rabbitUserName"],
                //Password = ConfigurationSettings.AppSettings["rabbitPassword"]
            };

            return connectionFactory.CreateConnection();
        }
    }
}
