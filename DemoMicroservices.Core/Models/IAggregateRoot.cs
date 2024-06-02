using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMicroservices.Core.Models
{
    public interface IAggregateRoot<TId>
    {
        public TId Id { get; set; }
        public int Version { get; set; }

    }
}
