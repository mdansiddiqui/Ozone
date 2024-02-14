using Ozone.Application.DTOs;
using Ozone.Application.DTOs.Projects;
using Ozone.Application.DTOs.Security;
using Ozone.Application.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozone.Application.Interfaces
{
    public interface IMainClause
    {
        Task<string> Create(MainClauseModel input);

        Task<GetPagedMainClauseModel> GetPagedMainClause(PagedResponseModel model);
        Task<string> MainClauseDeleteById(long id); 
    }
}
