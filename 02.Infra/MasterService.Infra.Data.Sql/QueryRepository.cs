using Dapper;
using MasterService.Core.Domain.QueryModel;
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
        private readonly IConfiguration _config;

        public QueryRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<List<object>> Select(GenericSummary request)
        {
            var Data = _config.GetSection("Services").Get<List<ServicesSummary>>();
            string SpParamaters = "";
            foreach (var item in Data)
                if (item.ServiceName == request.SystemName)
                {
                    await using var dbConnection = new SqlConnection(item.ConnectionString);
                    if (item.Inputs != null && item.Inputs.Count > 0 && request.Request != null && request.Request.Count > 0)
                        SpParamaters = SetDataAndValue(item.Inputs, request.Request);
                    var result = (List<object>)await dbConnection.QueryAsync<object>($@"
                       EXEC {item.SpName} {SpParamaters};");
                    return result;
                }
            return new List<object>();
        }

        public string SetDataAndValue(List<ServicesSpInputsSummary> Inputs, Dictionary<string, object> Data)
        {
            string parameters = "";
            if (Data != null)
                foreach (var item in Data)
                    foreach (var li in Inputs)
                        if (item.Key == li.Name)
                            parameters += "@" + item.Key + " = " + (item.Value == null ? "null" : item.Value) +" ,";

            return parameters.Remove(parameters.Length - 1);
        }
    }
}
