using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Ozone.Infrastructure.Persistence.Models
{
    public partial class ActivityLogRemarksHistory
    {
        [Key]
        public long Id { get; set; }
        public long? ActivityLogId { get; set; }
        public long? ApprovalStatusId { get; set; }
        public string Remarks { get; set; }
        public long? RemarksById { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? RemarksDate { get; set; }

        [ForeignKey(nameof(ActivityLogId))]
        [InverseProperty("ActivityLogRemarksHistory")]
        public virtual ActivityLog ActivityLog { get; set; }
        [ForeignKey(nameof(ApprovalStatusId))]
        [InverseProperty("ActivityLogRemarksHistory")]
        public virtual ApprovalStatus ApprovalStatus { get; set; }
        [ForeignKey(nameof(RemarksById))]
        [InverseProperty(nameof(SecUser.ActivityLogRemarksHistory))]
        public virtual SecUser RemarksBy { get; set; }
    }
}
