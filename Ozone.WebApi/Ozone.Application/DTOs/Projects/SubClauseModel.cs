using Ozone.Infrastructure.Persistence.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ozone.Application.DTOs.Projects
{
    public class SubClauseModel
    {

        public long Id { get; set; }
        public long StandardId { get; set; }
        public string StandardName { get; set; }

        public long MainClauseId { get; set; }
        public string MainClauseName { get; set; }
        public string SubClauseName { get; set; }
        public string Requirement { get; set; }
        public long CreatedById { get; set; }

        public DateTime? CretedByDate { get; set; }
        public long LastModifiedById { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class GetPagedSubClauseModel
    {
        public int TotalCount { get; set; }
        public List<SubClauseModel> SubClauseModel { get; set; }
    }
}
