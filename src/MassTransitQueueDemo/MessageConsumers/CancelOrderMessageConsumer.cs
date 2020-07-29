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
            Console.WriteLine($"Canceling order with ID {context.Message.OrderId}");

            return Task.CompletedTask;
        }
    }
}