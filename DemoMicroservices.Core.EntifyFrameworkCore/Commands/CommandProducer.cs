using DemoMicroservices.Core.Commands;
using DemoMicroservices.Core.Commands.Producer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMicroservices.Core.EntifyFrameworkCore.Commands
{
    public class CommandProducer<TDbContext> : ICommandProducer where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;

        public CommandProducer(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public Task<Guid> SendAsync<TCommand>(CommandEnvelop envelop) where TCommand : ICommand
        {
            throw new NotImplementedException();
        }

        public Task<List<Guid>> SendAsync<TCommand>(IEnumerable<CommandEnvelop> envelops) where TCommand : ICommand
        {
            throw new NotImplementedException();
        }

        public Task<Guid> SendAsync<TCommand>(string channel, TCommand command, string replyChannel, Dictionary<string, string> headers) where TCommand : ICommand
        {
            throw new NotImplementedException();
        }
    }
}
