using System;

namespace MassTransitQueueDemo.MessageModels
{
    // 定义 message
    public class CreateOrderMessage
    {
        public int OrderId { get; set; }

        public DateTime OrderDate { get; set; }
    }
}