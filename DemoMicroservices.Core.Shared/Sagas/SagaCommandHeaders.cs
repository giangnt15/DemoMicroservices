using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMicroservices.Core.Shared.Sagas
{
    public static class SagaCommandHeaders
    {
        public const string SAGA_TYPE = "SagaType";
        public const string SAGA_ID = "SagaId";
    }
}
