using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RabbitMQ.Client;
using RabbitMQ.Util;
using RabbitMQ.Client.Events;
using System.Text;

namespace ChatApplication.Models.HelperBll
{
    public class RabbitMQBll
    {
        public IConnection GetConnection()
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.UserName = "guest";
            factory.Password = "guest";
            factory.Port = 5672;
            factory.HostName = "localhost";
            factory.VirtualHost = "/";
            // factory.Uri = "http://192.168.7.140:15672/";
            return factory.CreateConnection();
        }
        public bool send(IConnection con, string message, int roomId, int userId)
        {
            try
            {

                IModel channel = con.CreateModel();
                var properties = channel.CreateBasicProperties();
                Dictionary<string, object> headerProps = new Dictionary<string, object>();
                headerProps.Add("UserId", userId);
                headerProps.Add("RoomId", roomId);
                properties.Headers = headerProps;

                string queue = GetQueueName(roomId, userId);

                channel.ExchangeDeclare("ChatExchange", ExchangeType.Fanout);
                channel.QueueDeclare(queue, true, false, false, null);
                channel.QueueBind(queue, "ChatExchange", queue, null);
                var msg = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish("ChatExchange", queue, properties, msg);
                //channel.BasicPublish("ChatExchange", roomqueue, null, msg);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;

        }
        public string receive(IConnection con, int roomId, int userId)
        {
            try
            {
                string queue = GetQueueName(roomId, userId);


                IModel channel = con.CreateModel();
                channel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: null);
                var consumer = new EventingBasicConsumer(channel);
                BasicGetResult result = channel.BasicGet(queue: queue, autoAck: true);
                if (result != null)
                {
                    // check that the message is not from the current user
                    int messageUserId = (int)result.BasicProperties.Headers["UserId"];
                    int messageRoomId = (int)result.BasicProperties.Headers["RoomId"];
                    if (userId == messageUserId || messageRoomId != roomId)
                        return null;

                    // Get User name
                    DataLayer dl = new DataLayer();
                    UserModel user = dl.GetUserById(messageUserId);
                    return user.username + ":" + Encoding.UTF8.GetString(result.Body);
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;

            }


        }

        public static string GetQueueName(int roomId, int userId)
        {
            return String.Format("UserId={0}-Room={1}", userId, roomId);
        }
    }

}