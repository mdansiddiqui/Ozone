﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Ozone.Infrastructure.Persistence.Models
{
    public partial class ActivityLog
    {
        public ActivityLog()
        {
            ActivityLogRemarksHistory = new HashSet<ActivityLogRemarksHistory>();
        }

        [Key]
        public long Id { get; set; }
        public long UserId { get; set; }
        [StringLength(2)]
        public string UserActionType { get; set; }
        public long FormId { get; set; }
        [StringLength(25)]
        public string TableName { get; set; }
        public long TableRowId { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        [StringLength(1000)]
        public string AffectedColumns { get; set; }
        public bool IsClosed { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        [Column("ApprovalStatusID")]
        public long? ApprovalStatusId { get; set; }
        public long? OrganizationId { get; set; }
        public string FilePath { get; set; }
        public string ContentType { get; set; }

        [ForeignKey(nameof(ApprovalStatusId))]
        [InverseProperty("ActivityLog")]
        public virtual ApprovalStatus ApprovalStatus { get; set; }
        [ForeignKey(nameof(FormId))]
        [InverseProperty(nameof(SecForm.ActivityLog))]
        public virtual SecForm Form { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        [InverseProperty("ActivityLog")]
        public virtual Organization Organization { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(SecUser.ActivityLog))]
        public virtual SecUser User { get; set; }
        [InverseProperty("ActivityLog")]
        public virtual ICollection<ActivityLogRemarksHistory> ActivityLogRemarksHistory { get; set; }
    }
}
