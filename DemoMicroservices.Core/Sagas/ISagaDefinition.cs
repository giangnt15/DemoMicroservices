using DemoMicroservices.Core.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMicroservices.Core.Sagas
{
    public interface ISagaDefinition<TData>
    {
        ISagaDefinition<TData> AddStep(ISagaStep<TData> step);
        IEnumerable<ISagaStep<TData>> GetSteps();
        Task<SagaHandleReplyOutcome<TData>> HandleReply(SagaReplyEnvelop envelop, TData data, SagaExecutionState state);
        Task<SagaHandleReplyOutcome<TData>> StartAsync(TData data);
    }
}
