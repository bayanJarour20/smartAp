﻿using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Service.BoundedContext;
using SmartStart.DataTransferObject.GeneralDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.Main.GeneralServices
{
    public interface IGeneralRepository : IElRepository
    {
        Task<OperationResult<object>> GetRemaining(Guid UserId);
        Task<OperationResult<object>> SetSelected(SelectedDto selectedDto, Guid UserId);
        Task<OperationResult<bool>> RemoveSelected(SelectedDto selectedDto, Guid UserId);
        Task<OperationResult<object>> GetSelected(Guid UserId);
        Task<OperationResult<object>> ActivateCode(string Code, Guid UserId);
    }
}
