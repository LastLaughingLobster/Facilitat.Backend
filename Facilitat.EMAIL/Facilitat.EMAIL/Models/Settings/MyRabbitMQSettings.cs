namespace Facilitat.EMAIL.Models.Settings
{
    public class MyRabbitMQSettings
    {
        public string Hostname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string QueueName { get; set; }
        public string DeadLetterExchange { get; set; }
        public string RetryQueueName { get; set; }
        public int RetryMessageTTL { get; set; }
    }

}
