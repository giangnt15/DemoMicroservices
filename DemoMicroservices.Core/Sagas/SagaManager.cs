using DemoMicroservices.Core.Messaging;
using DemoMicroservices.Core.Shared.Commands;
using DemoMicroservices.Core.Shared.Common;
using DemoMicroservices.Core.Shared.Sagas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMicroservices.Core.Sagas
{
    public class SagaManager<TData> : ISagaManager<TData>
    {
        private readonly ISaga<TData> _saga;
        private readonly IMessageConsumer _messageConsumer;
        private readonly ISagaRepository _sagaRepository;
        private readonly ISagaDefinition<TData> _sagaDefinition;

        public SagaManager(ISaga<TData> saga, IMessageConsumer messageConsumer, 
            ISagaRepository sagaRepository, ISagaDefinition<TData> sagaDefinition)
        {
            _saga = saga;
            _messageConsumer = messageConsumer;
            _sagaRepository = sagaRepository;
            _sagaDefinition = sagaDefinition;
        }

        public async Task ProcessReplyAsync(SagaReplyEnvelop sagaReplyEnvelop)
        {
            var replyType = sagaReplyEnvelop.GetHeader(SagaReplyHeaders.SAGA_TYPE);
            var sagaId = sagaReplyEnvelop.GetHeader(SagaReplyHeaders.SAGA_ID);
            
            var sagaInstance = await _sagaRepository.LoadAsync(sagaId, replyType);
            if (sagaInstance == null)
            {
                return;
            }
            TData data = System.Text.Json.JsonSerializer.Deserialize<TData>(sagaInstance.SerializedData);
            var outcome = await _sagaDefinition.HandleReply(sagaReplyEnvelop, data, null);
            await ProcessReplyOutcome(outcome , sagaInstance);
        }

        private async Task ProcessReplyOutcome(SagaHandleReplyOutcome<TData> outcome, SagaInstance sagaInstance)
        {
            while (true)
            {
                if (outcome.IsLocalFailure)
                {
                    outcome = await _sagaDefinition.HandleReply(new SagaReplyEnvelop()
                    {
                        Message = "{}",
                        Headers = new Dictionary<string, string>()
                        {
                            { ReplyMessageHeaders.REPLY_TYPE, typeof(PseudoSagaReplyMessage).AssemblyQualifiedName },
                            { SagaReplyHeaders.OUTCOME, CommandReplyOutcome.FAILURE }
                        }
                    }, outcome.Data, outcome.ExecutionState);
                }
                else
                {
                    sagaInstance.SetCommands(outcome.CommandsToSend);
                    sagaInstance.SerializedData = System.Text.Json.JsonSerializer.Serialize(outcome.Data);
                    sagaInstance.SetState(outcome.ExecutionState);
                    await _sagaRepository.SaveAsync(sagaInstance);

                    if (outcome.ExecutionState.Ended)
                    {
                        //TODO: Xử lý kết thúc saga
                    }
                    
                    if (outcome.IsReplyExpected)
                    {
                        return;
                    }
                    else
                    {
                        outcome = await _sagaDefinition.HandleReply(new SagaReplyEnvelop()
                        {
                            Message = "{}",
                            Headers = new Dictionary<string, string>()
                            {
                                { ReplyMessageHeaders.REPLY_TYPE, typeof(PseudoSagaReplyMessage).AssemblyQualifiedName },
                                { SagaReplyHeaders.OUTCOME, CommandReplyOutcome.SUCCESS }
                            }
                        }, outcome.Data, outcome.ExecutionState);
                    }
                }
            }
        }
            

        public async Task CreateAsync(TData data)
        {
            var sagaInstance = new SagaInstance()
            {
                Id = Guid.NewGuid(),
                SagaType = _saga.GetSagaType(),
                SerializedData = System.Text.Json.JsonSerializer.Serialize(data),
            };
            await _sagaRepository.SaveAsync(sagaInstance);
            var startOutcome = await _sagaDefinition.StartAsync(data);
            await ProcessReplyOutcome(startOutcome, sagaInstance);
        }

        public void SubscribeToReplyChannel()
        {
            _messageConsumer.Subscribe<SagaReplyEnvelop>(_saga.GetSagaType(), _saga.GetReplyChannel(), ProcessReplyAsync);
        }

    }
}
