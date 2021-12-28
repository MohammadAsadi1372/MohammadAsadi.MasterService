using Dapper;
using MasterService.Core.Domain.Repo;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterService.Infra.Data.Sql
{
    public class QueryRepository : IQueryRepository
    {
        private readonly IConfiguration config;

        public QueryRepository(IConfiguration config)
        {
            this.config = config;
        }

        public async Task<List<object>> Select(Dictionary<string,object> keyValues)
        {
            await using var dbConnection = new SqlConnection(config.GetConnectionString("ConnectionString"));

            var result = (List<object>)await dbConnection.QueryAsync<object>($@"
            EXECUTE proc_Jetprint_Select;");
            return result;
        }
    }
}
