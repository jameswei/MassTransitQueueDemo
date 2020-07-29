using System;

namespace MassTransitQueueDemo.MessageModels
{
    public class CreateOrderMessage
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; }
    }
}