using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMicroservices.Core.Sagas
{
    public class LocalSagaStepBuilder<TData> : ISagaStepBuilder<TData>
    {
        private LocalSagaStep<TData> _step;
        private readonly SagaDefinitionBuilder<TData> _parent;
        public LocalSagaStepBuilder(SagaDefinitionBuilder<TData> parent)
        {
            _step = new LocalSagaStep<TData>();
            _parent = parent;
        }

        public LocalSagaStepBuilder<TData> WithAction(Func<TData, Task> action)
        {
            _step.SetAction(action);
            return this;
        }
        public LocalSagaStepBuilder<TData> WithCompensation(Func<TData, Task> compensation)
        {
            _step.SetCompensation(compensation);
            return this;
        }

        public ISagaDefinition<TData> Build()
        {
            return _parent.Build();
        }

        public void Clear()
        {
            _step = new LocalSagaStep<TData>();
        }

        public SagaStepBuilder<TData> Step()
        {
             _parent.AddStep(_step);
            Clear();
            return new SagaStepBuilder<TData>(_parent);
        }
    }
}
