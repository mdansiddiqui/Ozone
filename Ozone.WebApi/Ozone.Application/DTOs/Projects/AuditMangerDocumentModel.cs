using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
namespace Ozone.Application.DTOs
{
   public class AuditMangerDocumentModel
    {
        public long Id { get; set; }
        public long? ClientAuditVisitId { get; set; }
        public long? AuditDocumentTypeId { get; set; }
        public string AuditDocumentTypename { get; set; }
        public string DocumentPath { get; set; }
        public string DocumentContentType { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? LastUpdatedById { get; set; }

        public DateTime? LastUpdateDate { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? Deactive { get; set; }

        public IFormFile File { get; set; }
    }
}
