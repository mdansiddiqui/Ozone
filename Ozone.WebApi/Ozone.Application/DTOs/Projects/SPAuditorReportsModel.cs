using System;
using System.Collections.Generic;
using System.Text;

namespace Ozone.Application.DTOs.Projects
{
   public class SPAuditorReportsModel 
    {
        public long? Id { get; set; }
        public long? LeadAuditorId { get; set; }
        public string LeadAuditorName { get; set; }
        public decimal? NoOfAudits { get; set; }
        public decimal? NCS { get; set; }
        public decimal? AVGNCS { get; set; }
        public decimal? AvgTAT { get; set; }
        public decimal? AvgTAT_1 { get; set; }
        public decimal? AvgTAT_2 { get; set; }
        public decimal? AvgTAT_3 { get; set; }
        public decimal? AvgTAT_4 { get; set; }
        public decimal? AvgTAT_5 { get; set; }
        public string ProjectCode { get; set; }
        public string VisitLevel { get; set; }
        //public long? ProjectId { get; set; }
        //public DateTime? ResubmitedDate { get; set; }
        //public DateTime? CompletedDate { get; set; }
        public long? totalDays { get; set; }
    }
    public class SPAuditorDetailModel
    {
        public long? Id { get; set; }
    public long? LeadAuditorId { get; set; }
    public string LeadAuditorName { get; set; }
    public string ClientName { get; set; }

        public decimal? NoOfAudits { get; set; }
    public decimal? NCS { get; set; }
    public decimal? AVGNCS { get; set; }
    public decimal? AvgTAT { get; set; }
    public decimal? AvgTAT_1 { get; set; }
    public decimal? AvgTAT_2 { get; set; }
    public decimal? AvgTAT_3 { get; set; }
    public decimal? AvgTAT_4 { get; set; }
    public decimal? AvgTAT_5 { get; set; }
    public string ProjectCode { get; set; }
    public string VisitLevel { get; set; }
    //public long? ProjectId { get; set; }
    //public DateTime? ResubmitedDate { get; set; }
    //public DateTime? CompletedDate { get; set; }
    public long? totalDays { get; set; }
}
}
