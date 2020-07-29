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

            var busControl = CreateBusControl(configuration);

            Task.Factory.StartNew(async () => await busControl.StartAsync());

            var sendEndpoint = await busControl.GetSendEndpoint(new Uri(configuration.QueueUrl));

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
                    var message = new CreateOrderMessage() { Id = ++orderId, OrderDate = DateTime.Now };

                    await sendEndpoint.Send(message);

                    Console.WriteLine($"Order with Id {message.Id} has been sent.");
                }
                else if (key.Key == ConsoleKey.NumPad2)
                {
                    Console.WriteLine("Enter the Id of the order that you want to cancel: ");

                    string cancelOrderId = Console.ReadLine();

                    var m2 = new CancelOrderMessage() { OrderId = Int32.Parse(cancelOrderId) };

                    await sendEndpoint.Send(m2);
                }

            } while (key.Key != ConsoleKey.Q);

            Console.WriteLine("Stopping...");

            await Task.Delay(2000);

            await busControl.StopAsync();

            Console.WriteLine("MassTransit Queue demo stopped.");
        }

        private static Configuration BuildConfiguration()
        {
            var settings = new Configuration();

            var configuration = new ConfigurationBuilder()
                                .SetBasePath(Environment.CurrentDirectory)
                                .AddJsonFile("appSettings.json", optional: false)
                                .AddJsonFile("appSettings.development.json", optional: true)
                                .Build();

            configuration.Bind(settings);

            return settings;
        }

        private static IBusControl CreateBusControl(Configuration configuration)
        {
            return
                Bus.Factory.CreateUsingAzureServiceBus(configure =>
                {
                    configure.Host(configuration.ServiceBusConnectionString);

                    configure.ReceiveEndpoint(configuration.QueueName, configurator =>
                    {
                        // Since we're not using Topics, or other non-basic ServiceBus features,
                        // we should call the SelectBasicTier method to avoid exceptions thrown by
                        // MassTransit while it tries to enable those features.
                        configurator.SelectBasicTier();
                        configurator.Consumer<CreateOrderMessageConsumer>();
                        configurator.Consumer<CancelOrderMessageConsumer>();
                    });
                });
        }
    }
}