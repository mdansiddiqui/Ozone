using System;
using System.Collections.Generic;
using System.Text;

namespace Ozone.Application.DTOs.Setup
{
    public class GetPagedAuditDocumentTypeModel
    {
        public int TotalCount { get; set; }
        public List<AuditDocumetTypeModel> AuditDocumetTypeModel { get; set; }
    }
}
