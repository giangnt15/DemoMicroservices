using DemoMicroservices.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMicroservices.Core.Sagas
{
    public interface ISaga<TData>
    {
        public ISagaDefinition<TData> Definition { get; }
        string GetSagaType();
        string GetReplyChannel();
    }
}
