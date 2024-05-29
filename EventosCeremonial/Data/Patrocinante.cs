using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace EventosCeremonial.Data
{
    [Table("Patrocinante")]
    public partial class Patrocinante
    {
        [Key]
        public int Id { get; set; }
        public int IdEvento { get; set; }
        public int? IdOrganismo { get; set; }
        [StringLength(255)]
        public string AreaInterna { get; set; }

        [ForeignKey(nameof(IdEvento))]
        [InverseProperty(nameof(Evento.Patrocinantes))]
        public virtual Evento IdEventoNavigation { get; set; }
    }
}
