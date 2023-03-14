using System;
using System.Collections.Generic;
using System.Text;

namespace Ozone.Application.DTOs.Projects
{
   public class sp_AuditorListReportModel
    {
        public long? S_No { get; set; }
        public long? UserId{ get; set; }

        public string AuditorLevel{ get; set; }

        public string FullName { get; set; }

        public string Status { get; set; }
        public string TypeOfEnrollment { get; set; }

        public string Gender { get; set; }
        public string NaceApproved { get; set; }
        public string Country { get; set; }

    }

    public class GetPagedAuditorListReportModel
    {
        public int TotalCount { get; set; }
        public List<sp_AuditorListReportModel> sp_AuditorListReportModel { get; set; }

    }
}
