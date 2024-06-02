using DemoMicroservices.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMicroservices.Core.Models
{
    public abstract class AggregateRoot<TId> : IAggregateRoot<TId>
    {
        protected AggregateRoot() { 
            Events = new List<IVersionedEvent>();
        }
        public TId Id { get; set; }
        public int Version { get; set; }

        internal protected List<IVersionedEvent> Events { get; set; }

        public void Apply(IVersionedEvent @event)
        {
            ArgumentNullException.ThrowIfNull(nameof(@event));
            @event.Version = Version + 1;
            When(@event);
            EnsureValidState();
            Events.Add(@event);
        }

        protected abstract void When(IVersionedEvent @event);

        protected abstract void EnsureValidState();

        public List<IVersionedEvent> GetEvents()
        {
            return Events?.ToList();
        }
        
        public void ClearEvents()
        {
            Events.Clear();
        }

        public void Load(IEnumerable<IVersionedEvent> events)
        {
            foreach (var @event in events)
            {
                When(@event);
            }
        }
    }
}
