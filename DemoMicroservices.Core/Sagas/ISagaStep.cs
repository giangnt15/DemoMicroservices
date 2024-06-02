using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMicroservices.Core.Sagas
{
    public interface ISagaStep<TData>
    {
        bool HasAction();
        bool HasCompensation();
        Task<IStepOutcome> Execute(TData data);
        Task<IStepOutcome> Compensate(TData data);
        bool IsSuccessful(SagaReplyEnvelop sagaReplyEnvelop);
        Action<string, TData> GetReplyHandler(string replyTypeName);
        bool IsEndStep { get; }
    }
}
