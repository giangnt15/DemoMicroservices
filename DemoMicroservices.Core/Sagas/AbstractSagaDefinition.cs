using DemoMicroservices.Core.Messaging;
using DemoMicroservices.Core.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMicroservices.Core.Sagas
{
    public abstract class AbstractSagaDefinition<TData> : ISagaDefinition<TData>
    {
        protected List<ISagaStep<TData>> _steps = new List<ISagaStep<TData>>();

        public ISagaDefinition<TData> AddStep(ISagaStep<TData> step)
        {
            _steps.Add(step);
            return this;
        }

        public IEnumerable<ISagaStep<TData>> GetSteps()
        {
            return _steps;
        }

        public async Task<SagaHandleReplyOutcome<TData>> HandleReply(SagaReplyEnvelop envelop, TData data, SagaExecutionState state)
        {
            var currentStep = _steps[state.CurrentStep];
            var replyTypeName = envelop.GetHeader(ReplyMessageHeaders.REPLY_TYPE);
            currentStep.GetReplyHandler(replyTypeName)?.Invoke(envelop.Message, data);
            if (currentStep.IsSuccessful(envelop))
            {
                var (step, newState) = NextStep(state);
                if (!newState.Ended)
                {
                    var outcome = await step.Execute(data);
                    return SagaHandleReplyOutcome<TData>.Create(outcome, newState, data);
                }
                return SagaHandleReplyOutcome<TData>.Create(newState, data);
            }
            else if (state.Compensating)
            {
                return SagaHandleReplyOutcome<TData>.Create(SagaExecutionState.FailedState(), data);
            }
            else
            {
                var compensateState = state.StartCompensating();
                var (step, newState) = NextStep(compensateState);
                if (!newState.Ended)
                {
                    var outcome = await step.Compensate(data);
                    return SagaHandleReplyOutcome<TData>.Create(outcome, newState, data);
                }
                return SagaHandleReplyOutcome<TData>.Create(newState, data);
            }
        }

        public async Task<SagaHandleReplyOutcome<TData>> StartAsync(TData data)
        {
            var state = SagaExecutionState.StartState();
            var (step, newState) = NextStep(state);
            var stepOutcome = await step.Execute(data);
            return SagaHandleReplyOutcome<TData>.Create(stepOutcome, newState, data);
        }

        protected (ISagaStep<TData>, SagaExecutionState state) NextStep(SagaExecutionState state)
        {
            var direction = state.Compensating ? -1 : 1;
            for (var i = state.CurrentStep + direction; i >= 0 && i < _steps.Count; i += direction)
            {
                var step = _steps[i];
                if (state.Compensating)
                {
                    if (step.HasCompensation())
                    {
                        return (step, state.ToState(i));
                    }
                }
                else
                {
                    if (step.HasAction())
                    {
                        return (step, state.ToState(i));
                    }
                }
            }

            return (LocalSagaStep<TData>.EndStep(), SagaExecutionState.EndedState());
        }
    }
}
