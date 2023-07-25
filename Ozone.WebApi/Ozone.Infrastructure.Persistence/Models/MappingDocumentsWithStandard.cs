using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Ozone.Infrastructure.Persistence.Models
{
    [Table("Mapping_Documents_With_Standard")]
    public partial class MappingDocumentsWithStandard
    {
        [Key]
        public long Id { get; set; }
        public long? StandardId { get; set; }
        public long? VisitLevelId { get; set; }
        public long? DocumentTypeId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsRequired { get; set; }
        public bool? DocumentForReviewer { get; set; }
        public int? DocumentAssignId { get; set; }

        [ForeignKey(nameof(DocumentAssignId))]
        [InverseProperty(nameof(DocumentAssignTo.MappingDocumentsWithStandard))]
        public virtual DocumentAssignTo DocumentAssign { get; set; }
        [ForeignKey(nameof(DocumentTypeId))]
        [InverseProperty(nameof(AuditDocumentsType.MappingDocumentsWithStandard))]
        public virtual AuditDocumentsType DocumentType { get; set; }
        [ForeignKey(nameof(StandardId))]
        [InverseProperty(nameof(Certification.MappingDocumentsWithStandard))]
        public virtual Certification Standard { get; set; }
        [ForeignKey(nameof(VisitLevelId))]
        [InverseProperty("MappingDocumentsWithStandard")]
        public virtual VisitLevel VisitLevel { get; set; }
    }
}
