using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Ozone.Infrastructure.Persistence.Models
{
    public partial class DocumentAssignTo
    {
        public DocumentAssignTo()
        {
            MappingDocumentsWithStandard = new HashSet<MappingDocumentsWithStandard>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        [InverseProperty("DocumentAssign")]
        public virtual ICollection<MappingDocumentsWithStandard> MappingDocumentsWithStandard { get; set; }
    }
}
