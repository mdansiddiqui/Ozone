using Ozone.Application.DTOs;
using Ozone.Application.DTOs.Projects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ozone.Application.Interfaces.Setup
{
    public interface ISubClause
    {
        Task<string> Create(SubClauseModel input);

        Task<GetPagedSubClauseModel> GetPagedSubClause(PagedResponseModel model);

        Task<string> SubClauseDeleteById(long id);


        Task<List<MainClauseModel>> GetAllClauseByStandard(long countryId);
        Task<List<MainClauseModel>> GetAllMainClaus();


    }
}
