using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Ozone.Infrastructure.Persistence.Models
{
    [Table("Good_Practices")]
    public partial class GoodPractices
    {
        [Key]
        public long Id { get; set; }
        public long? DescriptionOfGoodPractice { get; set; }
        [StringLength(255)]
        public string ObjectiveEvidence { get; set; }
        public string Evidence { get; set; }
        public long CreatedById { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedByDate { get; set; }
        public long UpdateById { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UpdateByDate { get; set; }
        public bool? IsDeleted { get; set; }

        [ForeignKey(nameof(DescriptionOfGoodPractice))]
        [InverseProperty(nameof(MainClause.GoodPractices))]
        public virtual MainClause DescriptionOfGoodPracticeNavigation { get; set; }
    }
}
