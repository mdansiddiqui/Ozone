using Ozone.Infrastructure.Persistence.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ozone.Application.DTOs
{ 
    public class MainClauseModel
    {
        public long Id { get; set; }
        public long StandardId { get; set; }
        public string StandardName { get; set; }
   
        public string MainClauseName { get; set; }
        public string Heading { get; set; }
        public string Requirement { get; set; }
        public long? CreatedById { get; set; }

        public DateTime? CretedByDate { get; set; }
        public long? UpdateById { get; set; }

        public DateTime? UpdateByDate { get; set; }
        public bool? IsDeleted { get; set; }




    }


    public class GetPagedMainClauseModel
    {
        public int TotalCount { get; set; }
        public List<MainClauseModel> MainClauseModel { get; set; }
    }
}

