﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMicroservices.Core.Saga
{
    public interface ISagaInstanceRepository
    {
        Task<SagaInstance> FindAsync(string sagaType, string sagaId);
        Task SaveAsync(SagaInstance sagaInstance);
    }
}
