using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Ozone.Infrastructure.Persistence.Models
{
    public partial class TypeOfEnrollment
    {
        public TypeOfEnrollment()
        {
            SecUser = new HashSet<SecUser>();
        }

        [Key]
        public long Id { get; set; }
        public string Name { get; set; }

        [InverseProperty("TypeOfEnrollment")]
        public virtual ICollection<SecUser> SecUser { get; set; }
    }
}
