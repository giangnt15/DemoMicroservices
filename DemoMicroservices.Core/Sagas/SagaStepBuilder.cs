using DemoMicroservices.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMicroservices.Core.Sagas
{
    public class SagaStepBuilder<TData>
    {
        private readonly SagaDefinitionBuilder<TData> _parent;
        private ISagaStepBuilder<TData> _builder;
        public SagaStepBuilder(SagaDefinitionBuilder<TData> parent)
        {
            _parent = parent;
        }
        public LocalSagaStepBuilder<TData> InvokeLocal(Func<TData, Task> action)
        {
            var localStepBuilder = new LocalSagaStepBuilder<TData>(_parent).WithAction(action);
            _builder = localStepBuilder;
            return localStepBuilder;
        }

        public LocalSagaStepBuilder<TData> WithCompensation(Func<TData, Task> action)
        {
            var localStepBuilder = new LocalSagaStepBuilder<TData>(_parent).WithCompensation(action);
            _builder = localStepBuilder;
            return localStepBuilder;
        }

        public ParticipantInvocationStepBuilder<TData> InvokeParticipant(Func<TData, List<CommandEnvelop>> action)
        {
            var participantStepBuilder = new ParticipantInvocationStepBuilder<TData>(_parent).WithAction(action);
            _builder = participantStepBuilder;
            return participantStepBuilder;
        }

        public ParticipantInvocationStepBuilder<TData> WithCompensation(Func<TData, List<CommandEnvelop>> action)
        {
            var participantStepBuilder = new ParticipantInvocationStepBuilder<TData>(_parent).WithCompensation(action);
            _builder = participantStepBuilder;
            return participantStepBuilder;
        }

        public SagaStepBuilder<TData> Step()
        {
            return _builder.Step();
        }
    }
}
