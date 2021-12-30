using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterService.Core.Domain.QueryModel
{
    public class ServicesSummary
    {
        public int ExpireTokenByHour { get; set; }
        public string ConnectionString { get; set; }
        public string ServiceName { get; set; }
        public string SpName { get; set; }
        public bool HaveLogin { get; set; }
        public List<ServicesLoginSummary> Login { get; set; }
        public List<ServicesSpInputsSummary> Inputs { get; set; }
    }
}
