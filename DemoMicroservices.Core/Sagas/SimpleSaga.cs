using DemoMicroservices.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DemoMicroservices.Core.Sagas
{
    public class SimpleSaga<TData> : ISaga<TData>
    {
        protected ISagaDefinition<TData> _definition;
        protected TData _data;
        protected SagaDefinitionBuilder<TData> _definitionBuilder = new SagaDefinitionBuilder<TData>();
        protected SagaExecutionState _state;

        public SimpleSaga()
        {
            _state = new SagaExecutionState(-1, false);
        }

        public ISagaDefinition<TData> Definition => _definition;

        protected List<IEvent> Events { get; set; } = new List<IEvent>();
        protected List<ICommand> Commands { get; set; } = new List<ICommand>();

        public List<ICommand> GetCommands()
        {
            return Commands.ToList();
        }
        public List<IEvent> GetEvents()
        {
            return Events.ToList();
        }

        public void ClearCommands()
        {
            Commands.Clear();
        }
        public void ClearEvents()
        {
            Events.Clear();
        }

        public string GetSagaType()
        {
            return GetType().Name;
        }

        public string GetReplyChannel()
        {
            return GetSagaType() + "-reply";
        }
    }
}
