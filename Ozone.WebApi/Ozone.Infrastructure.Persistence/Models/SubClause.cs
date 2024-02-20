using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Ozone.Infrastructure.Persistence.Models
{
    public partial class SubClause
    {
        [Key]
        public long Id { get; set; }
        public long StandardId { get; set; }
        public long MainClauseId { get; set; }
        public string SubClauseName { get; set; }
        public string Requirement { get; set; }
        public long CreatedById { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CretedByDate { get; set; }
        public long LastModifiedById { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey(nameof(MainClauseId))]
        [InverseProperty("SubClause")]
        public virtual MainClause MainClause { get; set; }
        [ForeignKey(nameof(StandardId))]
        [InverseProperty(nameof(Certification.SubClause))]
        public virtual Certification Standard { get; set; }
    }
}
