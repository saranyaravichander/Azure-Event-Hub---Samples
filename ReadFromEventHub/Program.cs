using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;

namespace ReadFromEventHub
{
    class Program
    {
        static void Main(string[] args)
        {
            string eventHubConnectionString = "Endpoint=sb://novanteh.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=/X/1/IdRHCqqugRdSieg40PYpGwbCKQIQsmJRJSAGjM=";
            string eventHubName = "outgoingmsg";
            string storageAccountName = "novanteventproc9648";
            string storageAccountKey = "Vv3vWTAHj/MxcBg+zNQURKE2v0NniFnxtb6Cg7jR8/THMrPS+QCiMudJS5paBXb9iFVScRdN7rX33P6oxWu5hQ==";
            string storageConnectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", storageAccountName, storageAccountKey);

            string eventProcessorHostName = Guid.NewGuid().ToString();
            EventProcessorHost eventProcessorHost = new EventProcessorHost(
                eventProcessorHostName, 
                eventHubName, 
                EventHubConsumerGroup.DefaultGroupName, 
                eventHubConnectionString, 
                storageConnectionString);
            Console.WriteLine("Registering EventProcessor...");
            var options = new EventProcessorOptions();
            options.ExceptionReceived += (sender, e) => { Console.WriteLine(e.Exception); };
            eventProcessorHost.RegisterEventProcessorAsync<SimpleEventProcessor>(options).Wait();

            Console.WriteLine("Receiving. Press enter key to stop worker.");
            Console.ReadLine();
            eventProcessorHost.UnregisterEventProcessorAsync().Wait();
        }
    }
}
