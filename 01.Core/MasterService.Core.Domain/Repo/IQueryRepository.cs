using MasterService.Core.Domain.QueryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterService.Core.Domain.Repo
{
    public interface IQueryRepository
    {
        Task<List<object>> Select(GenericSummary Request);
    }
}
