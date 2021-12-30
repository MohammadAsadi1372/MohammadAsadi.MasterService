using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterService.Core.Domain.QueryModel
{
    public class GenericSummary
    {
        public string SystemName { get; set; }
        public Dictionary<string, object> Request { get; set; }
    }
}
