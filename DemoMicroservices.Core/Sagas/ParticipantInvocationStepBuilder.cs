using DemoMicroservices.Core.Commands;
using System;
using System.Collections.Generic;

namespace DemoMicroservices.Core.Sagas
{
    public class ParticipantInvocationStepBuilder<TData> : ISagaStepBuilder<TData>
    {
        private ParticipantInvocationStep<TData> _step;
        private SagaDefinitionBuilder<TData> _parent;

        public ParticipantInvocationStepBuilder(SagaDefinitionBuilder<TData> parent)
        {
            _step = new ParticipantInvocationStep<TData>();
            _parent = parent;
        }

        public ParticipantInvocationStepBuilder<TData> WithAction(Func<TData, List<CommandEnvelop>> action)
        {
            _step.SetAction(action);
            return this;
        }

        public ParticipantInvocationStepBuilder<TData> WithCompensation(Func<TData, List<CommandEnvelop>> compensation)
        {
            _step.SetCompensation(compensation);
            return this;
        }

        public ParticipantInvocationStepBuilder<TData> OnReply<TReply>(Action<TReply, TData> action) where TReply : SagaReplyMessage
        {
            _step.AddReplyHandler(action);
            return this;
        }

        public ISagaDefinition<TData> Build()
        {
            return _parent.Build();
        }

        public void Clear()
        {
            _step = new ParticipantInvocationStep<TData>();
        }

        public SagaStepBuilder<TData> Step()
        {
            _parent.AddStep(_step);
            Clear();
            return new SagaStepBuilder<TData>(_parent);
        }



    }
}
