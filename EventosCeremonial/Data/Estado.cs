using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace EventosCeremonial.Data
{
    [Table("Estado")]
    public partial class Estado
    {
        public Estado()
        {
            Eventos = new HashSet<Evento>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string Nombre { get; set; }
        [Required]
        [StringLength(2)]
        public string Activo { get; set; }

        [InverseProperty(nameof(Evento.IdEstadoNavigation))]
        public virtual ICollection<Evento> Eventos { get; set; }
    }
}
