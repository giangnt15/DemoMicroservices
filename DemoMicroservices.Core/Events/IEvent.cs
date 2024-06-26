﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMicroservices.Core.Events
{
    public interface IEvent
    {
        public Guid Id { get; set; }
        public Guid SourceId { get; set; }
        public Guid Trigger {  get; set; }
    }
}
