using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Service.BoundedContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.Shared.HomeService
{
    public interface IHomeRepository : IElRepository
    {
        Task<OperationResult<object>> Home();
    }
}
