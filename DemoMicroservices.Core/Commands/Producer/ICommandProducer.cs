using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMicroservices.Core.Commands.Producer
{
    public interface ICommandProducer
    {
        Task<Guid> SendAsync<TCommand>(CommandEnvelop envelop) where TCommand : ICommand;
        Task<List<Guid>> SendAsync<TCommand>(IEnumerable<CommandEnvelop> envelops) where TCommand : ICommand;
        Task<Guid> SendAsync<TCommand>(string channel, TCommand command, string replyChannel, Dictionary<string, string> headers) where TCommand : ICommand;
    }
}
