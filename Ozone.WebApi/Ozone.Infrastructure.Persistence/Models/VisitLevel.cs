using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Ozone.Infrastructure.Persistence.Models
{
    public partial class VisitLevel
    {
        public VisitLevel()
        {
            ClientAuditVisit = new HashSet<ClientAuditVisit>();
            MappingDocumentsWithStandard = new HashSet<MappingDocumentsWithStandard>();
        }

        [Key]
        public long Id { get; set; }
        [StringLength(50)]
        public string Code { get; set; }
        [StringLength(200)]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public long? CreatedById { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        public long? LastUpdatedById { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastUpdatedDate { get; set; }
        public long? StandardId { get; set; }

        [ForeignKey(nameof(CreatedById))]
        [InverseProperty(nameof(SecUser.VisitLevelCreatedBy))]
        public virtual SecUser CreatedBy { get; set; }
        [ForeignKey(nameof(LastUpdatedById))]
        [InverseProperty(nameof(SecUser.VisitLevelLastUpdatedBy))]
        public virtual SecUser LastUpdatedBy { get; set; }
        [ForeignKey(nameof(StandardId))]
        [InverseProperty(nameof(Certification.VisitLevel))]
        public virtual Certification Standard { get; set; }
        [InverseProperty("VisitLevel")]
        public virtual ICollection<ClientAuditVisit> ClientAuditVisit { get; set; }
        [InverseProperty("VisitLevel")]
        public virtual ICollection<MappingDocumentsWithStandard> MappingDocumentsWithStandard { get; set; }
    }
}
