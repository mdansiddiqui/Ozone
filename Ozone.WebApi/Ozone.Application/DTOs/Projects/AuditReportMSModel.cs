﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Ozone.Application.DTOs
{
   public class AuditReportMSModel
    {
        public long Id { get; set; }
        public long? ProjectId { get; set; }
        public string ProjectCode { get; set; }
        public long? ClientAuditVisitId { get; set; }
        public bool? IsDeleted { get; set; }
        public long? CreatedById { get; set; }
      
        public DateTime? CreatedDate { get; set; }
        public long? LastModifiedById { get; set; }
       
        public DateTime? LastModifiedDate { get; set; }
        public long? ApprovalStatusId { get; set; }
        public string ApprovalStatusName { get; set; }
        public long? LeadAuditorId { get; set; }


        public long? Minor { get; set; }
        public long? Major { get; set; }
        public long? Critical { get; set; }
        public long? TimeBound { get; set; }
        public long? Observation { get; set; }
     
    }
}
