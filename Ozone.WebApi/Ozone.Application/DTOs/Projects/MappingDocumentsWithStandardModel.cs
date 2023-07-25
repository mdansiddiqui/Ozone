using System;
using System.Collections.Generic;
using System.Text;

namespace Ozone.Application.DTOs.Projects
{
    public class MappingDocumentsWithStandardModel
    {
        public long Id { get; set; }
        public long? StandardId { get; set; }
        public long? DocumentTypeId { get; set; }
        public long? VisitLevelId { get; set; }
        public string StandardName { get; set; }
        public string DocumentName { get; set; }
        public string VisitLevelName { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsRequired { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? DocumentForReviewer { get; set; }
        public int? DocumentAssignId { get; set; }
        public string DocumentAssignName { get; set; }
    }
}
