using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Ozone.Infrastructure.Persistence.Models
{
    public partial class MainClause
    {
        public MainClause()
        {
          //  GoodPractices = new HashSet<GoodPractices>();
            SubClause = new HashSet<SubClause>();
        }

        [Key]
        public long Id { get; set; }
        public long StandardId { get; set; }
        [Required]
        [StringLength(50)]
        public string MainClauseName { get; set; }
        public string Heading { get; set; }
        public string Requirement { get; set; }
        public long CreatedById { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CretedByDate { get; set; }
        public long UpdateById { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdateByDate { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey(nameof(StandardId))]
        [InverseProperty(nameof(Certification.MainClause))]
        public virtual Certification Standard { get; set; }
        [InverseProperty("DescriptionOfGoodPracticeNavigation")]
        public virtual ICollection<GoodPractices> GoodPractices { get; set; }
        [InverseProperty("MainClause")]
        public virtual ICollection<SubClause> SubClause { get; set; }

    }
}
