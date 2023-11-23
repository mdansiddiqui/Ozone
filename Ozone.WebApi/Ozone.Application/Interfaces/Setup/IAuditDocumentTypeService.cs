using Ozone.Application.DTOs;
using Ozone.Application.DTOs.Setup;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ozone.Application.Interfaces.Setup
{
    public interface IAuditDocumentTypeService
    {
      
        Task<string> Create(AuditDocumetTypeModel input);
        Task<GetPagedAuditDocumentTypeModel>GetPagedAuditDocumentTypeResponse(PagedResponseModel model);
        Task<AuditDocumetTypeModel> GetAuditDocumetTypeBYId(long id);
        Task<string> AuditDocumetTypeDeleteById(long id);
    }
}
