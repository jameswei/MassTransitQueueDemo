using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransitQueueDemo.MessageConsumers;
using MassTransitQueueDemo.MessageModels;
using Microsoft.Extensions.Configuration;

namespace MassTransitQueueDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = BuildConfiguration();

            var bus = CreateBusControl(configuration);

            await Task.Factory.StartNew(async () => await bus.StartAsync());
            // 通过指定的 queue uri 创建 send endpoint
            var sendEndpoint = await bus.GetSendEndpoint(new Uri(configuration.QueueUrl));

            Console.WriteLine("Press a key:");
            Console.WriteLine(" 1 to send a CreateOrder message");
            Console.WriteLine(" 2 to send a CancelOrder message");
            Console.WriteLine(" q to stop the application");

            ConsoleKeyInfo key;

            int orderId = 0;

            do
            {
                key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key == ConsoleKey.NumPad1)
                {
                    var message = new CreateOrderMessage() { OrderId = ++orderId, OrderDate = DateTime.Now };

                    await sendEndpoint.Send(message);

                    Console.WriteLine($"Order with Id {message.OrderId} has been sent.");
                }
                else if (key.Key == ConsoleKey.NumPad2)
                {
                    Console.WriteLine("Enter the Id of the order that you want to cancel: ");

                    string cancelOrderId = Console.ReadLine();

                    var m2 = new CancelOrderMessage() { OrderId = Int32.Parse(cancelOrderId) };

                    await sendEndpoint.Send(m2);
                }
                else if (key.Key == ConsoleKey.Q)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input, press key either 1, 2, or q.");
                }

            } while (true);

            Console.WriteLine("Stopping...");

            await Task.Delay(2000);

            await bus.StopAsync();

            Console.WriteLine("MassTransit Queue demo stopped.");
        }

        // 加载解析 appsettings.{ENV}.json
        private static Configuration BuildConfiguration()
        {
            var appSettings = new Configuration();

            var configuration = new ConfigurationBuilder()
                                .SetBasePath(Environment.CurrentDirectory)
                                .AddJsonFile("appSettings.json", optional: false)
                                .AddJsonFile("appSettings.development.json", optional: true)
                                .Build();

            configuration.Bind(appSettings);

            return appSettings;
        }

        // 创建 MassTransit 的 service bus
        private static IBusControl CreateBusControl(Configuration configuration)
        {
            return
            // 使用 ASB 作为 transport provider
                Bus.Factory.CreateUsingAzureServiceBus(configure =>
                {
                    configure.Host(configuration.ServiceBusConnectionString);
                    configure.ReceiveEndpoint(configuration.QueueName, configurator =>
                    {
                        // Since we're not using Topics, or other non-basic ServiceBus features,
                        // we should call the SelectBasicTier method to avoid exceptions thrown by
                        // MassTransit while it tries to enable those features.
                        // 只使用 ASB queue，所以显式指定 basic-tier，避免 MassTransit 隐式的创建 topic和subscription
                        configurator.SelectBasicTier();
                        // 指定要消费的 message type
                        configurator.Consumer<CreateOrderMessageConsumer>();
                        configurator.Consumer<CancelOrderMessageConsumer>();
                    });
                });
        }
    }
}