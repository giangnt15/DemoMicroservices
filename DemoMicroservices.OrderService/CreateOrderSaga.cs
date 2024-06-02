using DemoMicroservices.Core.Commands;
using DemoMicroservices.Core.Messaging;
using DemoMicroservices.Core.Saga;
using DemoMicroservices.Core.Shared.Sagas;

namespace DemoMicroservices.OrderService
{
    public class CreateOrderSaga : AbstractSaga<CreateOrderSagaData>
    {
        public CreateOrderSaga()
        {
            _stepBuilder
                    .Step()
                        .WithStateName("CreatingOrder")
                    .Step()
                        .WithStateName("CreatingTicket")
                        .InvokeParticipant(data =>
                        {
                            return
                                EnvelopFactory.CreateCommandEnvelop<CreateTicketCmd>(
                                            new CreateTicketCmd
                                            {
                                                OrderId = data.OrderId,
                                                CustomerId = data.CustomerId,
                                                ProductId = data.ProductId,
                                                Quantity = data.Quantity
                                            }, new Dictionary<string, string>() {
                                                { SagaCommandHeaders.SAGA_TYPE, typeof(CreateOrderSaga).AssemblyQualifiedName }                                             
                                                
                                            }, "abc", "xyz");
                        })
                        .OnReply<CreateTicketRep>((msg, state, stateName, data) =>
                        {
                            data.TicketId = msg.TicketId;
                        }).WithCompensation(data =>
                        {
                             return  EnvelopFactory.CreateCommandEnvelop<RejectOrderCmd>(
                                            new RejectOrderCmd
                                            {
                                                OrderId = data.OrderId,
                                            }, new Dictionary<string, string>(), "abc", "xyz");
                        });
        }
    }

    public class CreateTicketCmd : Command
    {
        public string OrderId { get; set; }
        public string CustomerId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class RejectOrderCmd : Command
    {
        public string OrderId { get; set; }
    }

    public class CreateOrderSagaData : SagaData
    {
        public string OrderId { get; set; }
        public string CustomerId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }

        public string TicketId { get; set; }

        public void Handle(CreateTicketRep createTicketRep)
        {
            TicketId = createTicketRep.TicketId;
        }
    }

    public class CreateTicketRep : SagaReplyMessage
    {
        public string TicketId { get; set; }
    }

}
