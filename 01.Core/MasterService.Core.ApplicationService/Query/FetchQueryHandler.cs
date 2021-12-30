using MasterService.Core.Domain.QueryModel;
using MasterService.Core.Domain.Repo;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterService.Core.ApplicationService.Query
{
    public class FetchQueryHandler : IRequestHandler<FetchQuery, List<object>>
    {
        private readonly IQueryRepository _repository;

        public FetchQueryHandler(IQueryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<object>> Handle(FetchQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.Select(new GenericSummary { Request = request.Request, SystemName = request.SystemName });
            return result;
        }
    }
}
