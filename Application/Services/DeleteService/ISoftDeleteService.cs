using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.DeleteService
{
    public interface ISoftDeleteService
    {
        Task<(bool Success, string Message)> SoftDeleteAsync<T>(long id, CancellationToken cancellationToken = default) where T : class;
        Task<bool> IsPrimaryKeyReferencedAsync<T>(long id, CancellationToken cancellationToken = default) where T : class;

    }
}
