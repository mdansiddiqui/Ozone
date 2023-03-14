using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Ozone.Infrastructure.Persistence.Models
{
    public partial class ProjectGeneralForm
    {
        [Key]
        public long Id { get; set; }
        [StringLength(50)]
        public string Code { get; set; }
        public bool? IsDeleted { get; set; }
        public long? CreatedById { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        public long? LastModifiedById { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastModifiedDate { get; set; }
        public long? ApprovedById { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ApprovedDate { get; set; }
        public long? RequestOfSiteId { get; set; }
        public long? ClientProjectId { get; set; }
        public int? TotalEmployees { get; set; }
        public string Address { get; set; }
        public long? CityId { get; set; }
        public long? ServicesId { get; set; }
        public long? AuditTypeId { get; set; }
        public long? SmetaAuditId { get; set; }
        public string ApplicationFormPath { get; set; }
        public string ApplicationContentType { get; set; }
        public string Remarks { get; set; }
        public string SiteFactory { get; set; }
        public string SiteProductionProcess { get; set; }
        public string ProductCategory { get; set; }
        [Column("NTNNumber")]
        [StringLength(50)]
        public string Ntnnumber { get; set; }
        public long? StandardId { get; set; }

        [ForeignKey(nameof(ApprovedById))]
        [InverseProperty(nameof(SecUser.ProjectGeneralFormApprovedBy))]
        public virtual SecUser ApprovedBy { get; set; }
        [ForeignKey(nameof(ClientProjectId))]
        [InverseProperty(nameof(ClientProjects.ProjectGeneralForm))]
        public virtual ClientProjects ClientProject { get; set; }
        [ForeignKey(nameof(CreatedById))]
        [InverseProperty(nameof(SecUser.ProjectGeneralFormCreatedBy))]
        public virtual SecUser CreatedBy { get; set; }
        [ForeignKey(nameof(LastModifiedById))]
        [InverseProperty(nameof(SecUser.ProjectGeneralFormLastModifiedBy))]
        public virtual SecUser LastModifiedBy { get; set; }
        [ForeignKey(nameof(RequestOfSiteId))]
        [InverseProperty("ProjectGeneralForm")]
        public virtual RequestOfSite RequestOfSite { get; set; }
        [ForeignKey(nameof(StandardId))]
        [InverseProperty(nameof(Certification.ProjectGeneralForm))]
        public virtual Certification Standard { get; set; }
    }
}
