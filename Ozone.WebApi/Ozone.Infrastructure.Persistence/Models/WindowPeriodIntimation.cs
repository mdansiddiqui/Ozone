using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Ozone.Infrastructure.Persistence.Models
{
    public partial class WindowPeriodIntimation
    {
        [Key]
        public long Id { get; set; }
        public long? ProjectId { get; set; }
        [Column("IntimationdateSurv_1")]
        public bool? IntimationdateSurv1 { get; set; }
        [Column("IntimationdateSurv_1_Date", TypeName = "datetime")]
        public DateTime? IntimationdateSurv1Date { get; set; }
        [Column("Windowperiod_start_Surv_1")]
        public bool? WindowperiodStartSurv1 { get; set; }
        [Column("Windowperiod_start_Surv_1_Date", TypeName = "datetime")]
        public DateTime? WindowperiodStartSurv1Date { get; set; }
        [Column("Windowperiod_Start_FUP_1")]
        public bool? WindowperiodStartFup1 { get; set; }
        [Column("Windowperiod_Start_FUP_1_Date", TypeName = "datetime")]
        public DateTime? WindowperiodStartFup1Date { get; set; }
        [Column("IntimationdateSurv_2")]
        public bool? IntimationdateSurv2 { get; set; }
        [Column("IntimationdateSurv_2_Date", TypeName = "datetime")]
        public DateTime? IntimationdateSurv2Date { get; set; }
        [Column("Windowperiod_start_Surv_2")]
        public bool? WindowperiodStartSurv2 { get; set; }
        [Column("Windowperiod_start_Surv_2_Date", TypeName = "datetime")]
        public DateTime? WindowperiodStartSurv2Date { get; set; }
        [Column("Windowperiod_Sart_FUP_2")]
        public bool? WindowperiodSartFup2 { get; set; }
        [Column("Windowperiod_Sart_FUP_2_Date", TypeName = "datetime")]
        public DateTime? WindowperiodSartFup2Date { get; set; }
        [Column("Recertification_Windowperiod_start_Surv_2")]
        public bool? RecertificationWindowperiodStartSurv2 { get; set; }
        [Column("Recertification_Windowperiod_start_Surv_2_Date", TypeName = "datetime")]
        public DateTime? RecertificationWindowperiodStartSurv2Date { get; set; }
        [Column("Followup_Recert_Start")]
        public bool? FollowupRecertStart { get; set; }
        [Column("Followup_Recert_Start_Date", TypeName = "datetime")]
        public DateTime? FollowupRecertStartDate { get; set; }
        public long? CreateById { get; set; }
        public bool? IsDeleted { get; set; }

        [ForeignKey(nameof(CreateById))]
        [InverseProperty(nameof(SecUser.WindowPeriodIntimation))]
        public virtual SecUser CreateBy { get; set; }
        [ForeignKey(nameof(ProjectId))]
        [InverseProperty(nameof(ClientProjects.WindowPeriodIntimation))]
        public virtual ClientProjects Project { get; set; }
    }
}
