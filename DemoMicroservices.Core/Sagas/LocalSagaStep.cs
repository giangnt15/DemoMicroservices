using DemoMicroservices.Core.Shared.Commands;
using DemoMicroservices.Core.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMicroservices.Core.Sagas
{
    public class LocalSagaStep<TData> : ISagaStep<TData>
    {
        private Func<TData, Task> _action;
        private Func<TData, Task> _compensation;
        private readonly bool _isEndStep;

        public LocalSagaStep()
        {
        }

        private LocalSagaStep(bool isEndStep)
        {
            _isEndStep = isEndStep;
        }

        public async Task<IStepOutcome> Compensate(TData data)
        {
            try
            {
                await _compensation?.Invoke(data);
                return StepOutcome.SuccessLocal();
            }
            catch (Exception ex)
            {
                return StepOutcome.FailureLocal(ex);
            }
        }

        public async Task<IStepOutcome> Execute(TData data)
        {
            try
            {
                await _action?.Invoke(data);
                return StepOutcome.SuccessLocal();
            }
            catch (Exception ex)
            {
                return StepOutcome.FailureLocal(ex);
            }
        }

        public Action<string> GetReplyHandler(string replyTypeName)
        {
            return null;
        }

        public bool HasAction()
        {
            return _action != null;
        }

        public bool HasCompensation()
        {
            return _compensation != null;
        }

        public bool IsSuccessful(SagaReplyEnvelop sagaReplyEnvelop)
        {
            return sagaReplyEnvelop.GetHeader(ReplyMessageHeaders.COMMAND_OUTCOME).Equals(CommandReplyOutcome.SUCCESS);
        }

        public void SetAction(Func<TData, Task> action)
        {
            ArgumentNullException.ThrowIfNull(action, nameof(action));
            _action = action;
        }

        public void SetCompensation(Func<TData, Task> compensation)
        {
            ArgumentNullException.ThrowIfNull(compensation, nameof(compensation));
            _compensation = compensation;
        }

        public bool IsEndStep => _isEndStep;

        public static LocalSagaStep<TData> EndStep()
        {
            var step = new LocalSagaStep<TData>(true);
            return step;
        }

        Action<string, TData> ISagaStep<TData>.GetReplyHandler(string replyTypeName)
        {
            throw new NotImplementedException();
        }
    }
}
