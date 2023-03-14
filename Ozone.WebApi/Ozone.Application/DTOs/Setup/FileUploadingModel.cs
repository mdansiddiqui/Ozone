using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ozone.Application.DTOs.Setup
{
    public class FileUploadingModel
    {

        public long Id { get; set; }
        public long? AgencyId { get; set; }
        public string AgencyName { get; set; }
        public long? FromUserId { get; set; }
        public string FromUserName { get; set; }

        public long? ToUserId { get; set; }
        public string ToUserIdName { get; set; }

        public IFormFile File { get; set; }
        public string FilePath { get; set; }
        public string FileContentType { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsClosed { get; set; }
        public string Description { get; set; }

    }

    public class GetPagedFileUploadingModel
    { public int TotalCount { get; set; }
        public List<FileUploadingModel> FileUploadingModel { get; set; }
    }
}
