using System;
using System.Collections.Generic;
using System.Text;

namespace Ozone.Application.DTOs
{
   public class RiskModel
    {
        public long Id { get; set; }
      
        public string Code { get; set; }
       
        public string Name { get; set; }
      
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public long? CreatedById { get; set; }
        
        public DateTime? CreatedDate { get; set; }
        public long? LastModifiedById { get; set; }
        public string RiskLevelName { get; set; }

        public DateTime? LastModifiedDate { get; set; }
       // //public long? EaCodeId { get; set; }
       // //public string EaCodeName { get; set; }
       // public long? NaceCodeId { get; set; }
       //public string NacecodeName { get; set; }

    }
}
