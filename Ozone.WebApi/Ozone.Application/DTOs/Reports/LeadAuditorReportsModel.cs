using System;
using System.Collections.Generic;
using System.Text;

namespace Ozone.Application.DTOs
{
    public class LeadAuditorReportsModel
    {
        public long? Id { get; set; }
        public long? LeadAuditorId { get; set; }
        public string LeadAuditorName { get; set; }
        public decimal? NoOfAudits { get; set; }
        public decimal?  NoOfNCs { get; set; }
        public decimal? AvgNoOfNCs { get; set; }
        public decimal? AvgTAT { get; set; }
    
    }
    public class GetPagedLeadAuditorReportsModel
    {
        public int TotalCount { get; set; }
        public List<LeadAuditorReportsModel> LeadAuditorReportsModel { get; set; }
    }
}
