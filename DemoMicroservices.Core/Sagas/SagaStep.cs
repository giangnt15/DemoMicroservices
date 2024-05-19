using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMicroservices.Core.Sagas
{
    public class SagaStep : ISagaStep
    {
        public bool HasAction()
        {
            throw new NotImplementedException();
        }

        public bool HasCompensation()
        {
            throw new NotImplementedException();
        }
    }
}
