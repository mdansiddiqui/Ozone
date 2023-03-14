using System;
using System.Collections.Generic;
using System.Text;

namespace Ozone.Application.DTOs.Projects
{
   public class ActivityLogRemarksHistoryModel
    {

        public long Id { get; set; }
        public long? ActivityLogId { get; set; }

        public long? ApprovalStatusId { get; set; }
        public string ApprovalStatusName { get; set; }

        public string Remarks { get; set; }

        public long? RemarksById { get; set; }

        public string RemarksByName { get; set; }
        public DateTime? RemarksDate { get; set; }
    }
}
