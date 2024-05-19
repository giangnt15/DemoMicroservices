using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMicroservices.Core.Events
{
    public class DomainEvent : IVersionedEvent
    {
        public Guid Id { get; set; }
        public Guid SourceId { get; set; }
        public int Version { get; set; }
    }
}
