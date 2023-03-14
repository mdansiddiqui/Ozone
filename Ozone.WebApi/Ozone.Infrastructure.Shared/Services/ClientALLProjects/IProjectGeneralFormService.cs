using Ozone.Application.DTOs;
using Ozone.Application.DTOs.Projects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ozone.Infrastructure.Shared.Services.ClientALLProjects
{
   public interface IProjectGeneralFormService
    {
        //Task<string> Create(ProjectGeneralFormModel input);
        Task<string> ContractSubmit(ClientProjectModel input);
        Task<string> ContractApproval(ClientProjectModel input);

        // Task<GetPagedStandardModel> GetPagedStandardResponse(PagedResponseStandardModel model);
        // Task<GetPagedProjectSlcpModel> GetPagedProjectSlcp(PagedResponseModel model);
        // Task<ProjectSA8000CreateModel> GetProjectSA8000BYId(long id);
        Task<string> ProjectGeneralFormDeleteById(long id);
        Task<ProjectGeneralFormModel> DownloadFile(long id);
        Task<GetPagedProjectGeneralFormModelForView> GetProjectGeneralFormBYId(long id);

        Task<string> Approval(ProjectRemarksHistoryModel input);

        Task<string> SubmitForReview(long id, long loginUserId);
        Task<ClientProjectModel> downloadContract(long id);
        Task<List<smetaauditPillarsModel>> GetAllSmetaAuditPillars();

    }
}
