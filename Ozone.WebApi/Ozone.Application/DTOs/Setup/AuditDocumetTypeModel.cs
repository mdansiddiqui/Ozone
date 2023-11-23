using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ozone.Application.DTOs.Setup
{
    public class AuditDocumetTypeModel
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
  
        public DateTime? LastModifieddate { get; set; }


    }
}
