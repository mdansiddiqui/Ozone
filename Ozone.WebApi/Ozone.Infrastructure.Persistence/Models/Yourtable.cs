using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Ozone.Infrastructure.Persistence.Models
{
    [Keyless]
    [Table("yourtable")]
    public partial class Yourtable
    {
        [Column("ID")]
        public int? Id { get; set; }
        [StringLength(5)]
        public string User { get; set; }
        [StringLength(8)]
        public string Department { get; set; }
    }
}
