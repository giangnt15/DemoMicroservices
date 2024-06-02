using DemoMicroservices.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMicroservices.Core.Repos
{
    public interface IAggregateRepository<TId, TAggregate> where TAggregate : AggregateRoot<TId>
    {
        Task<TAggregate> LoadAsync(TId id);

        Task SaveAsync(TAggregate aggregate);

        Task<bool> ExistsAsync(TId id);
    }
}
