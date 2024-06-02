using DemoMicroservices.Core.Models;
using DemoMicroservices.Core.Repos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace DemoMicroservices.Core.EntifyFrameworkCore.Repos
{
    public class AggregateRepository<TId, TAggregate, TDbContext> : IAggregateRepository<TId, TAggregate> where TAggregate : AggregateRoot<TId> where TDbContext : DbContext
    {
        protected TDbContext DbContext; 

        public AggregateRepository(TDbContext dbContext) { 
            DbContext = dbContext;

        }

        public async Task<bool> ExistsAsync(TId id)
        {
            return await DbContext.Set<TAggregate>().AnyAsync(agg => agg.Id.Equals(id));
        }

        public async Task<TAggregate> LoadAsync(TId id)
        {
            return await DbContext.Set<TAggregate>().FindAsync(id);
        }

        public async Task SaveAsync(TAggregate aggregate)
        {
           await DbContext.Set<TAggregate>().AddAsync(aggregate);
        }
    }
}
