using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransitQueueDemo.MessageModels;

namespace MassTransitQueueDemo.MessageConsumers
{
    // 实现 IConsumer，通过类型参数指定要 consume 的 message type
    public class CreateOrderMessageConsumer : IConsumer<CreateOrderMessage>
    {
        public Task Consume(ConsumeContext<CreateOrderMessage> context)
        {
            Console.WriteLine("A CreateOrderMessage has been received!");
            Console.WriteLine($"Processing order {context.Message.OrderId} @ {context.Message.OrderDate}");
            return Task.CompletedTask;
        }
    }
}