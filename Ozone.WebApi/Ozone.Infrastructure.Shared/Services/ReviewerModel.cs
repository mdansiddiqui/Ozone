using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Ozone.Infrastructure.Shared
{
    public class ReviewerModel
    {
        public long? ReviewerId { get; set; }
        public long? AuditVisitId { get; set; }
        public long? UpdatedById { get; set; }
    }
}
