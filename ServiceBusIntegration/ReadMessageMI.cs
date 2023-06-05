using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using System.Threading;

namespace fnReadMessage
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            await ReadMessage(log);

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }

        public static async Task ReadMessage(ILogger log)
        {
            // the client that owns the connection and can be used to create senders and receivers
            ServiceBusClient client;

            // the processor that reads and processes messages from the queue
            ServiceBusProcessor processor;

            // The Service Bus client types are safe to cache and use as a singleton for the lifetime
            // of the application, which is best practice when messages are being published or read
            // regularly.
            //
            // Set the transport type to AmqpWebSockets so that the ServiceBusClient uses port 443.
            // If you use the default AmqpTcp, make sure that ports 5671 and 5672 are open.

            // TODO: Replace the <NAMESPACE-NAME> and <QUEUE-NAME> placeholders
            var clientOptions = new ServiceBusClientOptions()
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            };
            client = new ServiceBusClient("fnServiceBus.servicebus.windows.net",new DefaultAzureCredential(), clientOptions);

            // create a processor that we can use to process the messages
            // TODO: Replace the <QUEUE-NAME> placeholder
            processor = client.CreateProcessor("fnqueue", new ServiceBusProcessorOptions());

            try
            {
                // add handler to process messages
                processor.ProcessMessageAsync += MessageHandler;

                // add handler to process any errors
                processor.ProcessErrorAsync += ErrorHandler;

                // start processing 
                await processor.StartProcessingAsync(); 
                Thread.Sleep(10000);
                await processor.StopProcessingAsync();
                
            }
            finally
            {
                // Calling DisposeAsync on client types is required to ensure that network
                // resources and other unmanaged objects are properly cleaned up.
                await processor.DisposeAsync();
                await client.DisposeAsync();
            }

            // handle received messages
            async Task MessageHandler(ProcessMessageEventArgs args)
            {
                string body = args.Message.Body.ToString();
                log.LogInformation($"Received: {body}");

                // complete the message. message is deleted from the queue. 
                await args.CompleteMessageAsync(args.Message);
            }

            // handle any errors when receiving messages
            Task ErrorHandler(ProcessErrorEventArgs args)
            {
                log.LogInformation(args.Exception.ToString());
                return Task.CompletedTask;
            }

        }
    }
}
