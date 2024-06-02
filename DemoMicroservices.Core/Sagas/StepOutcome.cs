using DemoMicroservices.Core.Commands;
using DemoMicroservices.Core.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMicroservices.Core.Sagas
{
    public interface IStepOutcome
    {

    }

    public abstract class StepOutcome : IStepOutcome
    {
        public static IStepOutcome Remote(List<CommandEnvelop> commands)
        {
            return new RemoteStepOutCome(commands);
        }

        public static IStepOutcome FailureLocal(Exception localException)
        {
            return new LocalStepOutcome(localException);
        }

        public static IStepOutcome SuccessLocal() => new LocalStepOutcome();

        public class RemoteStepOutCome : StepOutcome
        {
            private readonly List<CommandEnvelop> _commands;

            internal RemoteStepOutCome(List<CommandEnvelop> commands)
            {
                _commands = commands;
            }

            public List<CommandEnvelop> Commands
            {
                get
                {
                    return _commands;
                }
            }
        }

        public class LocalStepOutcome : StepOutcome
        {
            private readonly Exception _localException;

            internal LocalStepOutcome()
            {
            }

            internal LocalStepOutcome(Exception localException)
            {
                _localException = localException;
            }

            public Exception LocalException
            {
                get
                {
                    return _localException;
                }
            }
        }
    }


}
