using DemoMicroservices.Core.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMicroservices.Core.Saga.Exceptions
{
    internal class SagaUnprocessableException : BusinessException
    {
        public SagaUnprocessableException(string message) : base(message)
        {
        }
    }
}
