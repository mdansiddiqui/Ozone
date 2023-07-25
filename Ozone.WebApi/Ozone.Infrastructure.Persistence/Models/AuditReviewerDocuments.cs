using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Ozone.Infrastructure.Persistence.Models
{
    public partial class AuditReviewerDocuments
    {
        [Key]
        public long Id { get; set; }
        public long? ClientAuditVisitId { get; set; }
        public long? AuditDocumentTypeId { get; set; }
        [Column("documentPath")]
        public string DocumentPath { get; set; }
        [Column("documentContentType")]
        public string DocumentContentType { get; set; }
        public long? CreatedById { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        public long? LastUpdatedById { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastUpdateDate { get; set; }
        public bool? IsDeleted { get; set; }

        [ForeignKey(nameof(AuditDocumentTypeId))]
        [InverseProperty(nameof(AuditDocumentsType.AuditReviewerDocuments))]
        public virtual AuditDocumentsType AuditDocumentType { get; set; }
        [ForeignKey(nameof(ClientAuditVisitId))]
        [InverseProperty("AuditReviewerDocuments")]
        public virtual ClientAuditVisit ClientAuditVisit { get; set; }
        [ForeignKey(nameof(CreatedById))]
        [InverseProperty(nameof(SecUser.AuditReviewerDocumentsCreatedBy))]
        public virtual SecUser CreatedBy { get; set; }
        [ForeignKey(nameof(LastUpdatedById))]
        [InverseProperty(nameof(SecUser.AuditReviewerDocumentsLastUpdatedBy))]
        public virtual SecUser LastUpdatedBy { get; set; }
    }
}
