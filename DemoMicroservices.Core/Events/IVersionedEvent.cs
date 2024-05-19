using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMicroservices.Core.Events
{
    public interface IVersionedEvent : IEvent
    {
        public int Version { get; set; }
    }
}
