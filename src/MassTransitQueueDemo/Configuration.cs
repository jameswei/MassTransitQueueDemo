namespace MassTransitQueueDemo
{
    /* 对应 appsettings 的配置项，用来加载和解析配置
    {
      "ServiceBusConnectionString": "<ConnectionString to the SB namespace>",
      "QueueName": "<Name of the Queue in the ServiceBus namespace that must be used>",
      "QueueUrl": "<Url of the Queue that must be used; must start with sb://>" 
    } 
    */
    public class Configuration
    {
        public string ServiceBusConnectionString { get; set; }
        public string QueueName { get; set; }
        public string QueueUrl { get; set; }
    }
}