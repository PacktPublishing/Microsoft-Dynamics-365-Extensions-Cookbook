using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xrm.Sdk;
using Microsoft.ServiceBus.Messaging;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace Xrm.Packt.Azure
{
    class EndPointListner
    {
        private const string EndpointUri = "https://.documents.azure.com:443/";
        private const string PrimaryKey = "=";
        private const string DatabaseName = "Dynamics 365 Complement";
        private const string CollectionName = "Auditing";
        private static DocumentClient client;

        static void Main(string[] args)
        {
            var connectionString = "Endpoint=sb://servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey==";
            var queueName = "d365queue";

            var queueClient = QueueClient.CreateFromConnectionString(connectionString, queueName);

            queueClient.OnMessage(message =>
            {
                var result = message.GetBody<RemoteExecutionContext>();

                var readLog = string.Format("{userId={0},entity={1},entityId={2}}", result.InitiatingUserId, result.PrimaryEntityName, result.PrimaryEntityId);

                client = new DocumentClient(new Uri(EndpointUri), PrimaryKey);
                client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName), readLog).Wait();
            });            

            Console.WriteLine("Press [Enter] to terminate");
            Console.ReadLine();
            queueClient.Close();
        }
    }
}
