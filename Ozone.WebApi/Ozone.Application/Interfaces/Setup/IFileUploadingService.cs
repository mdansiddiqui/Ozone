using Ozone.Application.DTOs;
using Ozone.Application.DTOs.Setup;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ozone.Application.Interfaces.Setup
{
   public interface IFileUploadingService
    {

         Task<string> Create(FileUploadingModel input);
        Task<GetPagedFileUploadingModel> GetPagedFiles(long id, PagedResponseModel model);

        Task<FileUploadingModel> DownloadClientChange(long id);
        Task<string> FileDeleteById(long id);

    }
}
