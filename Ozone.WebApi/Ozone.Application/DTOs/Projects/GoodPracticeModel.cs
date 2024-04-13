using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ozone.Application.DTOs.Projects
{
    public class GoodPracticeModel
    {
        public long Id { get; set; }
        public long? DescriptionOfGoodPractice { get; set; }
        public string ObjectiveEvidence { get; set; }
        public string Evidence { get; set; }
        public long CreatedById { get; set; }
        public DateTime? CreatedByDate { get; set; }
        public long UpdateById { get; set; }
        public DateTime UpdateByDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
