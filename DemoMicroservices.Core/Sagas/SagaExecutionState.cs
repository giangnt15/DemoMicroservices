using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMicroservices.Core.Sagas
{
    public class SagaExecutionState
    {
        private int _currentStep;
        private bool compensating;
        private bool ended;
        private bool failed;

        //default constructor
        public SagaExecutionState()
        {
            CurrentStep = -1;
            Compensating = false;
            Ended = false;
            Failed = false;
        }

        //constructor with parameters
        public SagaExecutionState(int currentStep, bool compensating)
        {
            CurrentStep = currentStep;
            Compensating = compensating;
        }


        // create getters and setters
        public int CurrentStep
        {
            get { return _currentStep; }
            set { _currentStep = value; }
        }
        public bool Compensating
        {
            get { return compensating; }
            set { compensating = value; }
        }
        public bool Ended
        {
            get { return ended; }
            set { ended = value; }
        }
        public bool Failed
        {
            get { return failed; }
            set { failed = value; }
        }

        public static SagaExecutionState StartState()
        {
            return new SagaExecutionState(-1, false);
        }

        public SagaExecutionState StartCompensating()
        {
            return new SagaExecutionState(CurrentStep, true);
        }

        public SagaExecutionState NextState()
        {
            return new SagaExecutionState(Compensating ? CurrentStep - 1 : CurrentStep + 1, Compensating);
        }

         public SagaExecutionState ToState(int stepIndex)
        {
            return new SagaExecutionState(stepIndex, Compensating);
        }


        public static SagaExecutionState EndedState()
        {
            return new SagaExecutionState() { Ended = true };
        }

        public static SagaExecutionState FailedState()
        {
            return new SagaExecutionState() { Failed = true, Ended = true };
        }

    }
}
