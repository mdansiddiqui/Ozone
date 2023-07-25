using Ozone.Application.DTOs;
using Ozone.Application.DTOs.Projects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ozone.Application.Interfaces.Setup
{
  public  interface IDocumentTypeService
    {
        Task<string> Create(DocumentsTypeModel input);
        Task<GetPagedDocumentsTypeModel> GetPagedDocumentsTypeResponse(PagedResponseModel model);
        Task<DocumentsTypeModel> GetDocumentsTypeBYId(long id);
        Task<string> DocumentTypeDeleteById(long id);
        Task<string> DelMSWD(long id);
        Task<string> createMSWD(MappingDocumentsWithStandardModel input);
        Task<List<MappingDocumentsWithStandardModel>> GetMSWD(PagedResponseModel model);
    }
}
