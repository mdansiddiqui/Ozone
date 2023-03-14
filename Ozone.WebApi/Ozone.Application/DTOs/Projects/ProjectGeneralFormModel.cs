using System;
using System.Collections.Generic;
using System.Text;
//using System;
//using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace Ozone.Application.DTOs.Projects
{
    public class ProjectGeneralFormModel
    {
        public long? ClientSiteId { get; set; }
        public long? StandardId { get; set; }
        public long? ClientId { get; set; }
        public long? ProjectTypeId { get; set; }
        public long? VerificationTypeId { get; set; }
        public string Remarks { get; set; }
        public IFormFile ApplicationForm { get; set; }
        public string ApplicationFormPath { get; set; }
        public string ApplicationContentType { get; set; }

        public string SiteFactory { get; set; }
        public string SiteProductionProcess { get; set; }
        public string ProductCategory { get; set; }
        public string Ntnnumber { get; set; }




        public long Id { get; set; }
      
        public string Code { get; set; }
        public bool? IsDeleted { get; set; }
        public long? CreatedById { get; set; }
       
        public DateTime? CreatedDate { get; set; }
        public long? LastModifiedById { get; set; }
        
        public DateTime? LastModifiedDate { get; set; }
        public long? ApprovedById { get; set; }
       
        public DateTime? ApprovedDate { get; set; }
        public long? RequestOfSiteId { get; set; }
        public long? ClientProjectId { get; set; }
        public int? TotalEmployees { get; set; }
        public string Address { get; set; }
        public long? CityId { get; set; }
        public long? ServicesId { get; set; }
        public long? AuditTypeId { get; set; }
        public long? SmetaAuditId { get; set; }

        public long? CertificationId { get; set; }
        //public long? StandardId { get; set; }

    }
}



