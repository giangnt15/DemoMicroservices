using DemoMicroservices.Core.Commands;
using DemoMicroservices.Core.Messaging;
using DemoMicroservices.Core.Shared.Commands;
using DemoMicroservices.Core.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMicroservices.Core.Sagas
{
    public class ParticipantInvocationStep<TData> : ISagaStep<TData>
    {
        private Func<TData, List<CommandEnvelop>> _action;
        private Func<TData, List<CommandEnvelop>> _compensation;

        private readonly Dictionary<string, Action<string, TData>> _actionReplyHandlers;
        private readonly Dictionary<string, Action<string, TData>> _compensationReplyHandlers;

        public bool IsEndStep => false;

        public ParticipantInvocationStep()
        {
            _actionReplyHandlers = new();
            _compensationReplyHandlers = new();
        }

        public ParticipantInvocationStep(Func<TData, List<CommandEnvelop>> action)
        {
            SetAction(action);
        }

        public Task<IStepOutcome> Compensate(TData data)
        {
            return Task.FromResult(StepOutcome.Remote(_compensation(data)));
        }

        public Task<IStepOutcome> Execute(TData data)
        {
            return Task.FromResult(StepOutcome.Remote(_action(data)));
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

        public void SetAction(Func<TData, List<CommandEnvelop>> action)
        {
            ArgumentNullException.ThrowIfNull(action, nameof(action));
            _action = action;
        }

        public void SetCompensation(Func<TData, List<CommandEnvelop>> compensation)
        {
            ArgumentNullException.ThrowIfNull(compensation, nameof(compensation));
            _compensation = compensation;
        }

        public void AddReplyHandler<TReply>(Action<TReply, TData> action) where TReply : SagaReplyMessage
        {
            if (HasCompensation())
            {
                _compensationReplyHandlers.Add(typeof(TReply).AssemblyQualifiedName, (msg, data) => action(Message.FromJson<TReply>(msg),data));
            }
            else
            {
                _actionReplyHandlers.Add(typeof(TReply).AssemblyQualifiedName, (msg, data) => action(Message.FromJson<TReply>(msg),data));
            }
        }

        public Action<string, TData> GetReplyHandler(string replyTypeName)
        {
            if (HasCompensation())
            {
                return _compensationReplyHandlers.GetValueOrDefault(replyTypeName);
            }
            else
            {
                return _actionReplyHandlers.GetValueOrDefault(replyTypeName);
            }
        }
    }
}
