using System;
using System.Collections.Generic;
using System.Text;

namespace Ozone.Application.DTOs.Reports
{
   public class CertifiedClientModel
    {
        public long S_No { get; set; }
        public long? ProjectId{ get; set; }
    public string ProjectCode { get; set; }
        public string ClientName { get; set; }
        public string Country { get; set; }
        public string CertificationStatus { get; set; }
        public string currentState { get; set; }
        public string Registration_no { get; set; }
        public DateTime? CertificationIssueDate { get; set; }
        public DateTime? CertificationExpiryDate { get; set; }
        public string Surveillance_Frequency { get; set; }
        public DateTime? IntimationdateSurv_1 { get; set; }
        public DateTime? Windowperiod_start_Surv_1 { get; set; }
        public DateTime? Windowperiod_end_Surv_1 { get; set; }
        public DateTime? Surv_1_due { get; set; }
        public DateTime? Windowperiod_Start_FUP_1 { get; set; }
        public DateTime? Windowperiod_end_FUP_1 { get; set; }
        public DateTime? IntimationdateSurv_2 { get; set; }
        public DateTime? Windowperiod_start_Surv_2 { get; set; }
        public DateTime? Windowperiod_end_Surv_2 { get; set; }
        public DateTime? Surv_2_due { get; set; }
        public DateTime? Windowperiod_Sart_FUP_2 { get; set; }
        public DateTime? Windowperiod_end_FUP_2 { get; set; }
       
        public DateTime? RecertificationIntimationdate { get; set; }
        public DateTime? Recertification_Windowperiod_start_Surv_2 { get; set; }
        public DateTime? Recertification_Windowperiod_end_Surv_2 { get; set; }
        public DateTime? Followup_Recert_Start { get; set; }
        public DateTime? Followup_Recert_end { get; set; }
        public string CycleCode { get; set; }
    }
    public class WindowperiodCreateModel {
        public long Id { get; set; }
        public string Type { get; set; }
        public long CreatedById { get; set; }
      
            }
}

