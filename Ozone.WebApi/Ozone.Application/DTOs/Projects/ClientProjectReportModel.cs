using System;
using System.Collections.Generic;
using System.Text;

namespace Ozone.Application.DTOs.Projects
{
   public class ClientProjectReportModel
    {

        public long? S_No { get; set; }

        public string StatusCode { get; set; }

        public string Registration_no { get; set; }
    
        public string ClientName { get; set; }
        public string Address { get; set; }

        public string Scope { get; set; }
        public string NaceCode { get; set; }
  
        public string CountryName { get; set; }

    }

    public class GetPagedClientProjectReportsModel
    {
        public int TotalCount { get; set; }
        public List<ClientProjectReportModel> ClientProjectReportModel { get; set; }
        
    }
}
