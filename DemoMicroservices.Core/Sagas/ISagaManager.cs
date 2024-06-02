using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMicroservices.Core.Sagas
{
    public interface ISagaManager<TData>
    {
        Task CreateAsync(TData data);
        Task ProcessReplyAsync(SagaReplyEnvelop sagaReplyEnvelop);
        void SubscribeToReplyChannel();
    }
}
