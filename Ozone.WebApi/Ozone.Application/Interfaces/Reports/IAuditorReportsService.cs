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
   public interface IAuditorReportsService
    {
         //Task<GetPagedLeadAuditorReportsModel> GetPagedLeadAuditorReportsModel(IDictionary<string, string> keyValuePairs);
        Task<List<SPAuditorReportsModel>> AuditorReports(IDictionary<string, string> keyValuePairs);


        Task<GetPagedClientProjectReportsModel> GetClientProjectReport(IDictionary<string, string> keyValuePairs);



        Task<GetPagedAuditorListReportModel> GetAuditorListReport(IDictionary<string, string> keyValuePairs);

        Task<GetPagedAuditorScheduleReportModel> GetAuditorScheduleReport(IDictionary<string, string> keyValuePairs);

        Task<GetPagedScheduleOfAuditReportModel> GetScheduleOfAuditReport(IDictionary<string, string> keyValuePairs);

        Task<GetPagedImpartialityReviewReportModel> GetImpartialityReviewReport(IDictionary<string, string> keyValuePairs);

        Task<List<SPAuditorDetailModel>> AuditorReportsDetail(IDictionary<string, string> keyValuePairs);
        
    }
}
