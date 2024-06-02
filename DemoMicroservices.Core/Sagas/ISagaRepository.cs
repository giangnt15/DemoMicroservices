using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMicroservices.Core.Sagas
{
    public interface ISagaRepository
    {
        Task<SagaInstance> LoadAsync(string sagaId, string sagaType);
        Task SaveAsync(SagaInstance saga);
        Task<bool> ExistAsync(string sagaId, string sagaType);
    }
}
