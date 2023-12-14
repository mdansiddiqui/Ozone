using System;
using System.Collections.Generic;
using System.Text;

namespace Ozone.Application.DTOs.Projects
{
   public class sp_ImpartialityReviewReportModel
    {
        public long? S_No { get; set; }

        public string ProjectNo { get; set; }

        public string ClientName { get; set; }

        public string Consultant { get; set; }
        public string ConsultancyName { get; set; }

        public string ContarctReview { get; set; }
        public string AuditTeam { get; set; }

        public string CertificateDecision { get; set; }
        public string PaymentRecived { get; set; }
        public DateTime? EndDate { get; set; }

    }

    public class GetPagedImpartialityReviewReportModel
    {
        public int TotalCount { get; set; }
        public List<sp_ImpartialityReviewReportModel> sp_ImpartialityReviewReportModel { get; set; }

    }
}
