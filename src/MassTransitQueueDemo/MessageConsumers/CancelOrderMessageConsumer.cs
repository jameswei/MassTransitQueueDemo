using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransitQueueDemo.MessageModels;

namespace MassTransitQueueDemo.MessageConsumers
{
    public class CancelOrderMessageConsumer : IConsumer<CancelOrderMessage>
    {
        public Task Consume(ConsumeContext<CancelOrderMessage> context)
        {
            Console.WriteLine("A CancelOrderMessage has been received!");
            Console.WriteLine($"Cancelling order {context.Message.OrderId}");
            return Task.CompletedTask;
        }
    }
}