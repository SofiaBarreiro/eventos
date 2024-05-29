using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace EventosCeremonial.Data
{
    [Table("Invitacion")]
    public partial class Invitacion
    {
        public Invitacion()
        {
            EventoParticipantes = new HashSet<EventoParticipante>();
        }

        [Key]
        public int Id { get; set; }
        public int? IdPersona { get; set; }
        public int? IdOrganismo { get; set; }
        public int IdEvento { get; set; }
        [Required ]
        [StringLength(150)]
        public string TipoInvitado { get; set; }
        public Guid? Qr { get; set; }

        [ForeignKey(nameof(IdEvento))]
        [InverseProperty(nameof(Evento.Invitacions))]
        public virtual Evento IdEventoNavigation { get; set; }
        [InverseProperty(nameof(EventoParticipante.IdInvitacionNavigation))]
        public virtual ICollection<EventoParticipante> EventoParticipantes { get; set; }

        public static implicit operator Invitacion(List<Invitacion> v)
        {
            throw new NotImplementedException();
        }
    }
}
