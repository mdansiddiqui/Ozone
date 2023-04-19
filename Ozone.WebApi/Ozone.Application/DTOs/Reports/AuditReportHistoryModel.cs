using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ozone.Application.DTOs.Reports
{
    public class AuditReportHistoryModel
    {
        public long Id { get; set; }
        public long? AuditReportId { get; set; }
        public long? ApprovalStatusId { get; set; }
        public string Remarks { get; set; }
        public long? RemarksById { get; set; }
        public DateTime? RemarksDate { get; set; }
        public bool? IsDeleted { get; set; }
        public long? ClientAuditVisitId { get; set; }
        public string ApprovalStatusName { get; set; }
        public string RemarksByName { get; set; }

    }
}
