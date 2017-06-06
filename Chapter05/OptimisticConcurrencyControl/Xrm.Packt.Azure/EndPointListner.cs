using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xrm.Sdk;
using Microsoft.ServiceBus.Messaging;


namespace Xrm.Packt.Azure
{
    class EndPointListner
    {
        static void Main(string[] args)
        {
            var connectionString = "Endpoint=sb://dynamics1.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=";
            var queueName = "d365queue";

            var client = QueueClient.CreateFromConnectionString(connectionString, queueName);

            client.OnMessage(message =>
            {
                var result = message.GetBody<RemoteExecutionContext>();
                Console.WriteLine(string.Format("Message id: {0}", message.MessageId));
                Console.WriteLine(result.PrimaryEntityName);
                Console.WriteLine(result.PrimaryEntityId);
            });            

            Console.WriteLine("Press [Enter] to terminate");
            Console.ReadLine();
            client.Close();
        }
    }
}
