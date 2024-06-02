using DemoMicroservices.Core.Commands;
using DemoMicroservices.Core.Commands.Producer;
using DemoMicroservices.Core.Shared.Commands;
using DemoMicroservices.Core.Shared.Sagas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMicroservices.Core.Sagas
{
    public class SagaCommandProducer : ISagaCommandProducer
    {
        private readonly ICommandProducer _commandProducer;
        public SagaCommandProducer(ICommandProducer commandProducer)
        {
            _commandProducer = commandProducer;
        }

        public async Task<Guid> SendAsync(string channel, string sagaType, string sagaId, List<ICommand> commands, string replyChannel)
        {
            Guid messageId = Guid.Empty;
            foreach (var command in commands)
            {
                var headers = new Dictionary<string, string>()
                {
                    { SagaCommandHeaders.SAGA_ID, sagaId },
                    { SagaCommandHeaders.SAGA_TYPE, sagaType },
                    { CommandHeaders.COMMAND_TYPE, command.GetType().AssemblyQualifiedName }
                };
                messageId = await _commandProducer.SendAsync(channel, command, replyChannel, headers);
            }
            return messageId;
        }
    }
}
