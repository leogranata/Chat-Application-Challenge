using System;
using System.Collections.Generic;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using BotManager;

namespace ChatApplication.Models.HelperBll
{
    public class RabbitMQBll
    {
        private const int max_queue_length = 50;

        public ConnectionFactory GetConnectionFactory()
        {
            ConnectionFactory factory = new ConnectionFactory
            {
                UserName = "guest",
                Password = "guest",
                Port = 5672,
                HostName = "localhost",
                VirtualHost = "/"
            };
            return factory;
        }
        public bool Send(string message, int roomId, int userId)
        {
            try
            {
                string queue = GetQueueName(roomId, userId);

                using (var connection = GetConnectionFactory().CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        // Pass to Bot Manager to process if needed
                        if (message.StartsWith("/") && message.Contains("="))
                        {
                            BotManager.BotManager botMgr = new BotManager.BotManager();
                            string response = botMgr.ExecuteCommand(message);
                            if (response != null)
                            {
                                var botResponseProps = channel.CreateBasicProperties();
                                Dictionary<string, object> botHeaderProps = new Dictionary<string, object>
                                {
                                    { "UserId", -1 },
                                    { "RoomId", roomId }
                                };
                                botResponseProps.Headers = botHeaderProps;
                                var responseBytes = Encoding.UTF8.GetBytes(response);
                                channel.BasicPublish("ChatAppExchange", queue, botResponseProps, responseBytes);
                            }
                        }
                        else
                        {
                            var properties = channel.CreateBasicProperties();
                            Dictionary<string, object> headerProps = new Dictionary<string, object>
                            {
                                { "UserId", userId },
                                { "RoomId", roomId }
                            };
                            properties.Headers = headerProps;

                            channel.ExchangeDeclare("ChatAppExchange", ExchangeType.Direct);
                            Dictionary<string, object> max_length = new Dictionary<string, object>
                            {
                                { "x-max-length", max_queue_length }
                            };

                            channel.QueueDeclare(queue, true, false, false, max_length);
                            channel.QueueBind(queue, "ChatAppExchange", queue, null);
                            var msg = Encoding.UTF8.GetBytes(message);
                            channel.BasicPublish("ChatAppExchange", queue, properties, msg);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;

        }

        [Obsolete]
        public string Receive(int roomId, int userId)
        {
            try
            {
                string queue = GetQueueName(roomId, userId);
                StringBuilder response = new StringBuilder();

                using (var connection = GetConnectionFactory().CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        Dictionary<string, object> max_length = new Dictionary<string, object>
                        {
                            { "x-max-length", max_queue_length }
                        };

                        var queueDeclareResponse = channel.QueueDeclare(GetQueueName(roomId, userId), true, false, false, max_length);

                        var consumer = new QueueingBasicConsumer(channel);
                        channel.BasicConsume(GetQueueName(roomId, userId), false, consumer);

                        if (queueDeclareResponse.MessageCount == 0) return null;

                        for (int i = 0; i < queueDeclareResponse.MessageCount; i++)
                        {
                            var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                            var body = ea.Body;
                            var message = Encoding.UTF8.GetString(body);
                            var messageUserId = (int)ea.BasicProperties.Headers["UserId"];
                            var messageRoomId = (int)ea.BasicProperties.Headers["RoomId"];
                            // If the message we are receiving is for this room
                            // and it has been generated by a different user
                            // add to the response
                            if (messageRoomId == roomId)
                            {
                                if (messageUserId != -1)
                                {
                                    DataLayer dl = new DataLayer();
                                    UserModel user = dl.GetUserById(messageUserId);
                                    response.AppendLine((messageUserId == userId ? "Me:" : user.username) + ":" + Encoding.UTF8.GetString(body) + "<br>");
                                }
                                else
                                {
                                    response.AppendLine(Encoding.UTF8.GetString(body) + "<br>");
                                }
                            }
                        }
                        return response.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }

        }

        public static string GetQueueName(int roomId, int userId)
        {
            return String.Format("ChatRoom={1}", userId, roomId);
        }
    }

}