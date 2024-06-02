using DemoMicroservices.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DemoMicroservices.Core.Sagas
{
    public class SagaInstance
    {
        public Guid Id { get; set; }
        public string SagaType { get; set; }
        public string SerializedData { get; set; }
        public int CurrentState { get;set; }
        public bool Compensating { get; set; }
        public bool Failed { get; set; }
        public bool Ended { get; set; }
        public string LastCommandId { get; set; }

        private List<CommandEnvelop> _commands;

        public void SetCommands(List<CommandEnvelop> commands)
        {
            _commands = commands;
        }

        public List<CommandEnvelop> GetCommands()
        {
            return _commands?.ToList();
        }
        public void ClearCommands()
        {
            _commands.Clear();
        }

        public void SetState(SagaExecutionState state)
        {
            CurrentState = state.CurrentStep;
            Compensating = state.Compensating;
            Failed = state.Failed;
            Ended = state.Ended;
        }

        public SagaInstance()
        {
        }

        public SagaInstance(Guid id, string sagaType, string serializedData, int currentState,
            bool compensating, bool failed, bool ended, string lastCommandId)
        {
            Id = id;
            SagaType = sagaType;
            SerializedData = serializedData;
            CurrentState = currentState;
            Compensating = compensating;
            Failed = failed;
            Ended = ended;
            LastCommandId = lastCommandId;
        }

    }
}
