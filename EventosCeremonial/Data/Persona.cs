using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace EventosCeremonial.Data
{
    [Table("Persona")]
    public partial class Persona
    {
        public Persona()
        {
            EventoParticipantes = new HashSet<EventoParticipante>();
        }

        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        [StringLength(150)]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El campo Apellido es obligatorio")]
        [StringLength(150)]
        public string Apellido { get; set; }
        [Required(ErrorMessage = "El campo Correo electrónico es obligatorio")]
        [StringLength(150)]
        public string MailContacto { get; set; }
        [StringLength(50)]
        public string TelefonoContacto { get; set; }
        [StringLength(150)]
        public string Puesto { get; set; }
        public int? IdOrganismo { get; set; }
        [StringLength(10)]
        public string TipoDocumento { get; set; }
        [StringLength(15)]
        public string NumeroDocumento { get; set; }

        [ForeignKey(nameof(IdOrganismo))]
        [InverseProperty(nameof(Organismo.Personas))]
        public virtual Organismo IdOrganismoNavigation { get; set; }
        [InverseProperty(nameof(EventoParticipante.IdPersonaNavigation))]
        public virtual ICollection<EventoParticipante> EventoParticipantes { get; set; }


        //public string tipo { get; set; }




    }
}
