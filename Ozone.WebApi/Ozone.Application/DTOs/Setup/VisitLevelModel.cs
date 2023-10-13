﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Ozone.Application.DTOs
{
   public class VisitLevelModel
    {
        public long Id { get; set; }

        public string Code { get; set; }
   
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public long? CreatedById { get; set; }
 
        public DateTime? CreatedDate { get; set; }
        public long? LastUpdatedById { get; set; }
  
        public DateTime? LastUpdatedDate { get; set; }
        public long? StandardId { get; set; }
        public string StandardName { get; set; }
    }
}
