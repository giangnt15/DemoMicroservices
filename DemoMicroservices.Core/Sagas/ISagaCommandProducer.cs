using DemoMicroservices.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMicroservices.Core.Sagas
{
    public interface ISagaCommandProducer
    {
        Task<Guid> SendAsync(string channel, string sagaType, string sagaId, List<ICommand> commands, string replyChannel);
    }
}
