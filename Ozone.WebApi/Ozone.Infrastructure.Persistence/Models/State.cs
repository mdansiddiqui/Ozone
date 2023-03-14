using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Ozone.Infrastructure.Persistence.Models
{
    public partial class State
    {
        public State()
        {
            Cities = new HashSet<Cities>();
            Client = new HashSet<Client>();
            ClientSites = new HashSet<ClientSites>();
            Consultant = new HashSet<Consultant>();
            Organization = new HashSet<Organization>();
            SecUser = new HashSet<SecUser>();
        }

        [Key]
        public long Id { get; set; }
        [StringLength(50)]
        public string Code { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public long? CountryId { get; set; }

        [ForeignKey(nameof(CountryId))]
        [InverseProperty(nameof(Countries.State))]
        public virtual Countries Country { get; set; }
        [InverseProperty("State")]
        public virtual ICollection<Cities> Cities { get; set; }
        [InverseProperty("State")]
        public virtual ICollection<Client> Client { get; set; }
        [InverseProperty("State")]
        public virtual ICollection<ClientSites> ClientSites { get; set; }
        [InverseProperty("State")]
        public virtual ICollection<Consultant> Consultant { get; set; }
        [InverseProperty("State")]
        public virtual ICollection<Organization> Organization { get; set; }
        [InverseProperty("State")]
        public virtual ICollection<SecUser> SecUser { get; set; }
    }
}
