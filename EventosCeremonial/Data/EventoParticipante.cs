using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace EventosCeremonial.Data
{
    [Table("EventoParticipante")]
    public partial class EventoParticipante
    {
        [Key]
        public int Id { get; set; }
        public int IdPersona { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime FechaHoraIngreso { get; set; }
        [StringLength(2)]
        public string ConfirmaAsistencia { get; set; }
        public int? IdPuntoAcceso { get; set; }
        [StringLength(15)]
        public string EstadodeInscripcion { get; set; }
        public int IdInvitacion { get; set; }

        [ForeignKey(nameof(IdInvitacion))]
        [InverseProperty(nameof(Invitacion.EventoParticipantes))]

        public virtual Invitacion IdInvitacionNavigation { get; set; }
        [ForeignKey(nameof(IdPersona))]
        [InverseProperty(nameof(Persona.EventoParticipantes))]
        public virtual Persona IdPersonaNavigation { get; set; }
    }
}
