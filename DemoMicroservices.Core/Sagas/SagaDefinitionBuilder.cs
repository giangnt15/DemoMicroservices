using DemoMicroservices.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DemoMicroservices.Core.Sagas
{
    public class SagaDefinitionBuilder<TData>
    {
        private readonly SagaDefinition<TData> _definition = new();
        private SagaStepBuilder<TData> _stepBuilder;

        public SagaStepBuilder<TData> Step()
        {
            if (_stepBuilder != null)
            {
                _stepBuilder = _stepBuilder.Step();
            }
            else
            {
                _stepBuilder = new SagaStepBuilder<TData>(this);
            }
            return _stepBuilder;
        }

        public SagaDefinitionBuilder<TData> AddStep(ISagaStep<TData> sagaStep)
        {
            _definition.AddStep(sagaStep);
            return this;
        }

        public ISagaDefinition<TData> Build()
        {
            _stepBuilder.Step();
            return _definition;
        }
    }
}
