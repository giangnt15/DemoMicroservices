using DemoMicroservices.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMicroservices.Core.Sagas
{
    public class SagaHandleReplyOutcome<TData>
    {
        public List<CommandEnvelop> CommandsToSend { get; internal set; }
        public SagaExecutionState ExecutionState { get; internal set; }
        public Exception LocalException { get; internal set; }
        public TData Data { get; internal set; }

        public bool IsReplyExpected => CommandsToSend != null && CommandsToSend.Count > 0;
        public bool IsLocalFailure => LocalException != null;

        public static SagaHandleReplyOutcome<TData> Create(IStepOutcome stepOutcome, SagaExecutionState executionState, TData data)
        {
            return stepOutcome switch
            {
                StepOutcome.RemoteStepOutCome remote => new SagaHandleReplyOutcome<TData>
                {
                    CommandsToSend = remote.Commands,
                    ExecutionState = executionState,
                    Data = data
                },
                StepOutcome.LocalStepOutcome local => new SagaHandleReplyOutcome<TData>
                {
                    ExecutionState = executionState,
                    LocalException = local.LocalException,
                    Data = data
                },
                _ => throw new InvalidOperationException("Unknown step outcome")
            };
        }

        public static SagaHandleReplyOutcome<TData> Create(SagaExecutionState executionState, TData data)
        {
            return new SagaHandleReplyOutcome<TData>
            {
                ExecutionState = executionState,
                Data = data
            };
        }
    }
}
