using Ozone.Application.DTOs;
using Ozone.Application.DTOs.Projects;
using Ozone.Application.DTOs.Security;

using Ozone.Application.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozone.Infrastructure.Shared.Services
{
   public interface IClientAuditVisitService
    {
        Task<string> Create(ClientAuditVisitModel input);
        Task<GetPagedClientAuditVisitModel> GetPagedClientAuditVisitResponse(long ProjectId, PagedResponseModel model);
        Task<ClientAuditVisitModel> GetClientAuditVisitBYId(long id);
        Task<List<ClientAuditVisitModel>> GetClientAuditVisitByOrganizationId(long id);
        Task<List<ClientAuditVisitModel>> GetClientAuditVisitBySearch(IDictionary<string, string> keyValuePairs);
        Task<string> ClientAuditVisitDeleteById(long id);
        Task<GetPagedClientAuditVisitModel> AuditPlan(long id,PagedResponseModel model);
        Task<ClientAuditVisitModel> DownloadAuditPlan(long id);
        Task<List<MappingDocumentsWithStandardModel>> GetAllRequiredDocumentBYId(long id);

        Task<List<MappingDocumentsWithStandardModel>> GetAllDocumentsTypeWSVLAS(long id);
        //Task<string> AuditPlaneSubmitForReview(long id, long loginUserId);
        // Task<string> AuditComplete(long id);
        Task<string> AuditComplete(ClientAuditVisitModel input);
        Task<GetPagedProjectStatusModel> ProjectStatus(long id , PagedResponseModel model);

        Task<List<AuditReviewerDocumentModel>> GetAllAuditReviewerDocuments(long clientauditvisitId);
        Task<string> AuditReviewerDocumentCreate(AuditReviewerDocumentModel input);

        Task<AuditReviewerDocumentModel> DownloadAuditReviewerDocuments(long id);
        Task<string> AuditReviewerDocumentDeleteById(long id);
        Task<List<MappingDocumentsWithStandardModel>> GetAllDocumentsTypeforReviewer(long id);
        Task<List<MappingDocumentsWithStandardModel>> GetAllManagerDocumentsType(long id);
        Task<List<AuditMangerDocumentModel>> GetAllAuditManagerDocuments(long clientauditvisitId);
        Task<string> AuditManagerDocumentCreate(AuditMangerDocumentModel input);
        Task<string> AuditManagerDocumentDeactiveById(long id);
        Task<AuditMangerDocumentModel> DownloadAudiorManagerDocuments(long id);

        Task<string> ChangeReviewer(ReviewerModel input);

    }
}
