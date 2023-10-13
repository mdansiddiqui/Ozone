using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Ozone.Infrastructure.Persistence.Models
{
    public partial class AuditManagerDocuments
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
        public bool? Deactive { get; set; }

        [ForeignKey(nameof(AuditDocumentTypeId))]
        [InverseProperty(nameof(AuditDocumentsType.AuditManagerDocuments))]
        public virtual AuditDocumentsType AuditDocumentType { get; set; }
        [ForeignKey(nameof(ClientAuditVisitId))]
        [InverseProperty("AuditManagerDocuments")]
        public virtual ClientAuditVisit ClientAuditVisit { get; set; }
        [ForeignKey(nameof(CreatedById))]
        [InverseProperty(nameof(SecUser.AuditManagerDocumentsCreatedBy))]
        public virtual SecUser CreatedBy { get; set; }
        [ForeignKey(nameof(LastUpdatedById))]
        [InverseProperty(nameof(SecUser.AuditManagerDocumentsLastUpdatedBy))]
        public virtual SecUser LastUpdatedBy { get; set; }
    }
}
