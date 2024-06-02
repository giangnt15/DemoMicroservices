using Amazon.SQS;
using Amazon.SQS.Model;
using DemoMicroservices.Core.Messaging;

namespace DemoMicroservices.Core.Sqs
{
    public class MessageConsumer : IMessageConsumer
    {
        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void Subscribe<TEnvelop>(string subcriberId, string channel, Action<TEnvelop> handler) where TEnvelop : Envelop
        {
            throw new NotImplementedException();
        }

        public void Subscribe<TEnvelop>(string subcriberId, string channel, Func<TEnvelop, Task> handler) where TEnvelop : Envelop
        {
            Task.Factory.StartNew(async () =>
            {
                using var sqsClient = new AmazonSQSClient(new AmazonSQSConfig()
                {
                    RegionEndpoint = Amazon.RegionEndpoint.USEast2,
                });
                while (true)
                {
                    var receiveMessageResponse = await sqsClient.ReceiveMessageAsync(new ReceiveMessageRequest()
                    {

                    });
                    DeleteMessageRequest deleteMessageRequest = new DeleteMessageRequest()
                    {
                        ReceiptHandle = receiveMessageResponse.Messages[0].ReceiptHandle,
                    };
                }
            });
        }
    }
}
