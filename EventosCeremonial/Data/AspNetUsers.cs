using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable


namespace EventosCeremonial.Data
{

    [Table("AspNetUsers")]
    public partial class AspNetUsers
    {

        [Key]
        public string Id { get; set; }

        public string Email { get; set; }

        public string NormalizedEmail { get; set; }

        public string NormalizedUserName { get; set; }

        public string UserName { get; set; }

        public virtual ICollection<AspNetUsers> AspNetUserss { get; set; }

    }
}
