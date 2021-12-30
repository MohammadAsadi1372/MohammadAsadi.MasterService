
using MasterService.Core.Domain.QueryModel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterService.Core.ApplicationService.Query
{
    public class FetchQuery : IRequest<List<object>>, IGenericSummary
    {
        public string SystemName { get; set; }
        public Dictionary<string, object> Request { get; set; }
    }
}
