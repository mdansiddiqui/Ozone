using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Ozone.Infrastructure.Persistence.Models
{
    public partial class FileUploading
    {
        [Key]
        public long Id { get; set; }
        public long? AgencyId { get; set; }
        public long? FromUserId { get; set; }
        public long? ToUserId { get; set; }
        public string FilePath { get; set; }
        public string FileContentType { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsClosed { get; set; }
        public string Description { get; set; }

        [ForeignKey(nameof(AgencyId))]
        [InverseProperty(nameof(Organization.FileUploading))]
        public virtual Organization Agency { get; set; }
        [ForeignKey(nameof(CreatedById))]
        [InverseProperty(nameof(SecUser.FileUploadingCreatedBy))]
        public virtual SecUser CreatedBy { get; set; }
        [ForeignKey(nameof(FromUserId))]
        [InverseProperty(nameof(SecUser.FileUploadingFromUser))]
        public virtual SecUser FromUser { get; set; }
        [ForeignKey(nameof(ToUserId))]
        [InverseProperty(nameof(SecUser.FileUploadingToUser))]
        public virtual SecUser ToUser { get; set; }
    }
}
