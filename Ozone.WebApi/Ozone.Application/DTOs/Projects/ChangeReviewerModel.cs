using System;
using System.Collections.Generic;
using System.Text;

namespace Ozone.Application.DTOs
{
    public class ChangeReviewerModel
    {
       
            public long? ReviewerId { get; set; }
            public long? AuditVisitId { get; set; }
            public long? UpdatedById { get; set; }

        
    }
}
