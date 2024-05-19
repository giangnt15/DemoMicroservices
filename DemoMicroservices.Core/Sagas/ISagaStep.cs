using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMicroservices.Core.Sagas
{
    public interface ISagaStep
    {
        bool HasAction();
        bool HasCompensation();
    }
}
