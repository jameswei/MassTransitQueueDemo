using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransitQueueDemo.MessageModels;

namespace MassTransitQueueDemo.MessageConsumers
{
    public class CreateOrderMessageConsumer : IConsumer<CreateOrderMessage>
    {
        public Task Consume(ConsumeContext<CreateOrderMessage> context)
        {
            Console.WriteLine("A CreateOrderMessage has been received!");
            Console.WriteLine($"Order {context.Message.Id} with orderdate {context.Message.OrderDate} processed!");

            return Task.CompletedTask;
        }
    }
}