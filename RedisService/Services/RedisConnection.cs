using System;
using System.Configuration;

using StackExchange.Redis;

namespace RedisService.Services
{
    public class RedisConnection
    {
        private static ConnectionMultiplexer _redis;
        private static readonly Object _multiplexerLock = new Object();

        private void ConnectRedis()
        {
            try
            {
                var configOptions = new ConfigurationOptions();
                configOptions.EndPoints.Add(ConfigurationSettings.AppSettings["redisHost"]);
                configOptions.ClientName = ConfigurationSettings.AppSettings["redisClientName"];
                configOptions.ConnectTimeout = 100000;
                configOptions.SyncTimeout = 100000;

                _redis = ConnectionMultiplexer.Connect(configOptions);
            }
            catch (Exception ex)
            {
                //exception handling
            }
        }

        public ConnectionMultiplexer GetRedisConnection
        {
            get
            {
                lock (_multiplexerLock)
                {
                    if (_redis == null || !_redis.IsConnected)
                    {
                        ConnectRedis();
                    }
                    return _redis;
                }
            }
        }
    }
}
