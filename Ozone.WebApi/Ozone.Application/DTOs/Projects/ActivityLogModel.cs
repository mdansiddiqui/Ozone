using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ozone.Application.DTOs.Projects
{
   public class ActivityLogModel
    {
        public long Id { get; set; }
        public long UserId { get; set; }
    
        public string UserActionType { get; set; }
        public long FormId { get; set; }
    
        public string TableName { get; set; }
        public long TableRowId { get; set; }
        public string ClientName { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
      
        public string AffectedColumns { get; set; }
        public bool IsClosed { get; set; }
        public long CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedDate { get; set; }
        public long? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    
        public long? ApprovalStatusId { get; set; }
        public string ApprovalStatusName { get; set; }
        public long? OrganizationId { get; set; }

        public string OrganizationName { get; set; }

        public string Remarks { get; set; }

        public string FormName { get; set; }
        public IFormFile File { get; set; }

        public string FilePath { get; set; }
        public string ContentType { get; set; }


    }
}
