﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Ozone.Infrastructure.Persistence.Models
{
    public partial class NaceCode
    {
        public NaceCode()
        {
            Client = new HashSet<Client>();
            ProjectSa8000 = new HashSet<ProjectSa8000>();
            UserAudit = new HashSet<UserAudit>();
            UserAuditorNace = new HashSet<UserAuditorNace>();
            UserConsultancy = new HashSet<UserConsultancy>();
        }

        [Key]
        public long Id { get; set; }
        [StringLength(50)]
        public string Code { get; set; }
        [StringLength(200)]
        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public long? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreeatedDate { get; set; }
        public string Description { get; set; }
        public long? EaCodeId { get; set; }
        public long? RisklevelId { get; set; }

        [ForeignKey(nameof(EaCodeId))]
        [InverseProperty(nameof(Eacode.NaceCode))]
        public virtual Eacode EaCode { get; set; }
        [ForeignKey(nameof(RisklevelId))]
        [InverseProperty(nameof(Risk.NaceCode))]
        public virtual Risk Risklevel { get; set; }
        [InverseProperty("NaceCode")]
        public virtual ICollection<Client> Client { get; set; }
        [InverseProperty("NaceCode")]
        public virtual ICollection<ProjectSa8000> ProjectSa8000 { get; set; }
        [InverseProperty("NaceCode")]
        public virtual ICollection<UserAudit> UserAudit { get; set; }
        [InverseProperty("NaceCode")]
        public virtual ICollection<UserAuditorNace> UserAuditorNace { get; set; }
        [InverseProperty("NaceCode")]
        public virtual ICollection<UserConsultancy> UserConsultancy { get; set; }
    }
}
