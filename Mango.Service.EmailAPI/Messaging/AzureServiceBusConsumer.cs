using Azure.Messaging.ServiceBus;
using Mango.Service.EmailAPI.Models.Dto;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Service.EmailAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string emailCartQueue;
        private readonly IConfiguration _configuration;
        private ServiceBusProcessor _emailCartProcesor;

        public AzureServiceBusConsumer(IConfiguration configuration)
        {
            _configuration = configuration;
            serviceBusConnectionString = _configuration.GetValue<string>("SesrviceBusConnectionString");
            emailCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue");

            var client = new ServiceBusClient(serviceBusConnectionString);
            _emailCartProcesor = client.CreateProcessor(emailCartQueue);
        }

        public async Task Start()
        {
            _emailCartProcesor.ProcessMessageAsync += OnEmailCartRequestReceived;
            _emailCartProcesor.ProcessErrorAsync += ErrorHandler;
        }

        public async Task Stop()
        {
            await _emailCartProcesor.StopProcessingAsync();
            await _emailCartProcesor.DisposeAsync();
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task OnEmailCartRequestReceived(ProcessMessageEventArgs args)
        {
            // this is where you recevie the message
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            CartDto objMessage = JsonConvert.DeserializeObject<CartDto>(body);

            try
            {
                // try to log email
                await args.CompleteMessageAsync(args.Message);
            } catch(Exception ex) {
                throw;
            }
        }
    }
}
