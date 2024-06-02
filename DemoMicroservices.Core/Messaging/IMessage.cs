﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMicroservices.Core.Messaging
{
    public interface IMessage
    {
        public Guid Id { get; set; }
        string ToJson();
    }
}
