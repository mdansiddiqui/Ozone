using System;
using System.Collections.Generic;
using System.Text;

namespace Ozone.Application.DTOs.Projects
{
   public class sp_AuditorScheduleReportModel
    {
        public long? S_No { get; set; }

        public string AuditorName { get; set; }

        public string AuditorGrade { get; set; }

        public string JoiningDate { get; set; }
        public string LastValidationDate { get; set; }

        public string NextDueDate { get; set; }
     
    }

    public class GetPagedAuditorScheduleReportModel
    {
        public int TotalCount { get; set; }
        public List<sp_AuditorScheduleReportModel> sp_AuditorScheduleReportModel { get; set; }

    }
}
