using System;
using System.Collections.Generic;
using System.Text;

namespace Ozone.Application.DTOs.Projects
{
   public class sp_ScheduleOfAuditReportModel
    {
        public long? S_No { get; set; }

        public string Registration_no { get; set; }

        public string ClientName { get; set; }

        public string ProjectCode { get; set; }
        public long? ProjectTypeId { get; set; }

        public string ProjectType { get; set; }
        public string NaceCode { get; set; }

        public string Stage_1 { get; set; }

        public DateTime? Stage_2 { get; set; }

        public long? SurveillanceVisitFrequencyId { get; set; }

        public string Surveillance_Frequency { get; set; }
        public DateTime? Surv_1 { get; set; }

        public DateTime? Followup_1 { get; set; }
        public DateTime? Surv_2 { get; set; }

        public DateTime? Followup_2 { get; set; }

        public DateTime? Surv_3 { get; set; }
        public DateTime? Surv_4 { get; set; }

        public DateTime? Surv_5 { get; set; }


        public DateTime? Recertification { get; set; }
        public DateTime? CertificationIssueDate { get; set; }
        
    }

    public class GetPagedScheduleOfAuditReportModel
    {
        public int TotalCount { get; set; }
        public List<sp_ScheduleOfAuditReportModel> sp_ScheduleOfAuditReportModel { get; set; }

    }
}
