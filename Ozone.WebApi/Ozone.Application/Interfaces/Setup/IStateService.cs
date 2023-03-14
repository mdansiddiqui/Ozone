using Ozone.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ozone.Application.Interfaces.Setup
{
   public interface IStateService
    {
        Task<string> Create(StateModel input);
        Task<GetPagedStateModel> GetPagedStateResponse(PagedResponseModel model);
        Task<StateModel> GetStateBYId(long id);
        Task<string> StateDeleteById(long id);

    }
}
