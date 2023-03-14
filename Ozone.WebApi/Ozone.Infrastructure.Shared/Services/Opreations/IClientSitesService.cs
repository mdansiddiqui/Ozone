using Ozone.Application.DTOs;
using Ozone.Application.DTOs.Projects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ozone.Infrastructure.Shared.Services
{
   public interface IClientSitesService
    {
        Task<string> CreateClientSites(ClientSitesModel input);

        Task<string> ApprovedClientChange(ActivityLogModel input);

        Task<string> ApprovedClientSitesChange(ActivityLogModel input);


        Task<GetPagedClientSitesModel> GetPagedClientSites(PagedResponseModel model);
        Task<GetPagedClientProjectsModel> GetPagedClientProjects(long id,PagedResponseModel model);


        Task<ClientSitesModel> GetClientSitesBYId(long id);

        Task<GetPagedClientProjectsModel> GetPagedAllProjects(long id, PagedResponseModel model);

        Task<GetPagedClientProjectsModel> GetPagedAllClientChange(long id, PagedResponseModel model);
        Task<GetPagedClientProjectsModel> GetAllClientSitesChangeRequest(long id, PagedResponseModel model);

        Task<List<ActivityLogModel>> GetClientChangeDataById(long id, PagedResponseModel model);

        Task<List<ActivityLogModel>> GetClientSitesChangeDataById(long id, PagedResponseModel model);

        Task<List<ActivityLogRemarksHistoryModel>> GetClientChangeRemarksDataById(long id);

        Task<List<ActivityLogRemarksHistoryModel>> GetClientSitesChangeRemarksDataById(long id);

        Task<GetPagedClientProjectsModel> GetPagedAllAudits(long id, PagedResponseModel model);
        Task<string> ClientSitesDeleteById(long id);

        Task<ProjectFormPathModel> GetProjectUrlBYId(long id);

 




    }
}
